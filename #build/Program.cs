using Cake.Frosting;

namespace Build
{
    public class Program : IFrostingStartup
    {
        public static void Main(string[] args) => new CakeHostBuilder()
            .WithArguments(args)
            .UseStartup<Program>()
            .Build()
            .Run();

        public void Configure(ICakeServices services)
        {
            services.UseContext<Context>();
            services.UseLifetime<Lifetime>();
            services.UseWorkingDirectory("..");
        }
    }
}
