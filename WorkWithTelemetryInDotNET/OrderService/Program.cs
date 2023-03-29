using OrderService;
using Shared.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .UseSharedSerilogConfiguration()
    .ConfigureServices((context, services) =>
        services
            .AddMassTransitSharedConfiguration(x =>
            {
                x.AddConsumer<OrderConsumer>();
            })    
            .AddOpenTelemetrySharedConfiguration(context.Configuration, context.HostingEnvironment.ApplicationName)
    )
    .Build();

await host.RunAsync();