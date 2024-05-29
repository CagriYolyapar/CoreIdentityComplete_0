using CoreIdentityComplete_0.Areas.Administrator.Models.AppRoles.RequestModels;
using CoreIdentityComplete_0.Areas.Administrator.Models.AppUsers;
using CoreIdentityComplete_0.Areas.Administrator.Models.FluentValidation;
using CoreIdentityComplete_0.Models.ContextClasses;
using CoreIdentityComplete_0.Models.Entities;
using CoreIdentityComplete_0.Models.FluentValidation;
using CoreIdentityComplete_0.Models.ViewModels.AppUsers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequiredLength = 3;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.SignIn.RequireConfirmedEmail = false;
    opt.Password.RequireDigit = false;
}).AddEntityFrameworkStores<MyContext>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    opt.SlidingExpiration = true;
    opt.Cookie.Name = "CyberSelf";
    opt.Cookie.SameSite = SameSiteMode.Strict;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opt.LoginPath = new PathString("/Home/SignIn");
    opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
});

builder.Services.AddTransient<IValidator<UserRegisterRequestModel>, UserRegisterRequestModelValidator>();
builder.Services.AddTransient<IValidator<UserSignInRequestModel>, UserSignInRequestModelValidator>();
builder.Services.AddTransient<IValidator<CreateRoleRequestModel>, CreateRoleRequestModelValidator>();
builder.Services.AddTransient<IValidator<CreateUserRequestModel>, CreateUserRequestModelValidator>();

builder.Services.AddDbContextPool<MyContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")).UseLazyLoadingProxies());

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Administrator",
    pattern: "{area}/{controller}/{action}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Register}/{id?}");

app.Run();
