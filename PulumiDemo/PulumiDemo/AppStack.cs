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
            var settings = new Dictionary<(string, string), Output<string>>();

            var config = new Config();
            var name = config.Get("name");
            var appSlots = config.GetObject<string[]>("appSlots");
            var deploymentRegions = config.GetObject<string[]>("deploymentRegions");

            var rg = new ResourceGroup($"rg-{name}-");
            
            var appcs = new ConfigurationStore($"appcs-{name}-", new ConfigurationStoreArgs
            {
                ResourceGroupName = rg.Name,
                Sku = new Pulumi.AzureNative.AppConfiguration.Inputs.SkuArgs
                {
                    Name = "Standard",
                }
            });
            
            var appcsKeys = ListConfigurationStoreKeys.Invoke(new ListConfigurationStoreKeysInvokeArgs
            {
                ConfigStoreName = appcs.Name,
                ResourceGroupName = rg.Name
            });
            
            var appcsReadWriteConnectionString = appcsKeys.Apply(x => x.Value[0].ConnectionString);
            var appcsReadOnlyConnectionString = appcsKeys.Apply(x => x.Value[2].ConnectionString);
            
            var stConnectionString = CreateStorageAccount($"st{name}", rg);
            settings.Add(("StorageConnectionString", null), stConnectionString);
            
            foreach (var deploymentRegion in deploymentRegions)
            {
                var aspName = $"asp-{name}-{deploymentRegion}-";
                var asp = new AppServicePlan(aspName, new AppServicePlanArgs
                {
                    ResourceGroupName = rg.Name,
                    Kind = "Linux",
                    Reserved = true,
                    Sku = new SkuDescriptionArgs
                    {
                        Tier = "Standard",
                        Name = "S1",
                    },
                    Location = deploymentRegion
                });
            
                var appName = $"app-{name}-{deploymentRegion}-";
                var app = new WebApp(appName, new WebAppArgs
                {
                    Location = deploymentRegion,
                    ResourceGroupName = rg.Name,
                    ServerFarmId = asp.Id,
                    SiteConfig = new SiteConfigArgs
                    {
                        AppSettings =
                        {
                            new NameValuePairArgs
                            {
                                Name = "AppConfiguration__ConnectionString",
                                Value = appcsReadOnlyConnectionString
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
                    var appSlotName = $"appsl-{name}-{deploymentRegion}-{appSlot}-";
                    _ = new WebAppSlot(appSlotName, new WebAppSlotArgs
                    {
                        Name = app.Name,
                        Slot = appSlot,
                        Location = deploymentRegion,
                        ResourceGroupName = rg.Name,
                        SiteConfig = new SiteConfigArgs
                        {
                            AppSettings =
                            {
                                new NameValuePairArgs{
                                    Name = "AppConfiguration__ConnectionString",
                                    Value = appcsReadOnlyConnectionString
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
                var appiName = $"appi-{name}-{appSlot}-";
                var appiSlot = new AppInsightsResource(appiName, rg.Name);
                settings.Add(("ApplicationInsights:ConnectionString", appSlot), appiSlot.ConnectionString);
            }
            
            var appi = new AppInsightsResource($"appi-{name}-", rg.Name);
            settings.Add(("ApplicationInsights:ConnectionString", "production"), appi.ConnectionString);
            
            appcsReadWriteConnectionString.Apply(connectionString =>
            {
                var appcsClient = new ConfigurationClient(connectionString);
                foreach (var item in settings)
                {
                    item.Value.Apply(value =>
                    {
                        var (key, label) = item.Key;
                        var setting = new ConfigurationSetting(key, value, label);
                        appcsClient.SetConfigurationSetting(setting);
                        return value;
                    });
                }
            
                return connectionString;
            });
        }

        private static Output<string> CreateStorageAccount(string storageAccountName, ResourceGroup resourceGroup)
        {
            var st = new StorageAccount(storageAccountName, new StorageAccountArgs
            {
                Sku = new Pulumi.AzureNative.Storage.Inputs.SkuArgs
                {
                    Name = SkuName.Standard_LRS
                },
                ResourceGroupName = resourceGroup.Name,
                Kind = Kind.StorageV2
            });
            // Retrieve the primary storage account key.
            var stKeys = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
            {
                ResourceGroupName = resourceGroup.Name,
                AccountName = st.Name
            });
            
            return stKeys.Apply(keys =>
            {
                var primaryStorageKey = keys.Keys[0].Value;
                // Build the connection string to the storage account.
                return Output.Format($"DefaultEndpointsProtocol=https;AccountName={st.Name};AccountKey={primaryStorageKey}");
            });
        }
    }
}
