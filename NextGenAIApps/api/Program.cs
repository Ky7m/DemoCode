using Api.Data;
using Api.Extensions;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSqlServerDbContext<ImageDbContext>("imagedb");

builder.AddAzureBlobContainerClient("images");
builder.AddAzureQueueServiceClient("queues");

builder.Services.AddOpenApi();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
});

// Register database initializer as hosted service
builder.Services.AddHostedService<DatabaseInitializer>();

var app = builder.Build();

app.UseAntiforgery();

app.UseFileServer();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapDefaultEndpoints();

app.MapImages();

app.Run();
