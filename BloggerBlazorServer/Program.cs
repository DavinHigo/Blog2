using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BloggerBlazorServer.Components;
using BloggerBlazorServer.Components.Account;
using BloggerBlazorServer.Data;
using BloggerBlazorServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<UserService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__blogdb");
if (connectionString == null) {
    // no Aspire
    Console.WriteLine("Connection string 'blogdb' not found. Using default connection string.");
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (connectionString == null) {
        throw new InvalidOperationException("Connection string not found.");
    }
}

Console.WriteLine($"Connection string: {connectionString}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var baseUrl = builder.Configuration["BaseUrl"] ?? "https://localhost:5001";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

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
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();
 
app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    // Seed the user
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedUserAsync(userManager, roleManager);
}

app.Run();

async Task SeedUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    var userId = "specific-user-id";
    var user = await userManager.FindByIdAsync(userId);
    if (user == null)
    {
        user = new ApplicationUser
        {
            Id = userId,
            UserName = "a@a.a",
            Email = "a@a.a",
            FirstName = "Admin",
            LastName = "User",
            IsAuth = true,
            Role = "Admin"
        };
        await userManager.CreateAsync(user, "P@$$w0rd");

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        await userManager.AddToRoleAsync(user, "Admin");
    }

    var contributorUserId = "specific-contributor-user-id";
    var contributorUser = await userManager.FindByIdAsync(contributorUserId);
    if (contributorUser == null)
    {
        contributorUser = new ApplicationUser
        {
            Id = contributorUserId,
            UserName = "c@c.c",
            Email = "c@c.c",
            FirstName = "Contributor",
            LastName = "User",
            IsAuth = true,
            Role = "Contributor"
        };
        await userManager.CreateAsync(contributorUser, "P@$$w0rd");

        if (!await roleManager.RoleExistsAsync("Contributor"))
        {
            await roleManager.CreateAsync(new IdentityRole("Contributor"));
        }

        await userManager.AddToRoleAsync(contributorUser, "Contributor");
    }
}