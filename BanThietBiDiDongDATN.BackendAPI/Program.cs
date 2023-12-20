using BanThietBiDiDongDATN.Application.Catalog.Brands;
using BanThietBiDiDongDATN.Application.Catalog.Categories;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.Commom.Mails;
using BanThietBiDiDongDATN.Application.Catalog.System;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Users;
using BanThietBiDiDongDATN.Data.EF;
using BanThietBiDiDongDATN.Data.entities;
using BanThietBiDiDongDATNApplication.Validate;
using BanThietBiDiDongDoAn.Applications.Brands;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using BanThietBiDiDongDATN.Application.Catalog.Vouchers;
using BanThietBiDiDongDATN.Application.Catalog.Products;
using Serilog;
using BanThietBiDiDongDATN.Application.Catalog.Orders;
using BanThietBiDiDongDATN.Application.Catalog.Carts;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Host.UseSerilog((hostingcontext, logggerConfiguration) => { logggerConfiguration.ReadFrom.Configuration(hostingcontext.Configuration); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger eShop Solution", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                    });
});

// mail
builder.Services.AddOptions();                                        // Kích hoạt Options
var mailsettings = builder.Configuration.GetSection("MailSettings");  // đọc config
builder.Services.Configure<MailSettings>(mailsettings);               // đăng ký để Inject
builder.Services.AddTransient<IEmailSender, SendMailService>();

builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequiredLength = 8;
    option.Password.RequireUppercase = true;
    option.Password.RequireLowercase = true;
    option.Password.RequireDigit = true;
    option.Password.RequireNonAlphanumeric = true;
    option.User.RequireUniqueEmail = true;
    option.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    option.SignIn.RequireConfirmedPhoneNumber = false;
});
builder.Services.Configure<SecurityStampValidatorOptions>(o => o.ValidationInterval = TimeSpan.FromHours(10));


//builder.Services.AddControllersWithViews()
//                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<BanThietBiDiDongDATNDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DATN_Database")));
builder.Services.AddIdentity<AppUser, AppRoles>().AddEntityFrameworkStores<BanThietBiDiDongDATNDbContext>().AddDefaultTokenProviders();
builder.Services.AddTransient<IStorageService, FileStorageService>();
builder.Services.AddTransient<IManageCategory, ManageCategory>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IManageBrand, ManageBrand>();
builder.Services.AddTransient<IManageVoucher, ManageVoucher>();
builder.Services.AddTransient<IManagePublicUser, ManagePublicUser>();
builder.Services.AddTransient<IRolesService, RolesService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IManageProduct, ManageProduct>();
builder.Services.AddTransient<IManageCart, ManagerCart>();
builder.Services.AddTransient<IManageOrder, ManageOrder>();

builder.Services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
string issuer = "https://hello.api.com";
string signingKey = "123456789987654321";
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
           .AddJwtBearer(options =>
           {
               options.RequireHttpsMetadata = false;
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   ValidateIssuer = true,
                   ValidIssuer = issuer,
                   ValidateAudience = true,
                   ValidAudience = issuer,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ClockSkew = TimeSpan.Zero,
                   RequireExpirationTime = true,
                   IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
                   RequireSignedTokens = true,
               };
           });


var app = builder.Build();
//app.UseSerilogRequestLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapControllers();

app.Run();