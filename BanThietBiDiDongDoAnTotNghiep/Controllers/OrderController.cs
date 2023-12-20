using BanThietBiDiDongDATN.ApiIntegration.Service.OrderApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.ProductApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.VoucherApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Brands.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Carts;
using BanThietBiDiDongDATN.Application.Catalog.Orders.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BanThietBiDiDongDATN.Admin.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderApiClient;
        private readonly IProductApiClient _productApiClient;
        private readonly IVoucherApiClient _voucherApiClient;
        public OrderController(IOrderService orderApiClient, IProductApiClient productApiClient, IVoucherApiClient voucherApiClient)
        {
            _voucherApiClient = voucherApiClient;
            _orderApiClient = orderApiClient;
            _productApiClient = productApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int PageIndex = 1, int PageSize = 10)
        {
            var request = new GetOrderRequest()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                Keyword = keyword,
            };
            var data = await _orderApiClient.GetAllPaging(request);
            ViewBag.Keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.SuscessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }
        [HttpGet]
        public async Task<IActionResult> Create(int numberProduct = 1)
        {
            var product = await _productApiClient.GetAll();

            ViewBag.Product = product.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.ProductName,
                Value = x.Id.ToString(),
            });
            ViewBag.NumberProduct = numberProduct;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> getProductOption(int Id)
        {
            var product = await _productApiClient.GetById(Id);
            if (product.ResultObj == null)
            {
                return Json("");
            }
            var item = product.ResultObj.Options.Select(x => new SelectListItem()
            {
                Text = x.ColorOption + "-" + x.SizeOption,
                Value = x.id.ToString(),
            });

            return Json(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderRequest request, int numberProduct = 1)
        {
            var product = await _productApiClient.GetAll();
            ViewBag.NumberProduct = numberProduct;
            ViewBag.Product = product.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.ProductName,
                Value = x.Id.ToString(),
            });
            if (!ModelState.IsValid)
                return View(request);
            var ListVoucher = await _voucherApiClient.GetAll();
            var voucher = ListVoucher.ResultObj.Where(x => x.VoucherCode == request.voucherId).FirstOrDefault();
            if (voucher != null)
            {
                if (voucher.BeginDate > DateTime.Now || voucher.ExpiredDate < DateTime.Now)
                {
                    ModelState.AddModelError("voucherId", "Voucher đã hết hạn");
                    return View(request);
                }
                if (voucher.Quantity <= 0)
                {
                    ModelState.AddModelError("voucherId", "Voucher đã hết");
                    return View(request);
                }
            }
            request.AppUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _orderApiClient.CreateOrder(request, 1);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Thêm đơn hàng thành công";
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
                    TempData["failed"] = "Thêm đơn hàng không thành công";
                }
                return RedirectToAction("Index");
            }
        }
        public IActionResult PrinfilePDF()
        {
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int OrderId, OrderStatus status)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _orderApiClient.ChangeStatus(OrderId, status);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Đổi trạng thái thành công";
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
                    TempData["failed"] = "Đổi trạng thái không thành công";
                }
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _orderApiClient.GetById(id);
            var ListCart = new List<CartViewModel>();
            foreach (var item in result.ResultObj.OrderDetails)
            {
                var product = await _productApiClient.GetById(item.ProductId);
                var option = product.ResultObj.Options.Where(x => x.id == item.OptionId).FirstOrDefault();
                var newPrice = product.ResultObj.Discount > 0 ? (Convert.ToDouble(option.OptionPrice) - (Convert.ToDouble(option.OptionPrice) * (product.ResultObj.Discount / 100))) : Convert.ToDouble(option.OptionPrice);
                var totalPrice = newPrice * item.Quantity;
                var cart = new CartViewModel()
                {
                    Discount = product.ResultObj.Discount,
                    ProductNane = product.ResultObj.ProductName,
                    ProductOriginal = Convert.ToDouble(option.OptionPrice),
                    ProductPrice = newPrice,
                    Quantity = item.Quantity,
                    ImgUrl = product.ResultObj.ProductImg.Count > 0 ? product.ResultObj.ProductImg[0].ImagePath : "",
                    totalPrice = totalPrice.ToString(),
                    OptionColor = option.ColorOption,
                    OptionSize = option.SizeOption,
                    ProductId = item.ProductId
                };
                ListCart.Add(cart);
            }
            ViewBag.Cart = ListCart;
            return View(result.ResultObj);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int orderId)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _orderApiClient.Delete(orderId);
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
                    TempData["failed"] = "Xóa không thành công";
                }
                return RedirectToAction("Index");
            }
        }
    }
}
