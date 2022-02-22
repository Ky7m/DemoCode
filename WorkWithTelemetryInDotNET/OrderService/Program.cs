using NServiceBus;
using Shared.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .UseSharedSerilogConfiguration()
    .UseNServiceBus(_ =>
    {
        var endpointConfiguration = new EndpointConfiguration(nameof(OrderService));
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=localhost");
        transport.UseConventionalRoutingTopology();
        endpointConfiguration.EnableInstallers();
        return endpointConfiguration;
    })
    .ConfigureServices((context, services) =>
        services.AddSharedOpenTelemetryTracing(context.Configuration, context.HostingEnvironment.ApplicationName)
    )
    .Build();

await host.RunAsync();