using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<E_Commerce.Models.ECommerceContext>();
builder.Services.AddDbContext<E_Commerce.Areas.Admin.Models.UserContext>(x => x.UseSqlServer("Data Source=seyda-pc;Initial Catalog=ECommerce; Integrated Security=True"));
builder.Services.AddDbContext<E_Commerce.Models.ECommerceContext>(x => x.UseSqlServer("Data Source=seyda-pc;Initial Catalog=ECommerce; Integrated Security=True"));

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

CultureInfo cultureInfo = new CultureInfo("tr-TR");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var app = builder.Build();

//services.AddIdentity<ApplicationUser, IdentityRole>(Configuration,
//    options =>
//        options.Password = new PasswordOptions
//        {
//            RequireDigit = true,
//            RequiredLength = 6,
//            RequireLowercase = true,
//            RequireUppercase = true,
//            RequireNonLetterOrDigit = false
//        })
//[...];
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"); //localhost/sellers þeklinde gidilebilmesi için home controller açýlýr

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
