using Pulumi;
using Pulumi.AzureNative.Insights;

namespace PulumiDemo
{
    public class AppInsightsResource : ComponentResource
    {
        [Output] public Output<string> ConnectionString { get; private set; }
        public AppInsightsResource(string resourceName, Input<string> resourceGroupName, ComponentResourceOptions options = null) 
            : base("resource:azure:app-insights", resourceName, options)
        {
            var appInsights = new Component(resourceName, new ComponentArgs
            {
                ResourceName = resourceName,
                ResourceGroupName = resourceGroupName, 
                Kind = "web"
            });
            ConnectionString = appInsights.ConnectionString;
        }
    }
}