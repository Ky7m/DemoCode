using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpLogging;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;
using OrderGenerator;
using Serilog;
using Serilog.Events;
using Shared;
using Shared.Extensions;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host
        .UseSharedSerilogConfiguration()
        .UseNServiceBus(_ =>
        {
            var endpointConfiguration = new EndpointConfiguration(nameof(OrderGenerator));
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            transport.UseConventionalRoutingTopology();
            transport.Routing()
                .RouteToEndpoint(
                messageType: typeof(PlaceOrder),
                destination: "OrderService"
            );
            transport.Routing()
                .RouteToEndpoint(
                messageType: typeof(CancelOrder),
                destination: "OrderService"
            );
            endpointConfiguration.EnableInstallers();
            
            var settings = endpointConfiguration.GetSettings();
            settings.Set(new NServiceBus.Extensions.Diagnostics.InstrumentationOptions
            {
                CaptureMessageBody = true
            });
            
            return endpointConfiguration;
        });

    builder.Services.AddSharedOpenTelemetryTracing(builder.Configuration, builder.Environment.ApplicationName);
    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestHeaders.Add("X-Correlation-ID");
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
    });
    
    builder.Services.AddHostedService<OrderGeneratorWorker>();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();
    app.UseHttpsRedirection();
    
    app.UseHttpLogging();
    app.UseSerilogRequestLogging();
    
    app.MapPost("/api/orders", async (PlaceOrder? order, IMessageSession messageSession, HttpContext context) =>
    {
        if (order is null)
        {
            return Results.BadRequest();
        }
        
        var activity = context.Features.Get<IHttpActivityFeature>()?.Activity;
        activity?.SetTag("order.customerId", Guid.NewGuid().ToString());
        
        var command = new PlaceOrder
        {
            OrderId = Guid.NewGuid().ToString()
        };
        app.Logger.LogInformation("Sending PlaceOrder command via WebAPI, OrderId = {OrderId}", command.OrderId);
        await messageSession.Send(command);
        return Results.Ok(command);
    });

    app.Run();
}
catch (Exception ex)
{
    // https://githubmemory.com/repo/dotnet/runtime/issues/60600
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}