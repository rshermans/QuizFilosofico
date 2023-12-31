using Microsoft.EntityFrameworkCore;
using QuizFilosofico.Data;
using QuizFilosofico.Models;
using NuGet.Protocol.Core.Types;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//esta linha foi adicionada: (e tamb�m o namespace Equipas.Data) <---------------------------------
builder.Services.AddDbContext<ApplicationDbContext>
    (options => options.UseSqlServer(
                        builder.Configuration.GetConnectionString("LAGOSTIM")));


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddScoped<QuizFilosofico>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{ options.IdleTimeout = TimeSpan.FromMinutes(15); });

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
