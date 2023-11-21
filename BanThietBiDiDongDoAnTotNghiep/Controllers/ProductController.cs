using BanThietBiDiDongDATN.ApiIntegration.Service.BrandApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.CategoryApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.ProductApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Products.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BanThietBiDiDongDATN.Admin.Controllers
{

    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoriesApiClient _categoriesApiClient;
        private readonly IBrandApiClient _brandApiClient;

        public ProductController(IProductApiClient productApiClient, IConfiguration configuration,
            ICategoriesApiClient categoriesApiClient, IBrandApiClient brandApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoriesApiClient = categoriesApiClient;
            _brandApiClient = brandApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int? categoryId, int PageIndex = 1, int PageSize = 5)
        {
            var request = new GetProductPagingRequest()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                keyword = keyword,
                CategoryId = categoryId
            };
            var categories = await _categoriesApiClient.GetAll();
            ViewBag.Categories = categories.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            var data = await _productApiClient.GetAllPaging(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuscessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoriesApiClient.GetAll();
            ViewBag.Category = categories.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString(),
            });
            var brand = await _brandApiClient.GetAllBrand();
            ViewBag.Brand = brand.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.BrandName,
                Value = x.id.ToString(),
            });
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.Create(request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _productApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var product = result.ResultObj;
                var updateRequest = new ProductUpdateRequest()
                {
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    CategoryId = product.CategoryId,
                    BrandId = product.BrandId,
                    ProductPrice = product.ProductPrice,
                    Discount = product.Discount,
                    isActived = product.isActived,
                    ImgUrl1= product.ProductImg.Where(x=>x.SortOrder==1).FirstOrDefault()!=null? product.ProductImg.Where(x => x.SortOrder == 1).FirstOrDefault()!.ImagePath:"",
                    ImgUrl2 = product.ProductImg.Where(x => x.SortOrder == 2).FirstOrDefault() != null ? product.ProductImg.Where(x => x.SortOrder == 2).FirstOrDefault()!.ImagePath : "",
                    ImgUrl3 = product.ProductImg.Where(x => x.SortOrder == 3).FirstOrDefault() != null ? product.ProductImg.Where(x => x.SortOrder == 3).FirstOrDefault()!.ImagePath : "",
                    ImgUrl4 = product.ProductImg.Where(x => x.SortOrder == 4).FirstOrDefault() != null ? product.ProductImg.Where(x => x.SortOrder == 4).FirstOrDefault()!.ImagePath : "",
                    Id = product.Id,
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.Update(request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Cập nhật thông tin thành công";
                return RedirectToAction("Index");

            }

            ModelState.AddModelError("", "Thay đổi không thành công");
            return View(request);
        }
       
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _productApiClient.GetById(id);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.Delete(productId);
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
        }
    }
}
