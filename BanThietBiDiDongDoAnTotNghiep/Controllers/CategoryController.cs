using BanThietBiDiDongDATN.ApiIntegration.Service.CategoryApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Categories.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BanThietBiDiDongDATN.Admin.Controllers
{
    [Authorize]
    public class CategoryController : BaseController
    {
        private readonly ICategoriesApiClient _brandApiClient;

        public CategoryController(ICategoriesApiClient categoriesApiClient)
        {
            _brandApiClient = categoriesApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int PageIndex = 1, int PageSize = 10)
        {
            var request = new GetCategoryRequest()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                keyword = keyword,
            };
            var data = await _brandApiClient.GetAllPaging(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuscessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _brandApiClient.CreateCategory(request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Thêm danh mục thành công";
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
                    TempData["failed"] = "Thêm danh mục không thành công";
                }
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _brandApiClient.GetById(id);
            if (result != null)
            {
                var updateRequest = new UpdateCategoryRequest()
                {
                    Id = result.ResultObj.Id,
                    CategoryName = result.ResultObj.CategoryName,
                    Description = result.ResultObj.Description,
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _brandApiClient.UpdateCategory(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Sửa danh mục thành công";
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
                    TempData["failed"] = "Sửa danh mục không thành công";
                }
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", " Cập nhật không thành công");
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new DeleteCategoryRequest()
            {
                id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCategoryRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _brandApiClient.Delete(request.id);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Xóa danh mục thành công";
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
                    TempData["failed"] = "Xóa danh mục không thành công";
                }
                return RedirectToAction("Index");
            }
        }
       
    }
}
