using MassTransit;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpLogging;
using OrderGenerator;
using Shared.Contracts;
using Shared.Extensions;


var builder = WebApplication.CreateBuilder(args);

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