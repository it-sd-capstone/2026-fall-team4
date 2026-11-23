using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using EventTicketFlow.Data;
using EventTicketFlow.Models;
using Microsoft.AspNetCore.Identity;
using EventTicketFlow.Services;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EventTicketFlow.Data.AppDbContext>(options =>
    options.UseSqlServer(DotNetEnv.Env.GetString("CONNECTION_STRING")));

builder.Services.AddTransient<TestDataService>();
builder.Services.AddTransient<EventService>();


// Add services to the container.
builder.Services.AddAuthorization(options =>
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin")));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizeAreaFolder("Admin", "/", "Admin"));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.MapGet("/db-test", async (AppDbContext db) =>
{
    
    try
    {
        await db.Database.ExecuteSqlRawAsync("SELECT 1");
        return Results.Ok("Database connection successful."); 
    }
    catch
    {
        return Results.Problem("Database connection failed.");
    }

});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // System roles used by the application
    var roles = new[] { "Admin", "EventManager", "Customer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create development test users when running locally
    // Users will be assigned to the appropriate role
    if (app.Environment.IsDevelopment())
    {
        var adminEmail = Env.GetString("ADMIN_EMAIL");
        var adminPassword = Env.GetString("ADMIN_PASSWORD");

        if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
        {
            await CreateTestUser(userManager, adminEmail, adminPassword, "Admin");
        }

        var managerEmail = Env.GetString("EVENTMANAGER_EMAIL");
        var managerPassword = Env.GetString("EVENTMANAGER_PASSWORD");

        if (!string.IsNullOrWhiteSpace(managerEmail) && !string.IsNullOrWhiteSpace(managerPassword))
        {
            await CreateTestUser(userManager, managerEmail, managerPassword, "EventManager");
        }

        var customerEmail = Env.GetString("CUSTOMER_EMAIL");
        var customerPassword = Env.GetString("CUSTOMER_PASSWORD");

        if (!string.IsNullOrWhiteSpace(customerEmail) && !string.IsNullOrWhiteSpace(customerPassword))
        {
            await CreateTestUser(userManager, customerEmail, customerPassword, "Customer");
        }
    }

    if (DotNetEnv.Env.GetString("SEED_TEST_DATA") == "true")
    {
        var testDataService = scope.ServiceProvider.GetRequiredService<TestDataService>();
        testDataService.PopulateTestData();
    }
}

app.Run();

// Creates test user if one does not already exist. If the user exists but is not assigned to the specified role,
// the role will be added.
static async Task CreateTestUser(UserManager<ApplicationUser> userManager, string email, string password, string role)
{
    // Check if the user exists
    var user = await userManager.FindByEmailAsync(email);

    if (user == null)
    {
        // Create a new Identity user
        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        // Create the user with the supplied password
        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            // Assign the user to the specified role
            await userManager.AddToRoleAsync(user, role);
        }
        else
        {
            // Log any creation errors
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Failed to create user {email}: {error.Description}");
            }
        }
    }
    else
    {
        // Ensure an existing user has the required role
        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
