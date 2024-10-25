using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pharmacy_v2.Repos;
using Pharmacy_v2.Data;
using Pharmacy_v2.Models;
using Pharmacy_v2.Repo;
using Pharmacy_v2.Repos.Repo_Interfaces;
using System.Configuration;
using Pharmacy.HttpServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Conn")));



builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; 
        options.Cookie.Name = ".AspNetCore.Cookies"; 
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); 
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IHttpServiceAsyncAwait, HttpServiceAsyncAwait>();
builder.Services.AddScoped<IMedicineReposatory, MedicineRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryReposatory>();
builder.Services.AddScoped<IBagRepository, BagRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

app.Run();
