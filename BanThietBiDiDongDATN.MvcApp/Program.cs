using BanThietBiDiDongDATN.ApiIntegration.Service.BrandApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.CartApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.CategoryApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.OrderApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.ProductApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.UserApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.VoucherApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.MvcApp.Models;
using BTL_KTPM.ApiIntegration.Service.CartApiClient;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
var mvcBuilder = builder.Services.AddRazorPages();
builder.Services.AddTransient<ICategoriesApiClient, CategoriesApiClient>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IBrandApiClient, BrandApiClient>();
builder.Services.AddTransient<IVoucherApiClient, VoucherApiClient>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<ICartApiClient, CartApiClient>();
builder.Services.AddScoped<IVNPayMethod, VNPayMethod>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Home/Index/";
    });
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(3);
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(3); 
});
builder.Services.Configure<SecurityStampValidatorOptions>(o => o.ValidationInterval = TimeSpan.FromHours(10));
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
