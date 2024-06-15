using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping_Cart_2.Data;
using Shopping_Cart_2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IUserOrderService, UserOrderService>();
builder.Services.AddTransient<IStockService, StockService>();
builder.Services.AddTransient<IManageItemService, ManageItemService>();
builder.Services.AddTransient<IRatingService, RatingService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();
// Uncomment it when you run the project first time, It will registered an admin
//using (var scope = app.Services.CreateScope())
//{
//    await DbSeeder.SeedDefaultData(scope.ServiceProvider);
//}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
