using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var blobs = builder.AddAzureStorage("Storage")
    .RunAsEmulator()
    .WithHttpEndpoint(10000, 10000, isProxied: false)
    .AddBlobs("BlobConnection");

var apiService = builder.AddProject<SvitlaSmartTalk2024_ApiService>("apiservice")
    .WithReference(blobs);

builder.AddProject<SvitlaSmartTalk2024_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
