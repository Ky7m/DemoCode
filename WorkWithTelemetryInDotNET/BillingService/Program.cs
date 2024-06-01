using BillingService;
using Shared.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
        services
            .AddMassTransitSharedConfiguration(x =>
            {
                x.AddConsumer<OrderPlacedConsumer>()
                    .Endpoint(endpoint => endpoint.Name = "billing");
            })    
            .AddOpenTelemetryDefaults(context.Configuration, context.HostingEnvironment)
    )
    .Build();

await host.RunAsync();