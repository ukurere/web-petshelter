using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using web_petshelter.Data;
using web_petshelter.Models;
using web_petshelter.Features.Statistics;
using web_petshelter.Infrastructure; // для IdentitySeed

var builder = WebApplication.CreateBuilder(args);

// ---------------- Конфіг ----------------
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// ---------------- DbContext ----------------
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var env = builder.Environment;

    if (env.EnvironmentName == "Docker")
        options.UseSqlite(builder.Configuration.GetConnectionString("DockerConnection"));
    else
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// ---------------- Identity + ролі + Default UI ----------------
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Identity/Account/Login";
    opt.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// ---------------- Сервіси застосунку ----------------
builder.Services.AddScoped<StatisticsService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

builder.Services.AddSignalR();

// ---------------- Build app ----------------
var app = builder.Build();

app.MapHub<web_petshelter.RealTime.TasksHub>("/hubs/tasks");

// ---------------- Pipeline ----------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
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
    pattern: "{controller=Home}/{action=Landing}/{id?}");

app.MapRazorPages();

// ---------------- Міграції + seed БД + seed адміна ----------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // міграції
    var db = services.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    // тестові дані притулків у дев-оточенні
    if (app.Environment.IsDevelopment())
        await DbSeed.SeedSheltersAsync(db);

    // створення ролі Admin і користувача yevgieniia.riabichenko@gmail.com з цією роллю
    await IdentitySeed.SeedAdminAsync(services);
}

// інший твій seed (якщо був)
await DbInitializer.SeedAsync(app.Services, app.Environment);

// ---------------- Run ----------------
app.Run();
