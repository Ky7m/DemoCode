using Pulumi;
using Pulumi.AzureNative.Insights.V20200202;
using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.OperationalInsights.Inputs;

namespace PulumiDemo
{
    public class AppInsightsResource : ComponentResource
    {
        [Output] public Output<string> ConnectionString { get; private set; }
        public AppInsightsResource(string resourceName, Input<string> resourceGroupName, ComponentResourceOptions options = null) 
            : base("pulumidemo:azure:app-insights", resourceName, options)
        {
            var log = new Workspace(resourceName.Replace("appi-","log-"), new()
            {
                ResourceGroupName = resourceGroupName,
                Sku = new WorkspaceSkuArgs
                {
                    Name = "PerGB2018",
                },
                RetentionInDays = 30,
            });
            var appi = new Component(resourceName, new ComponentArgs
            {
                ResourceGroupName = resourceGroupName, 
                Kind = "web",
                ApplicationType = ApplicationType.Web,
                IngestionMode = IngestionMode.LogAnalytics,
                WorkspaceResourceId = log.Id
            });
            
            ConnectionString = appi.ConnectionString;
        }
    }
}