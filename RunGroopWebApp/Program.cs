using ClubGroopWebApp.Data;
using ClubGroopWebApp.Extensions;
using ClubGroopWebApp.Helpers;
using ClubGroopWebApp.Interfaces;
using ClubGroopWebApp.Models;
using ClubGroopWebApp.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClubGroopWebApp.Repository;
using ClubGroopWebApp.Servises;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();


builder.Services.AddScoped<IPhotoService, PhotoService>();//pfoto servise
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings")); //Api pfoto servise

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>(); //include identity
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();


var app = builder.Build();

//run Terminal --> dotnet run seeddata
if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
  // await Seed.SeedUsersAndRolesAsync(app);
    Seed.SeedData(app);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.InitializeDatabase<ApplicationDbContext>().ConfigureAwait(false);//run migration(if exist) and create Db

app.Run();
