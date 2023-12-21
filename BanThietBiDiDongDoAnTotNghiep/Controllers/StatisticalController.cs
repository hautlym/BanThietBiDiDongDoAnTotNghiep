using BanThietBiDiDongDATN.ApiIntegration.Service.OrderApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.ProductApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace BanThietBiDiDongDATN.Admin.Controllers
{
    [Authorize]
    public class StatisticalController : BaseController
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderService _orderService;
        public StatisticalController(IOrderService orderService, IProductApiClient productApiClient)
        {
            _orderService = orderService;
            _productApiClient = productApiClient;
        }
        public async Task<IActionResult> Index(DateTime? begin, DateTime? end)
        {
            var order = (await _orderService.GetAll()).ResultObj;
            var product = (await _productApiClient.GetAll()).ResultObj;
            ViewBag.Product= product.Select(x => new SelectListItem()
            {
                Text =x.ProductName,
                Value = x.Id.ToString(),
            });
            DateTime firstDayOfCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            #region thongke theo thang
            // Lấy danh sách đơn hàng từ đầu tháng đến ngày hiện tại
            var ordersInCurrentMonth = order
                .Where(order => order.OrderDate >= firstDayOfCurrentMonth && order.OrderDate <= DateTime.Now)
                .OrderBy(order => order.OrderDate);

            // Sử dụng LINQ để lấy doanh thu theo ngày
            var revenueByDay = ordersInCurrentMonth.Where(x=>x.status!= OrderStatus.Cancel)
                .GroupBy(order => order.OrderDate.Date)
                .Select(group => new
                {
                    Date = group.Key,
                    TotalAmount = group.Sum(order => order.OrderDetails.Select(x=>x.Price).Sum()),
                    NumberOfOrders = group.Count()
                });
            int soNgayTrongThang = DateTime.DaysInMonth(revenueByDay.FirstOrDefault().Date.Year, revenueByDay.FirstOrDefault().Date.Month);
            var listDataMonth = new List<double>();
            var listLabelMonth = new List<string>();
            var listOrderMonth = new List<int>();    
            for (int i = 1;i<= soNgayTrongThang; i++)
            {
                listLabelMonth.Add(i.ToString()+ "/"+revenueByDay.FirstOrDefault().Date.Month);
                var value = revenueByDay.Where(x => x.Date.Day == i).FirstOrDefault();
                if(value!=null)
                {
                    listDataMonth.Add(value.TotalAmount);
                    listOrderMonth.Add(value.NumberOfOrders);
                }
                else
                {
                    listOrderMonth.Add(0);
                    listDataMonth.Add(0);
                }
            }
            ViewBag.DatesByMonth = listLabelMonth;
            ViewBag.AmountsByMoth = listDataMonth;
            ViewBag.NumberOrder = listOrderMonth;
            #endregion

            #region thongke theo tuan
            DateTime today = DateTime.Today;
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            DateTime startOfWeek = today.AddDays(  daysUntilMonday-7);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            var listDataWeek = new List<double>();
            var listLabelWeek = new List<string>();
            var NumberOrderWeek = new List<int>();
            var weeklyRevenue = order
                .Where(s => s.OrderDate >= startOfWeek && s.OrderDate <= endOfWeek)
                .GroupBy(s => s.OrderDate.DayOfWeek)
                .Select(group => new
                {
                    DayOfWeek = group.Key,
                    NumberOrder = group.Count(),
                    TotalAmount = group.Sum(order => order.OrderDetails.Select(x => x.Price).Sum()),
                })
                .OrderBy(item => item.DayOfWeek)
                .ToList();
            foreach (DayOfWeek thu in Enum.GetValues(typeof(DayOfWeek)))
            {
                listLabelWeek.Add(HelpperMethod.ChuyenDoiThuSangChu(thu));
                var value = weeklyRevenue.Where(x => x.DayOfWeek == thu).FirstOrDefault();
                if(value != null)
                {
                    listDataWeek.Add(value.TotalAmount);
                    NumberOrderWeek.Add(value.NumberOrder);
                }
                else
                {
                    listDataWeek.Add(0);
                    NumberOrderWeek.Add(0);
                }
            }
            ViewBag.labelWeek = listLabelWeek;
            ViewBag.DataWeek = listDataWeek;
            ViewBag.OrderWeek = NumberOrderWeek;
            #endregion
            #region theo ngay
            var lableDay = new List<string>();
            var dataDay = new List<double>();
            var OrderDay = new List<int>();
            var TodayRevenue = order
                .Where(s => s.OrderDate.Day == DateTime.Today.Day )
                .GroupBy(s => s.OrderDate.Hour)
                .Select(group => new
                {
                    Hour = group.Key,
                    NumberOrder = group.Count(),
                    TotalAmount = group.Sum(order => order.OrderDetails.Select(x => x.Price).Sum()),
                })
                .OrderBy(item => item.Hour)
                .ToList();
            for (int i = 1;i<=24;i++)
            {
                lableDay.Add(i.ToString()+"h");
                var value = TodayRevenue.Where(x => x.Hour == i).FirstOrDefault();
                if(value!=null)
                {
                    dataDay.Add(value.TotalAmount);
                    OrderDay.Add(value.NumberOrder);
                }
                else
                {
                    dataDay.Add(0);
                    OrderDay.Add(0);
                }
            }
            ViewBag.OrderDay = OrderDay;
            ViewBag.labelDay = lableDay;
            ViewBag.dataDay = dataDay;
            #endregion
            #region phuongThucThanhToan
            var lablePayment = new List<string>();
            var dataPayment = new List<double>();
            var OrderPayment = new List<int>();
            var payment= order
               .GroupBy(s => s.typePayment)
               .Select(group => new
               {
                   payment = group.Key,
                   NumberOrder = group.Count(),
                   TotalAmount = group.Sum(order => order.OrderDetails.Select(x => x.Price).Sum()),
               })
               .OrderBy(item => item.payment)
               .ToList();
            foreach (var item in payment)
            {
                lablePayment.Add(item.payment.ToString());
                dataPayment.Add(item.NumberOrder);
            }
            ViewBag.lablePayment = lablePayment;
            ViewBag.dataPayment = dataPayment;
            #endregion
            #region ThongKeSoDon
            var OrderData = new List<int>();
            var OrderLabel = new List<string>();    
            var OrderNumber = ordersInCurrentMonth
               .GroupBy(order => order.status)
               .Select(group => new
               {
                   Status = group.Key,
                   NumberOfOrders = group.Count()
               })
               .OrderBy(item => item.Status)
            .ToList();
            foreach (OrderStatus item in Enum.GetValues(typeof(OrderStatus)))
            {
                OrderLabel.Add(item.ToString());
                var value = OrderNumber.Where(x => x.Status == item).FirstOrDefault();
                if(value != null)
                {
                    OrderData.Add(value.NumberOfOrders);
                }
                else
                {
                    OrderData.Add(0);
                }
            }
            ViewBag.OrderLabel = OrderLabel;
            ViewBag.OrderData = OrderData;
            #endregion
            #region topProduct 
            var topProduct = order
                .Where(x=>x.status!= OrderStatus.Cancel)
                .Select(x => x.OrderDetails).ToList().SelectMany(x=>x)
              .GroupBy(s => s.ProductId)
              .Select(group => new
              {
                  product = group.Key,
                  QuantityProduct = group.Select(x=>x.Quantity).Sum(),
              })
              .OrderByDescending(item => item.QuantityProduct).Take(5)
              .ToList();
            var listProduct = new Dictionary<string, int>();
            foreach (var item in topProduct)
            {
                var value = (await _productApiClient.GetById(item.product)).ResultObj;
                listProduct.Add(value.ProductName, item.QuantityProduct);
            }
            ViewBag.TopProduct = listProduct;
            #endregion
         
            return View();
        }
    }
}
