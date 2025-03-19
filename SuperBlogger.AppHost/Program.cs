var builder = DistributedApplication.CreateBuilder(args);

// What about SQL Server?
var blogdb = builder.AddSqlServer("dbServer")
    .AddDatabase("blogdb"); 

var blazorServer = builder.AddProject<Projects.BloggerBlazorServer>("blazorServer")
    .WithReference(blogdb)
    .WaitFor(blogdb);

var apiService = builder.AddProject<Projects.SuperBlogger_ApiService>("apiservice")
    .WithReference(blogdb)
    .WaitFor(blazorServer);

builder.AddProject<Projects.SuperBlogger_Web>("webfrontend")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
