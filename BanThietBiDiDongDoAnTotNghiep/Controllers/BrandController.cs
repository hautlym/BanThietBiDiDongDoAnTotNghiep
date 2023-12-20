using BanThietBiDiDongDATN.ApiIntegration.Service.CategoryApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Categories.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Categories;
using Microsoft.AspNetCore.Mvc;
using BanThietBiDiDongDATN.ApiIntegration.Service.BrandApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Brands.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace BanThietBiDiDongDATN.Admin.Controllers
{
    [Authorize]
    public class BrandController : BaseController
    {
        private readonly IBrandApiClient _brandApiClient;

        public BrandController(IBrandApiClient brandApiClient)
        {
            _brandApiClient = brandApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int PageIndex = 1, int PageSize = 10)
        {
            var request = new getBrandRequest()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                keyword = keyword,
            };
            var data = await _brandApiClient.GetAlllPaging(request);
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(CreateBrandRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _brandApiClient.Create(request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Thêm thương hiệu thành công";
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
                    TempData["failed"] = "Thêm thương hiệu không thành công";
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
                var updateRequest = new UpdateBrandRequest()
                {
                    id= result.ResultObj.id,
                    BrandName = result.ResultObj.BrandName,
                    ImgLogo=result.ResultObj.logo,
                    description=result.ResultObj.description,
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(UpdateBrandRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _brandApiClient.Update(request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Sửa thương hiệu thành công";
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
                    TempData["failed"] = "Sửa thương hiệu không thành công";
                }
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", " Cập nhật không thành công");
            return View(request);
        }

        //[HttpGet]
        //public IActionResult Delete(int id)
        //{
        //    return View(new DeleteCategoryRequest()
        //    {
        //        id = id
        //    });
        //}

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _brandApiClient.Delete(id);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Xóa thương hiệu thành công";
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
                    TempData["failed"] = "Xóa thương hiệu không thành công";
                }
                return RedirectToAction("Index");
            }
        }
    }
}
