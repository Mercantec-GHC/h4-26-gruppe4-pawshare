var builder = DistributedApplication.CreateBuilder(args);

var abc = builder.AddPostgres("whatever")
    .WithDataVolume()
    .WithPgWeb();

var db = abc.AddDatabase("db");

builder.AddProject<Projects.API>("api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
