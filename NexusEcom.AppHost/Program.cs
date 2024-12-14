var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.NexusEcom>("nexusecom");

builder.Build().Run();
