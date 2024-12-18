var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.NexusEcom>("nexusecom");

try
{
    builder.Build().Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Detailed Error: {ex}");
    Console.WriteLine($"Inner Exception: {ex.InnerException}");
}
