using System.Collections.Generic;
using Azure.Data.AppConfiguration;
using Pulumi;
using Pulumi.Azure.AppConfiguration;
using Pulumi.Azure.AppService;
using Pulumi.Azure.AppService.Inputs;
using Pulumi.Azure.Core;
using Pulumi.Azure.FrontDoor.Inputs;
using Pulumi.Azure.Storage;
using Pulumi.AzureAD;
using Pulumi.Random;

namespace PulumiDemo
{
    public class CommonInfrastructureTemplate : Stack
    {
        public CommonInfrastructureTemplate()
        {
            var config = new Pulumi.Config("CommonInfrastructureTemplate");

            var defaultLocation = config.Require("DefaultLocation");
            var defaultName = config.Require("DefaultName");

            var resourceGroup = new ResourceGroup(defaultName, new ResourceGroupArgs
            {
                Name = defaultName,
                Location = defaultLocation
            });

            var settings = new Dictionary<(string, string), Output<string>>();

            var configurationStore = new ConfigurationStore(defaultName, new ConfigurationStoreArgs
            {
                Name = defaultName,
                ResourceGroupName = resourceGroup.Name,
                Location = defaultLocation,
                Sku = "standard"
            });
            var configurationStoreReadOnlyConnectionString = configurationStore.PrimaryReadKeys.Apply(array => array[0].ConnectionString);
            var configurationStoreReadWriteConnectionString = configurationStore.PrimaryWriteKeys.Apply(array => array[0].ConnectionString);

            var storageAccountName = config.Get("StorageAccountName");
            if (!string.IsNullOrEmpty(storageAccountName))
            {
                var connectionString = CreateStorageAccountIfEnabled(storageAccountName, resourceGroup, defaultLocation);
                settings.Add(("StorageConnectionString", null), connectionString);
            }

            var appSlots = config.GetObject<string[]>("AppSlots");
            var productionBackendPoolList = new InputList<FrontdoorBackendPoolBackendArgs>();
            var slotBackendPoolList = new Dictionary<string, InputList<FrontdoorBackendPoolBackendArgs>>(appSlots.Length);
            var deploymentRegions = config.GetObject<string[]>("DeploymentRegions");
            foreach (var deploymentRegion in deploymentRegions)
            {
                var planName = $"{defaultName}-{deploymentRegion}-plan";
                var appServicePlan = new Plan(planName, new PlanArgs
                {
                    Name = planName,
                    ResourceGroupName = resourceGroup.Name,
                    Kind = "Linux",
                    Reserved = true,
                    Sku = new PlanSkuArgs
                    {
                        Tier = "Standard",
                        Size = "S1",
                    },
                    Location = deploymentRegion
                });

                var appName = $"{defaultName}-{deploymentRegion}";
                var app = new AppService(appName, new AppServiceArgs
                {
                    Name = appName,
                    Location = deploymentRegion,
                    ResourceGroupName = resourceGroup.Name,
                    AppServicePlanId = appServicePlan.Id,
                    AppSettings =
                    {
                        {"AppConfiguration__ConnectionString", configurationStoreReadOnlyConnectionString},
                        {"AppConfiguration__Environment", "production"}
                    },
                    SiteConfig = new AppServiceSiteConfigArgs
                    {
                    }
                });

                productionBackendPoolList.Add(new FrontdoorBackendPoolBackendArgs
                {
                    Address = app.DefaultSiteHostname,
                    HostHeader = app.DefaultSiteHostname,
                    HttpPort = 80,
                    HttpsPort = 443
                });

                foreach (var appSlot in appSlots)
                {
                    var appSlotName = $"{appName}-{appSlot}";
                    var slot = new Slot(appSlotName, new SlotArgs
                    {
                        Name = appSlot,
                        Location = deploymentRegion,
                        AppServiceName = app.Name,
                        ResourceGroupName = resourceGroup.Name,
                        AppServicePlanId = appServicePlan.Id,
                        AppSettings =
                        {
                            {"AppConfiguration__ConnectionString", configurationStoreReadOnlyConnectionString},
                            {"AppConfiguration__Environment", appSlot}
                        }
                    });

                    var slotRecord = new FrontdoorBackendPoolBackendArgs
                    {
                        Address = slot.DefaultSiteHostname,
                        HostHeader = slot.DefaultSiteHostname,
                        HttpPort = 80,
                        HttpsPort = 443
                    };

                    if (slotBackendPoolList.TryGetValue(appSlot, out var slotList))
                    {
                        slotList.Add(slotRecord);
                    }
                    else
                    {
                        slotBackendPoolList.Add(appSlot, new InputList<FrontdoorBackendPoolBackendArgs>
                        {
                            slotRecord
                        });
                    }
                }
            }

            foreach (var appSlot in appSlots)
            {
                var resourceName = $"{defaultName}-{appSlot}";
                var slotAppInsights = new AppInsightsResource(resourceName, resourceGroup.Name);
                settings.Add(("ApplicationInsights:InstrumentationKey", appSlot), slotAppInsights.InstrumentationKey);

                var (slotMicrosoftAppId, slotMicrosoftAppPassword) = CreateMicrosoftApp(resourceName);
                settings.Add(("MicrosoftAppId", appSlot), slotMicrosoftAppId);
                settings.Add(("MicrosoftAppPassword", appSlot), slotMicrosoftAppPassword);

                var _ = new FrontDoorResource(resourceName, resourceGroup.Name ,defaultName, slotBackendPoolList[appSlot]);
            }

            var (productionMicrosoftAppId, productionMicrosoftAppPassword) = CreateMicrosoftApp(defaultName);
            settings.Add(("MicrosoftAppId", "production"), productionMicrosoftAppId);
            settings.Add(("MicrosoftAppPassword", "production"), productionMicrosoftAppPassword);

            var appInsights = new AppInsightsResource(defaultName, resourceGroup.Name);
            settings.Add(("ApplicationInsights:InstrumentationKey", "production"), appInsights.InstrumentationKey);

            var frontDoorResource = new FrontDoorResource(defaultName, resourceGroup.Name, defaultName, productionBackendPoolList);

            configurationStoreReadWriteConnectionString.Apply(connectionString =>
            {
                var configurationStoreClient = new ConfigurationClient(connectionString);
                foreach (var item in settings)
                {
                    item.Value.Apply(value =>
                    {
                        var (key, label) = item.Key;
                        var setting = new ConfigurationSetting(key, value, label);
                        configurationStoreClient.SetConfigurationSetting(setting);
                        return value;
                    });
                }

                return connectionString;
            });


            MainUrl = frontDoorResource.Cname;
        }

