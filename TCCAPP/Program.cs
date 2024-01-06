using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MarketingCORE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CORE;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MarketingCOREContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MarketingCOREContext") ?? throw new InvalidOperationException("Connection string 'MarketingCOREContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.AccessDeniedPath = "/Home/Privacy";
        option.LoginPath = "/Acceso/Index";
        TimeSpan.FromMinutes(20);
        //option.ExpireTimeSpan = TimeSpan.Zero;
        //option.SlidingExpiration = true;

    });

builder.Services.AddSession();
builder.Services.AddDbContext<MarketingCOREContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MarketingCOREContext")));
//builder.Services.AddIdentity<Usuario, IdentityRole>().AddDefaultTokenProviders();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Index}/{id?}");

IWebHostEnvironment env = app.Environment;

app.Run();
