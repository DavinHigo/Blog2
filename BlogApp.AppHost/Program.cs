var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql").AddDatabase("sqldata"); 


var apiService = builder.AddProject<Projects.BlogApp_ApiService>("apiservice")
    .WithReference(sql)
    .WaitFor(sql);

builder.AddProject<Projects.BlogApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

// Add the Razor server project
var razorServer = builder.AddProject<Projects.BlogServer>("razorserver");


var api = builder.AddProject<Projects.BlogApp_ApiService>("backend")
    .WithReference(sql)
    .WaitFor(sql); 

builder.Build().Run();
