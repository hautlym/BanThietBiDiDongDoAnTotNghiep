using BanThietBiDiDongDATN.ApiIntegration.Service.BrandApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.CategoryApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.ProductApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Products.Dtos;
using BanThietBiDiDongDATN.MvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BanThietBiDiDongDATN.MvcApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoriesApiClient _categoriesApiClient;
        private readonly IBrandApiClient _brandApiClient;
        public ProductController(IProductApiClient productApiClient, IBrandApiClient brandApiClient, ICategoriesApiClient categoriesApiClient)
        {
            _productApiClient = productApiClient;
            _brandApiClient = brandApiClient;
            _categoriesApiClient = categoriesApiClient;
        }
        public async Task<IActionResult> Index(string? keyword, int? categoryId, int? BrandId, int? SortBy, int? Price, int typeClear = 1, int PageIndex = 1, int PageSize = 9)
        {
            var request = new GetPublicProductRequest()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                Keywword = typeClear != 0 ? keyword : "",
                CategoryId = typeClear != 0 ? categoryId : 0,
                BrandId = typeClear != 0 ? BrandId : 0,
                Price = typeClear != 0 ? Price : 0,
                SortBy = typeClear != 0 ? SortBy : 0
            };
            var brand = await _brandApiClient.GetAllBrand();
            var category = await _categoriesApiClient.GetAll();
            var data = await _productApiClient.PublicGetAll(request);
            ViewBag.Categories = category.ResultObj;
            ViewBag.Brand = brand.ResultObj;
            ViewBag.CheckedCategory = request.CategoryId == null ? 0 : request.CategoryId;
            ViewBag.CheckedBrand = request.BrandId == null ? 0 : request.BrandId;
            ViewBag.CheckedSort = request.SortBy == null ? 0 : request.SortBy;
            ViewBag.CheckedPrice = request.Price == null ? 0 : request.Price;
            return View(data.ResultObj);
        }
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var product = await _productApiClient.GetById(id);
                var listProduct = await _productApiClient.GetAll();
                var listRelated = listProduct.ResultObj.Where(x => x.CategoryId == product.ResultObj.CategoryId).Take(5).ToList();
                var model = new DeltailModel()
                {
                    Product = product.ResultObj,
                    RelatedProduct = listRelated
                };
                return View(model);
            }catch(Exception ex )
            {
                return RedirectToAction("Index");
            }
        }
    }
}
