using Mcp.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSqlServerClient(connectionName: "imagedb");

builder.AddAzureChatCompletionsClient("chat")
    .AddChatClient();

builder.Services.AddMemoryCache();

builder.Services.AddMcpServer()
    .WithHttpTransport(options => options.Stateless = true)
    .WithTools<DataQueryTools>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapMcp();

app.Run();
