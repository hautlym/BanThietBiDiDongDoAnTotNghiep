using BanThietBiDiDongDATN.ApiIntegration.Service.UserApiClient;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace BanThietBiDiDongDATN.MvcApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public AccountController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Vui lòng điền tài khoản hoặc mật khẩu");
                return View();
            }
            var token = await _userApiClient.Authenticate(request);
            if (token.ResultObj == null)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mẩt khẩu");
                return View();
            }
            
            var userPrincipal = this.ValidateToken(token.ResultObj);
            var kq = userPrincipal.FindFirst(ClaimTypes.Role).Value;
            if (!kq.Contains("Customer"))
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại");
                return View();
            }
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Today.AddDays(3),
                IsPersistent = true
            };
            HttpContext.Session.SetString("Token", token.ResultObj);
            await HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       userPrincipal,
                       authProperties);
            var checkTime = await _userApiClient.CheckSignedTime(Guid.Parse(userPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value));

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RegisterUser(request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra thông tin");
                return View();
            }
            return RedirectToAction("VerifyEmail", "Account");
        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
                identity.AddClaim(new Claim("access_token", jwtToken));

            return principal;
        }
        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ComfirmEmail()
        {
            return View();
        }
    }
}
