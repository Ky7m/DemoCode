using Api.Data;
using Api.Extensions;
using Api.Services;
using CSnakes.Runtime;
using CSnakes.Runtime.Locators;
using Microsoft.AspNetCore.Mvc;

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


var home = Path.Join(Environment.CurrentDirectory, "python");
var pb = builder.Services.WithPython();
if (builder.Environment.IsEnvironment("localhost"))
{
    pb.FromRedistributable(RedistributablePythonVersion.Python3_12);
}
else
{
    pb.FromEnvironmentVariable("Python3_ROOT_DIR", Environment.GetEnvironmentVariable("PYTHON_VERSION") ?? "3.12");
}
pb.WithHome(home).WithPipInstaller().WithVirtualEnvironment(Path.Join(home, ".venv"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IPythonEnvironment>().Helpers());

var app = builder.Build();

app.UseAntiforgery();

app.UseFileServer();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapDefaultEndpoints();

app.MapImages();

app.MapGet("/python", ([FromServices] IHelpers helpers) => Results.Ok(helpers.ExtractAbbr("abc cdf")));

app.Run();
