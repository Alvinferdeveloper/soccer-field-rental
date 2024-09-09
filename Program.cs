using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using proyectoCanchas.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("AppDbContext"), new MySqlServerVersion(new Version(8,0))).EnableDetailedErrors());

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth";  // Ruta al formulario de inicio de sesión
        options.LogoutPath = "/Account/Logout"; // Ruta al cierre de sesión
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Tiempo de expiración de la cookie
        options.SlidingExpiration = true; // Renovar la cookie si el usuario está activo
    });

    builder.Services.AddAuthorization(options =>
{
   
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
