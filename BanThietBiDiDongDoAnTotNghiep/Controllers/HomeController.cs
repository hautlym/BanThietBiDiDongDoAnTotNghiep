using BanThietBiDiDongDATN.Admin.Models;
using BanThietBiDiDongDATN.ApiIntegration.Service.OrderApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.ProductApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.UserApiClient;
using BanThietBiDiDongDATN.Data.entities;
using BanThietBiDiDongDATN.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace BanThietBiDiDongDATN.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderService _orderService;
        private readonly IUserApiClient _userApiClient;

        public HomeController(ILogger<HomeController> logger, IProductApiClient productApiClient, IOrderService orderService, IUserApiClient userApiClient)
        {
            _logger = logger;
            _productApiClient = productApiClient;
            _orderService = orderService;
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var order = (await _orderService.GetAll()).ResultObj;
            var product = (await _productApiClient.GetAll()).ResultObj;
            
            DateTime firstDayOfCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var ordersInCurrentMonth = order
                .Where(order => order.OrderDate >= firstDayOfCurrentMonth && order.OrderDate <= DateTime.Now)
                .OrderBy(order => order.OrderDate);

            // Sử dụng LINQ để lấy doanh thu theo ngày
            var revenueByDay = ordersInCurrentMonth.Where(x => x.status != OrderStatus.Cancel)
                .GroupBy(x=>x.OrderDate.Month)
                .Select(group => new
                {
                    Month = group.Key,
                    TotalPrice = group.Sum(order => order.OrderDetails.Select(x => x.Price).Sum()),
                    totalOrder = group.Count(),
                    totalProduct = group.Sum(order => order.OrderDetails.Select(x => x.Quantity).Sum()),
                }).FirstOrDefault();

           
            ViewBag.TongDoanhThu = revenueByDay.TotalPrice;
            ViewBag.TongSanPhamBan = revenueByDay.totalProduct;
            ViewBag.TongDonHang = revenueByDay.totalOrder;
            var listOrder = order.OrderByDescending(x=>x.OrderDate).Take(10).ToList();
            ViewBag.ListOrder = listOrder;
            return View();
        }

        public IActionResult Privacy()
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