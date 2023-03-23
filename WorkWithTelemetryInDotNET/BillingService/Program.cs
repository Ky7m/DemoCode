using Shared.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .UseSharedSerilogConfiguration()
    .UseNServiceBus(_ =>
    {
        var endpointConfiguration = new EndpointConfiguration(nameof(BillingService));
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=localhost");
        transport.UseConventionalRoutingTopology(QueueType.Quorum);
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableOpenTelemetry();
        return endpointConfiguration;
    })
    .ConfigureServices((context, services) =>
        services.AddOpenTelemetrySharedConfiguration(context.Configuration, context.HostingEnvironment.ApplicationName)
    )
    .Build();

await host.RunAsync();