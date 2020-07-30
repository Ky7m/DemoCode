using Pulumi;
using Pulumi.Azure.Core;
using PulumiDemo;

namespace PulumiDemoTests
{
    public class AppInsightsResourceTestStack : Stack
    {
        public AppInsightsResourceTestStack()
        {
            var resourceGroup = new ResourceGroup("www-prod-rg");
            var _ = new AppInsightsResource("appi", resourceGroup.Name);
        }
    }
}