using Pulumi;
using Pulumi.Azure.AppInsights;

namespace PulumiDemo
{
    public class AppInsightsResource : ComponentResource
    {
        [Output] public Output<string> InstrumentationKey { get; private set; }
        public AppInsightsResource(string resourceName, Input<string> resourceGroupName, ComponentResourceOptions options = null) 
            : base("resource:azure:app-insights", resourceName, options)
        {
            var appInsights = new Insights(resourceName, new InsightsArgs {Name = resourceName, ResourceGroupName = resourceGroupName, ApplicationType = "web"});
            InstrumentationKey = appInsights.InstrumentationKey;
        }
    }
}