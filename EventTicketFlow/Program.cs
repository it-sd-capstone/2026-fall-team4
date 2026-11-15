using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using EventTicketFlow.Data;
using EventTicketFlow.Models;
using Microsoft.AspNetCore.Identity;
using EventTicketFlow.Services;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EventTicketFlow.Data.AppDbContext>(options =>
    options.UseSqlServer(DotNetEnv.Env.GetString("CONNECTION_STRING")));

builder.Services.AddTransient<TestDataService>();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

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

    if (DotNetEnv.Env.GetString("SEED_TEST_DATA") == "true")
    {
        var testDataService = scope.ServiceProvider.GetRequiredService<TestDataService>();
        testDataService.PopulateTestData();
    }
}

app.Run();
