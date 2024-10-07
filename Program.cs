using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecto_TiendaElectronica.ModelBinder;
using Proyecto_TiendaElectronica.Models;
using Proyecto_TiendaElectronica.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddControllersWithViews(
    options => {
        options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());
    }
);

var connectionString = builder.Configuration.GetConnectionString("Server=localhost;Database=TiendaElectronica;Trusted_Connection=True;TrustServerCertificate=True;");

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<Usuario, IdentityRole>(options => {
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); // Tiempo de bloqueo
    options.Lockout.MaxFailedAccessAttempts = 2; // Máximo de intentos fallidos antes del bloqueo
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
