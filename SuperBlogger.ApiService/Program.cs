using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperBlogger.ApiService.Data;
using SuperBlogger.ApiService.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddProblemDetails();

// Retrieve the connection string from the environment variable or configuration file
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__blogdb");
if (connectionString == null)
{
    Console.WriteLine("Connection string 'blogdb' not found. Using default connection string.");
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (connectionString == null)
    {
        throw new InvalidOperationException("Connection string not found.");
    }
}

Console.WriteLine($"Connection string: {connectionString}");

// Add DbContext 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ArticleService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add MVC services
builder.Services.AddControllers();

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
