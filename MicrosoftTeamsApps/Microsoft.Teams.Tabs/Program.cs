using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Microsoft.Teams.Tabs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .Configure(app => app.UseDefaultFiles().UseStaticFiles())
                .Build()
                .Run();
        }
    }
}
