using System.Collections.Generic;
using Azure.Data.AppConfiguration;
using Pulumi;
using Pulumi.AzureNative.AppConfiguration;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

namespace PulumiDemo
{
    public class AppStack : Stack
    {
        public AppStack()
        {
            var config = new Config("CommonInfrastructureTemplate");

            var defaultLocation = config.Require("DefaultLocation");
            var defaultName = config.Require("DefaultName");

            var resourceGroup = new ResourceGroup(defaultName, new ResourceGroupArgs
            {
                ResourceGroupName = defaultName,
                Location = defaultLocation
            });

            var settings = new Dictionary<(string, string), Output<string>>();

            var configurationStore = new ConfigurationStore(defaultName, new ConfigurationStoreArgs
            {
                ConfigStoreName = defaultName,
                ResourceGroupName = resourceGroup.Name,
                Location = defaultLocation,
                Sku = new Pulumi.AzureNative.AppConfiguration.Inputs.SkuArgs
                {
                    Name = "Standard",
                }
            });

            var configurationStoreKeys = ListConfigurationStoreKeys.Invoke(new ListConfigurationStoreKeysInvokeArgs
            {
                ConfigStoreName = configurationStore.Name,
                ResourceGroupName = resourceGroup.Name
            });
            
            var configurationStoreReadWriteConnectionString = configurationStoreKeys.Apply(x => x.Value[0].ConnectionString);
            var configurationStoreReadOnlyConnectionString = configurationStoreKeys.Apply(x => x.Value[2].ConnectionString);
            
            var storageAccountName = config.Get("StorageAccountName");
            if (!string.IsNullOrEmpty(storageAccountName))
            {
                var connectionString = CreateStorageAccountIfEnabled(storageAccountName, resourceGroup, defaultLocation);
                settings.Add(("StorageConnectionString", null), connectionString);
            }
            
            var appSlots = config.GetObject<string[]>("AppSlots");
            var deploymentRegions = config.GetObject<string[]>("DeploymentRegions");
            foreach (var deploymentRegion in deploymentRegions)
            {
                var planName = $"{defaultName}-{deploymentRegion}-plan";
                var appServicePlan = new AppServicePlan(planName, new AppServicePlanArgs
                {
                    Name = planName,
                    ResourceGroupName = resourceGroup.Name,
                    Kind = "Linux",
                    Reserved = true,
                    Sku = new SkuDescriptionArgs
                    {
                        Tier = "Standard",
                        Name = "S1",
                    },
                    Location = deploymentRegion
                });
            
                var appName = $"{defaultName}-{deploymentRegion}";
                var app = new WebApp(appName, new WebAppArgs
                {
                    Name = appName,
                    Location = deploymentRegion,
                    ResourceGroupName = resourceGroup.Name,
                    ServerFarmId = appServicePlan.Id,
                    SiteConfig = new SiteConfigArgs
                    {
                        AppSettings =
                        {
                            new NameValuePairArgs
                            {
                                Name = "AppConfiguration__ConnectionString",
                                Value = configurationStoreReadOnlyConnectionString
                            },
                            new NameValuePairArgs
                            {
                                Name = "AppConfiguration__Environment",
                                Value = "production",
                            }
                        }
                    }
                });

                foreach (var appSlot in appSlots)
                {
                    var appSlotName = $"{appName}-{appSlot}";
                    var slot = new WebAppSlot(appSlotName, new WebAppSlotArgs
                    {
                        Name = app.Name,
                        Slot = appSlot,
                        Location = deploymentRegion,
                        ResourceGroupName = resourceGroup.Name,
                        SiteConfig = new SiteConfigArgs
                        {
                            AppSettings =
                            {
                                new NameValuePairArgs{
                                    Name = "AppConfiguration__ConnectionString",
                                    Value = configurationStoreReadOnlyConnectionString
                                },
                                new NameValuePairArgs{
                                    Name = "AppConfiguration__Environment",
                                    Value = appSlot
                                }
                            }
                        }
                    });
                }
            }
            
            foreach (var appSlot in appSlots)
            {
                var resourceName = $"{defaultName}-{appSlot}";
                var slotAppInsights = new AppInsightsResource(resourceName, resourceGroup.Name);
                settings.Add(("ApplicationInsights:ConnectionString", appSlot), slotAppInsights.ConnectionString);
            }

            var appInsights = new AppInsightsResource(defaultName, resourceGroup.Name);
            settings.Add(("ApplicationInsights:ConnectionString", "production"), appInsights.ConnectionString);
            
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
        }

        private static Output<string> CreateStorageAccountIfEnabled(string storageAccountName, ResourceGroup resourceGroup, string defaultLocation)
        {
            var storageAccount = new StorageAccount(storageAccountName, new StorageAccountArgs
            {
                AccountName = storageAccountName,
                Sku = new Pulumi.AzureNative.Storage.Inputs.SkuArgs
                {
                    Name = SkuName.Premium_LRS
                },
                ResourceGroupName = resourceGroup.Name,
                Location = defaultLocation,
                Kind = Kind.BlockBlobStorage
            });
            // Retrieve the primary storage account key.
            var storageAccountKeys = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
            {
                ResourceGroupName = resourceGroup.Name,
                AccountName = storageAccount.Name
            });

            return storageAccountKeys.Apply(keys =>
            {
                var primaryStorageKey = keys.Keys[0].Value;
                // Build the connection string to the storage account.
                return Output.Format($"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={primaryStorageKey}");
            });
        }
    }
}
