using Microsoft.EntityFrameworkCore;
using QuizFilosofico.Data;
using QuizFilosofico.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var sqliteConnection = builder.Configuration.GetConnectionString("SQLite");
var sqlServerConnection = builder.Configuration.GetConnectionString("LAGOSTIM");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (!string.IsNullOrWhiteSpace(sqliteConnection))
    {
        options.UseSqlite(sqliteConnection);
    }
    else if (!string.IsNullOrWhiteSpace(sqlServerConnection))
    {
        options.UseSqlServer(sqlServerConnection);
    }
    else
    {
        throw new InvalidOperationException("Configure uma connection string em ConnectionStrings:SQLite ou ConnectionStrings:LAGOSTIM.");
    }
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
});

builder.Services.Configure<QuizFilosofico.Models.OpenAI.OpenAiOptions>(builder.Configuration.GetSection("OpenAI"));
builder.Services.AddHttpClient<OpenAiQuizService>();
builder.Services.AddScoped<OpenAiQuizService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

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
