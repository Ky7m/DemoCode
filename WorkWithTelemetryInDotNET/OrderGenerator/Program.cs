using MassTransit;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpLogging;
using OrderGenerator;
using Serilog;
using Serilog.Events;
using Shared.Contracts;
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
        .UseSerilogDefaults();

    builder.Services.AddOpenTelemetryDefaults(builder.Configuration, builder.Environment);
    builder.Services.AddMassTransitSharedConfiguration();
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
    
    app.MapPost("/api/orders", async (PlaceOrder? order, IBus bus, HttpContext context) =>
    {
        if (order is null)
        {
            return Results.BadRequest();
        }
        
        var activity = context.Features.Get<IHttpActivityFeature>()?.Activity;
        activity?.SetTag("order.customerId", Guid.NewGuid().ToString());
        
        var command = new PlaceOrder
        {
            OrderId = Guid.NewGuid()
        };
        app.Logger.LogInformation("/api/orders sent PlaceOrder command, OrderId = {OrderId}", command.OrderId);
        await bus.Publish(command);
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