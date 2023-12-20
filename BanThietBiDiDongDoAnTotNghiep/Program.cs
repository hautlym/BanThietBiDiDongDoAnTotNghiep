using BanThietBiDiDongDATN.ApiIntegration.Service.BrandApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.CategoryApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.OrderApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.ProductApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.RolesApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.UserApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.VoucherApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Data.EF;
using BanThietBiDiDongDATN.Data.entities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var mvcBuilder = builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index/";
        options.AccessDeniedPath = "/Home/Index/";
        options.ExpireTimeSpan = TimeSpan.FromDays(3); // Thời gian sống của cookie là 7 ngày
        options.SlidingExpiration = true; // Tự động gia hạn thời gian sống khi có hoạt động
    });
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(3); // Thời gian sống của phiên làm việc là 7 ngày
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});
builder.Services.Configure<SecurityStampValidatorOptions>(o => o.ValidationInterval = TimeSpan.FromHours(10));
builder.Services.AddTransient<ICategoriesApiClient, CategoriesApiClient>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IRoleApiClient, RoleApiClient>();
builder.Services.AddTransient<IBrandApiClient, BrandApiClient>();
builder.Services.AddTransient<IVoucherApiClient, VoucherApiClient>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<StatusCodeRedirectMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.Use((context, next) =>
{
    if (!context.User.Identity?.IsAuthenticated ?? false)
        return next();

    var accessTokenClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "access_token");
    if (accessTokenClaim == null)
        return next();

    context.Session.SetString("Token", accessTokenClaim.Value);
    return next();
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