        private (Output<string>, Output<string>) CreateMicrosoftApp(string name)
        {
            var msa = new Application(name, new ApplicationArgs
            {
                Name = name,
                Oauth2AllowImplicitFlow = false,
                AvailableToOtherTenants = true,
                PublicClient = true
            });

            var pwd = new RandomPassword($"{name}-password", new RandomPasswordArgs
            {
                Length = 16,
                MinNumeric = 1,
                MinSpecial = 1,
                MinUpper = 1,
                MinLower = 1
            });

            var msaSecret = new ApplicationPassword($"{name}-secret", new ApplicationPasswordArgs {ApplicationObjectId = msa.ObjectId, EndDate = "2299-12-29T00:00:00Z", Value = pwd.Result});

            return (msa.ApplicationId, msaSecret.Value);
        }
        
        private Output<string> CreateStorageAccountIfEnabled(string storageAccountName, ResourceGroup resourceGroup, string defaultLocation)
        {
            var storageAccount = new Account(storageAccountName, new AccountArgs
            {
                Name = storageAccountName,
                ResourceGroupName = resourceGroup.Name,
                AccountReplicationType = "LRS",
                AccountTier = "Premium",
                Location = defaultLocation,
                AccountKind = "BlockBlobStorage"
            });
            return storageAccount.PrimaryConnectionString;
        }

        [Output]
        public Output<string> MainUrl { get; set; }
    }
}
