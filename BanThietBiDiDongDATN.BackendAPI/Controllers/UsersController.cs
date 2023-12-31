﻿
using BanThietBiDiDongDATN.Application.Catalog.System;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Runtime.InteropServices;
using System.Text;

namespace BTL_KTPM.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        public UsersController(IUserService userService, UserManager<AppUser> userManager, IEmailSender emailSender, IConfiguration configuration)
        {
            _userService = userService;
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var resultToken = await _userService.Authencate(request);
            if (string.IsNullOrEmpty(resultToken.ResultObj))
            {
                return BadRequest(resultToken);
            }
            return Ok(resultToken);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            // Gửi mã xác thực đến email của người dùng
            var confirmationLink = Url.Action("VerifyEmail", "Users", new { userId = user.Id, code = token }, Request.Scheme);
            await _emailSender.SendEmailAsync(request.Email, "Xác thực Email", $"Hãy xác nhận địa chỉ email bằng cách <a href='{confirmationLink}'>Bấm vào đây</a>.");
            return Ok(result);
        }
        //public string CreateVerificationLink(string userId, string token)
        //{
        //    return $"{new Uri(_configuration["BaseAddress"])}/VerifyEmail?userId={userId}&token={token}";
        //}
        [HttpGet()]
        [AllowAnonymous]
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
                return Redirect($"https://localhost:7282/Account/ComfirmEmail");
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest request)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var products = await _userService.GetUserPaging(request);
            return Ok(products);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.delete(id);
            return Ok(result);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Update(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }
        [HttpPut("{id}/roles")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RoleAssign(Guid id, [FromBody] RolesAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RoleAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("changePassword")]
        [Authorize]

        public async Task<IActionResult> ChangePassword(UpdatePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.ChangePassword(request.Id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("AdminChangePass")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AdminChangePassword(AdminUpdatePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.AdminChangePassword(request.Id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("checkLogin")]
        public async Task<IActionResult> CheckLastSignIn(Guid id)
        {
            var result = await _userService.CheckSignedTime(id);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}