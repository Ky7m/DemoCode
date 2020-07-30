using System.Threading.Tasks;
using Pulumi;

namespace PulumiDemo
{
    internal static class Program
    {
        private static Task<int> Main() => Deployment.RunAsync<CommonInfrastructureTemplate>();
    }
}
