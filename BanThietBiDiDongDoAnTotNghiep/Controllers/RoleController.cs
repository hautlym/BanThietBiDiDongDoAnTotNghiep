using BanThietBiDiDongDATN.ApiIntegration.Service.RolesApiClient;

using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BanThietBiDiDongDATN.Admin.Models;
using BanThietBiDiDongDATN.ApiIntegration.Service.UserApiClient;
using Microsoft.AspNetCore.Identity;

namespace BanThietBiDiDongDATN.Admin.Controllers
{
    public class RoleController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;
        private readonly IUserApiClient _userApiClient;

        public RoleController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _configuration = configuration;
            _roleApiClient = roleApiClient;
            _userApiClient = userApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int PageIndex = 1, int PageSize = 10)
        {
            var request = new GetRolePagingRequest()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                Keyword = keyword
            };
            var data = await _roleApiClient.GetRolePaging(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuscessMsg = TempData["result"];
            }
            var roleMd = new PageResult<RoleModel>()
            {
                TotalRecords = data.ResultObj.TotalRecords,
                Items = data.ResultObj.Items.Select(x => new RoleModel()
                {
                    RoleId = x.Id.ToString(),
                    RoleName = x.Name,
                    Description = x.Description,
                    TotalUser = _roleApiClient.GetTotalUserWithRole(x.Id.ToString()).Result.ResultObj
                }).ToList(),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            var result = new ApiSuccessResult<PageResult<RoleModel>>(roleMd);
            return View(result.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _roleApiClient.CreateRole(request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Thêm quyền thành công";
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
                    TempData["failed"] = "Thêm quyền không thành công";
                }
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            return View();
        }
        //[HttpGet]
        //public async Task<IActionResult> Details(Guid id)
        //{
        //    var result = await _roleApiClient.GetRoleById(id.ToString());
        //    return View(result.ResultObj);
        //}
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRoleRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _roleApiClient.UpdateRole(request.id.ToString(), request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Sửa quyền thành công";
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
                    TempData["failed"] = "Thêm quyền không thành công";
                }
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _roleApiClient.DeleteRole(id.ToString());
            if (result.IsSuccessed)
            {
                TempData["success"] = "Xóa quyền thành công";
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
                    TempData["failed"] = "Thêm quyền không thành công";
                }
                return RedirectToAction("Index");
            }

            //ModelState.AddModelError("", result.Message);
            //return View(request);
        }
    }
}
