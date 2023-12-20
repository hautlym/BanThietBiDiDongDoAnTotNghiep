using BanThietBiDiDongDATN.Admin.Models;
using BanThietBiDiDongDATN.ApiIntegration.Service.RolesApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.UserApiClient;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanThietBiDiDongDATN.Admin.Controllers
{
    [Authorize]
    public class ManageAccount : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;

        public ManageAccount(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }

        public async Task<IActionResult> Index(string userId)
        {
            var users = await _userApiClient.GetById(Guid.Parse(userId));
            var model = new UserModel()
            {
                user = users.ResultObj
            };
            return View(users.ResultObj);
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UpdatePasswordRequest request)
        {
            if(!ModelState.IsValid) {
                return View();
            }
            var result =await _userApiClient.ChangePassword(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Đổi mật khẩu thành công";
                return View();
            }
            else
            {
                if (result.Message != null)
                {
                    TempData["failed"] = result.Message;
                }
                else
                {
                    TempData["failed"] = "Đổi mật khẩu không thành công";
                }
                return View();
            }
        }
    }
}
