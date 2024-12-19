var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.KNAB_Assessment_ApiService>("apiservice");

builder.AddProject<Projects.KNAB_Assessment_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
