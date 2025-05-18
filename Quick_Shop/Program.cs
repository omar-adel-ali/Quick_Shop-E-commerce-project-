using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quick_Shop.Data;
using Quick_Shop.Data.Base;
using Quick_Shop.Data.Services;
using Quick_Shop.Models.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Register Generic Repository
builder.Services.AddScoped(typeof(IEntityBaseRepository<>), typeof(EntityBaseRepository<>));

// Register Services
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<ICartsService, CartsService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IWishlistsService, WishlistsService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<PaymentService>();

var app = builder.Build();

// Seed Roles and Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate(); // تطبيق المigrations لو فيه حاجة متأخرة
        }
        await AppDbInitializer.Seed(app);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
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

// Middleware لتحديد الأدوار
app.Use(async (context, next) =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        var userRole = context.User.IsInRole("Admin") ? "Admin" : "Customer";
        context.Items["UserRole"] = userRole;
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// إضافة مسار مخصص للـ Admin
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Dashboard}/{id?}",
    defaults: new { controller = "Admin", action = "Dashboard" });

app.Run();