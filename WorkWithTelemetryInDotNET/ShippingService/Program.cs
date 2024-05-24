using Shared.Extensions;
using ShippingService;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
        services
            .AddMassTransitSharedConfiguration(x =>
            {
                x.AddConsumer<OrderPlacedConsumer>()
                    .Endpoint(x => x.Name = "shipping");
            })
            .AddOpenTelemetryDefaults(context.Configuration, context.HostingEnvironment)
    )
    .Build();

await host.RunAsync();