using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Pulumi;
using Pulumi.Azure.AppInsights;
using Pulumi.Azure.Core;
using Pulumi.Testing;
using Xunit;

namespace PulumiDemoTests
{
    public class AppInsightsResourceTests
    {
	    [Fact]
	    public async Task AppInsightsResourceExists()
	    {
		    var resources = await Deployment.TestAsync<AppInsightsResourceTestStack>(new Mocks(), new TestOptions {StackName = "dev"});

		    var resourceGroups = resources.OfType<ResourceGroup>().ToList();
		    resourceGroups.Count.Should().Be(1, "a single resource group is expected");

		    var appInsights = resources.OfType<Insights>().ToList();
		    appInsights.Count.Should().Be(1, "a single app insights instance is expected");
	    }
    }
}