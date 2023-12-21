using BanThietBiDiDongDATN.ApiIntegration.Service.OrderApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.ProductApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Carts;
using BanThietBiDiDongDATN.Application.Catalog.Products.Dtos;
using BanThietBiDiDongDATN.Data.entities;
using BanThietBiDiDongDATN.MvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BanThietBiDiDongDATN.MvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderService _orderService;
        public HomeController(ILogger<HomeController> logger, IProductApiClient productApiClient, IOrderService orderService)
        {
            _logger = logger;
            _productApiClient = productApiClient;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var product = await _productApiClient.GetAll();
                var ListProduct = product.ResultObj.Where(x => x.isActived == true).ToList();
                var HomeVm = new HomeModel();
                HomeVm.BannerProduct = ListProduct.Where(x => x.Discount > 0).Take(3).ToList();
                HomeVm.NewProductPhone = ListProduct.Where(x => x.CategoryId == 1).OrderByDescending(x => x.CreateDate).Take(6).ToList();
                HomeVm.NewProductHeadPhone = ListProduct.Where(x => x.CategoryId == 2).OrderByDescending(x => x.CreateDate).Take(6).ToList();
                HomeVm.NewProductCharge = ListProduct.Where(x => x.CategoryId == 3).OrderByDescending(x => x.CreateDate).Take(6).ToList();
                HomeVm.NewOtherProduuct = ListProduct.Where(x => x.CategoryId != 1 && x.CategoryId != 2 && x.CategoryId != 3).OrderByDescending(x => x.CreateDate).Take(6).ToList();
                HomeVm.RecomandProduct = ListProduct.Take(8).ToList();
                HomeVm.NewProduct = ListProduct.OrderByDescending(x => x.CreateDate).Take(6).ToList();
                var order = await _orderService.GetAll();
                var sale = order.ResultObj.Select(x => x.OrderDetails).ToList();
                var listDetail = new List<OrderDetail>();
                foreach (var item in sale)
                {
                    listDetail.AddRange(item);
                }
                HomeVm.TopSaleProduct = new List<ProductViewModel>();
                var list = listDetail.GroupBy(x => x.ProductId).Select(group => new
                {
                    ProductId = group.Key,
                    TotalQuantity = group.Sum(order => order.Quantity)
                }).OrderByDescending(result => result.TotalQuantity).Take(5).ToList();
                foreach (var item in list)
                {
                    var pr = ListProduct.Where(x => x.Id == item.ProductId).FirstOrDefault();
                    if (pr != null)
                        HomeVm.TopSaleProduct.Add(pr);
                }
                return View(HomeVm);
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error");
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Introduce()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}