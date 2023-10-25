using OrderService;
using Shared.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .UseSerilogDefaults()
    .ConfigureServices((context, services) =>
        services
            .AddMassTransitSharedConfiguration(x =>
            {
                x.AddConsumer<OrderConsumer>();
            })    
            .AddOpenTelemetryDefaults(context.Configuration, context.HostingEnvironment)
    )
    .Build();

await host.RunAsync();