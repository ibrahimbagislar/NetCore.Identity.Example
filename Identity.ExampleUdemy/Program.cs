using Identity.ExampleUdemy.CustomDescriber;
using Identity.ExampleUdemy.Data.Contexts;
using Identity.ExampleUdemy.Data.Entites;
using Identity.ExampleUdemy.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequiredLength = 5;
    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedEmail = false;

    opt.Lockout.MaxFailedAccessAttempts = 15;

}).AddErrorDescriber<CustomErrorDescriber>().AddEntityFrameworkStores<IdentityContext>();



builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SameSite = SameSiteMode.Strict;
    // cookie ad�
    opt.Cookie.Name = "ibrahimbagislar_identity";
    // kullan�c� giri� yapmam�� ise ve giri� yapmas� gereken bir sayfaya girdi�inde y�nlendirilecek sayfa
    opt.LoginPath = new PathString("/home/signin");
    // giri� yap�lm�� ancak ilgili role �zerinde bulunmuyorsa (�rnek adminin girdi�i yere member rol� olan biri giriyor) y�nlendirilecek sayfa
    opt.AccessDeniedPath = new PathString("/home/accessdenied");
    // cookie ge�erlilik s�resi
    opt.ExpireTimeSpan = TimeSpan.FromDays(365);
});
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddDbContext<IdentityContext>(opt =>
{
     opt.UseSqlServer("server=DESKTOP-N6VN4VE; database=UdemyIdentityApp; integrated security=true; TrustServerCertificate=True;");
});

var app = builder.Build();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = "/node_modules",
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "node_modules"))
});
app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = "/wwwroot", // �stemci taraf�ndan kullan�lacak yol
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
});
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});


app.Run();
