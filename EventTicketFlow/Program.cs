using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using EventTicketFlow.Data;
using Microsoft.AspNetCore.Identity;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.");

builder.Services.AddDbContext<EventTicketFlow.Data.AppDbContext>(options =>
    options.UseSqlServer(DotNetEnv.Env.GetString("CONNECTION_STRING")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<EventTicketFlowContext>();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
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

//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>();
//    var roles = new[] { "Admin", "EventManager", "Customer" };

//    foreach (var role in roles)
//    {
//        if(!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//    }
//}

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

app.Run();
