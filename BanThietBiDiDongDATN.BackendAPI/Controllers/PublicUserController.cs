using BanThietBiDiDongDATN.Application.Catalog.System;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Users;
using BanThietBiDiDongDATN.Application.Catalog.Users.Dtos;
using BanThietBiDiDongDATN.Data.EF;
using BanThietBiDiDongDATN.Data.entities;
using BanThietBiDiDongDoAn.Applications.Brands;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;

namespace BanThietBiDiDongDATN.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicUserController : ControllerBase
    {
        private readonly BanThietBiDiDongDATNDbContext _context;
        private readonly IManagePublicUser _userService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
       
        public PublicUserController(BanThietBiDiDongDATNDbContext context, IManagePublicUser userService,

            IEmailSender emailService, UserManager<AppUser> userManager)
        {
            _context = context;
            _userService = userService;
            _emailSender = emailService;
            _userManager = userManager;
            
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExist = await _userManager.FindByNameAsync(request.Username);
            if (userExist != null)
            {
                return BadRequest(new { Message = "Tài khoản đã tồn tại" });
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return BadRequest(new { Message = "Email đã tồn tại" });
            }
            var user = new AppUser()
            {
                Dob = request.Dob,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Gender = Convert.ToBoolean(request.Gender),
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                // Tạo và lưu mã xác thực
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                // Gửi mã xác thực đến email của người dùng
                var confirmationLink = Url.Action("VerifyEmail", "PublicUser", new { userId = user.Id, code = token },Request.Scheme);
                await _emailSender.SendEmailAsync(request.Email, "Xác thực Email", $"Hãy xác nhận địa chỉ email bằng cách <a href='{confirmationLink}'>Bấm vào đây</a>.");

                return Ok(new { Message = "Registration successful. Please check your email for verification." });
            }
            else
            {
                return BadRequest(new { Message = "Đăng kí không thành công" });
            }
        }

        // Xác thực email bằng mã xác thực
        [HttpGet()]
        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var decode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decode);
            if (result.Succeeded)
            {
                // Đăng nhập người dùng sau khi xác thực thành công
                //await _signInManager.SignInAsync(user, isPersistent: false);
                //return Redirect("~/"); // Hoặc trang đích sau khi xác thực
                return Ok(new { Message = "Verify success" });
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return NotFound();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink = Url.Action("ResetPassword", "PublicUser", new { userId = user.Id, code = token }, Request.Scheme);
            await _emailSender.SendEmailAsync(email, "Quên mật khẩu ", $"Để đặt lại mật khẩu hãy <a href='{confirmationLink}'>Bấm vào đây</a>.");
            return Ok("Password reset email sent.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            // Kiểm tra và xử lý reset password
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, request.code, request.NewPassword);
                if (result.Succeeded)
                {
                    return Ok("Password has been reset successfully.");
                }
            }
            return BadRequest("Password reset failed.");
        }
        //[HttpPost("change-password")]
        //public async Task<IActionResult> ChangePassword([FromBody]  string UserId,string Newpassword )
        //{
        //    // Kiểm tra và xử lý reset password
        //    var user = await _userManager.FindByIdAsync(request.UserId);
        //    if (user != null)
        //    {
        //        var result = await _userManager.ResetPasswordAsync(user, request.code, request.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            return Ok("Password has been reset successfully.");
        //        }
        //    }
        //    return BadRequest("Password reset failed.");
        //}
    }
}
