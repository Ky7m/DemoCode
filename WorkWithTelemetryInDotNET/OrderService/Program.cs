using OrderService;
using Shared.Extensions;
using Shared.Metrics;

var host = Host.CreateDefaultBuilder(args)
    .UseSerilogDefaults()
    .ConfigureServices((context, services) =>
        services
            .AddMassTransitSharedConfiguration(x =>
            {
                x.AddConsumer<OrderConsumer>();
            })    
            .AddOpenTelemetryDefaults(context.Configuration, context.HostingEnvironment)
            .AddSingleton<OrderMetrics>()
    )
    .Build();

await host.RunAsync();