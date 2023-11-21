using BanThietBiDiDongDATN.ApiIntegration.Service.UserApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using BanThietBiDiDongDATN.ApiIntegration.Service.RolesApiClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices;

namespace BanThietBiDiDongDATN.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;

        public UserController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int PageIndex = 1, int PageSize = 10)
        {
            var request = new GetUserPagingRequest()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                Keyword = keyword,
            };
           
            var roles = await _roleApiClient.GetAll();
            ViewBag.Roles = roles.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
            var data = await _userApiClient.GetUserPaging(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuscessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string UserId)
        {
            var result =await _userApiClient.GetById(Guid.Parse(UserId));
            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleApiClient.GetAll();
            ViewBag.Roles = roles.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Thêm người dùng thành công";
                return RedirectToAction("Index");
            }
            else
            {
                if (result.Message != null)
                {
                    TempData["failed"] = result.Message;
                }
                else
                {
                    TempData["failed"] = "Thêm người dùng không thành công";
                }
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var result = await _userApiClient.GetById(Guid.Parse(Id));
            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = user.Id,
                    Address = user.Address,
                    Img= user.Avatar,
                    Gender= user.Gender,
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            return View(result.ResultObj);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _userApiClient.UpdateUser(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Sửa thông tin thành công";
                return RedirectToAction("Index");
            }
            else
            {
                if (result.Message != null)
                {
                    TempData["failed"] = result.Message;
                }
                else
                {
                    TempData["failed"] = "Thay đổi thông tin không thành công";
                }
                return RedirectToAction("Index");
            }
        }
      
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.Delete(Guid.Parse(id));
            if (result.IsSuccessed)
            {
                TempData["success"] = "Xóa thành công";
                return RedirectToAction("Index");
            }
            else
            {
                if (result.Message != null)
                {
                    TempData["failed"] = result.Message;
                }
                else
                {
                    TempData["failed"] = "Xóa người không thành công";
                }
                return RedirectToAction("Index");
            }

            //ModelState.AddModelError("", result.Message);
            //return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(string roleId,string userId)
        {
            if(roleId=="0")
            {
                TempData["failed"] = "Gán quyền không thành công";
                return RedirectToAction("Index");
            }
            var role =await _roleApiClient.GetRoleById(roleId);
            var roleVm = new RolesAssignRequest() { 
            Id=Guid.Parse(userId),
            roles= new AppRoles(){Id=role.ResultObj.Id,Name= role.ResultObj.Name, Description=role.ResultObj.Description}
            };
            var result = await _userApiClient.RoleAssign(Guid.Parse(userId), roleVm);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Gán quyền thành công";
                return RedirectToAction("Index");
            }
            else
            {
                if (result.Message != null)
                {
                    TempData["failed"] = result.Message;
                }
                else
                {
                    TempData["failed"] = "Gán quyền không thành công";
                }
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> AdminChangePassword(string UserId)
        {
            var request = new AdminUpdatePasswordRequest()
            { 
                Id = Guid.Parse(UserId)
            };
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> AdminChangePassword(AdminUpdatePasswordRequest request)
        {
            var request2 = new AdminUpdatePasswordRequest()
            {
                Id = request.Id,
            };
            if (!ModelState.IsValid)
            {
                return View(request2);
            }
            var result = await _userApiClient.AdminChangePassword(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Đổi mật khẩu thành công";
                return View(request2);
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
                return View(request2);
            }
        }
    }
}
