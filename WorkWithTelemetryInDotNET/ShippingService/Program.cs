using Shared.Extensions;
using ShippingService;

var host = Host.CreateDefaultBuilder(args)
    .UseSharedSerilogConfiguration()
    .ConfigureServices((context, services) =>
        services
            .AddMassTransitSharedConfiguration(x =>
            {
                x.AddConsumer<OrderPlacedConsumer>()
                    .Endpoint(x => x.Name = "shipping");
            })
            .AddOpenTelemetrySharedConfiguration(context.Configuration, context.HostingEnvironment.ApplicationName)
    )
    .Build();

await host.RunAsync();