var builder = DistributedApplication.CreateBuilder(args);

var abc = builder.AddPostgres("whatever");

var db = abc.AddDatabase("db");

builder.AddProject<Projects.API>("api").WithReference(db);

builder.Build().Run();
