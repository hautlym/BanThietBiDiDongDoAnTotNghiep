using BanThietBiDiDongDATN.ApiIntegration.Service.CartApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.OrderApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.VoucherApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Carts.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Orders.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.Enums;
using BanThietBiDiDongDATN.MvcApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;

namespace BanThietBiDiDongDATN.MvcApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartApiClient _cartApiClient;
        private readonly IOrderService _orderApiClient;
        private readonly IVoucherApiClient _voucherApiClient;
        private readonly IVNPayMethod _vNPayMethod;
        private string vnp_TmnCode = "WF6HGPRL";
        private string vnp_SecureHash = "DHZVHWNWCSGCNXNHVYTDEYZVBAXJASXN";
        public CartController(ICartApiClient cartApiClient, IOrderService orderApiClient, IVoucherApiClient voucherApiClient,
            IVNPayMethod vnPayService)
        {
            _cartApiClient = cartApiClient;
            _orderApiClient = orderApiClient;
            _voucherApiClient = voucherApiClient;
            _vNPayMethod = vnPayService;
        }
        public async Task<IActionResult> Index()
        {

            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var Cart = await _cartApiClient.GetAllByUserId(Guid.Parse(UserId));

            if (Cart == null)
            {
                return View();
            }
            return View(Cart);
        }
        public async Task<IActionResult> CheckOutView()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Cart = await _cartApiClient.GetAllByUserId(Guid.Parse(UserId));
            return View(Cart);
        }
        [HttpPost]
        public async Task<IActionResult> CheckOut(CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("CheckOutView", "Cart");
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Cart = await _cartApiClient.GetAllByUserId(Guid.Parse(UserId));
            if (Cart == null || Cart.Count < 0)
            {
                return RedirectToAction("Index", "Cart");
            }
            var ListVoucher = await _voucherApiClient.GetAll();
            var voucher = ListVoucher.ResultObj.Where(x => x.VoucherCode == request.voucherId).FirstOrDefault();
            if (request.typePayment == 0)
            {
                request.AppUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _orderApiClient.CreateOrder(request, 0);
                if (result.IsSuccessed)
                {
                    var kq2 = await _orderApiClient.DeleteCart(Guid.Parse(UserId));
                    TempData["success"] = "Thêm đơn hàng thành công";
                    return RedirectToAction("CheckOutDetail", "Cart");
                }
                else
                {
                    return RedirectToAction("CheckOutView", "Cart");
                }
            }
            else
            {
                var total = Cart.Select(x => Convert.ToDouble(x.totalPrice)).Sum();
                if (voucher != null)
                {
                    total = total - (total * (voucher.Discount / 100));
                }
                TempData["request"] =JsonConvert.SerializeObject(request);
                var order = new PaymentInformationModel()
                {
                    Amount = total,
                    Name = request.ShipName,
                    OrderDescription = request.ShipDescription,
                    OrderType = request.typePayment.ToString(),
                    createOrderRequest = request
                };
                var url = _vNPayMethod.CreatePaymentUrl(order, HttpContext);
                return Redirect(url);
            }

        }
        public async Task<IActionResult> PayOnline()
        {
            var response = _vNPayMethod.PaymentExecute(Request.Query);
            if (response.Success)
            {
                if (response.VnPayResponseCode.Equals("00"))
                {
                    var ListVoucher = await _voucherApiClient.GetAll();
                    var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    string value = (string)TempData["request"];
                    var request = JsonConvert.DeserializeObject<CreateOrderRequest>(value);
                    var voucher = ListVoucher.ResultObj.Where(x => x.VoucherCode == request.voucherId).FirstOrDefault();
                    request.AppUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    var result = await _orderApiClient.CreateOrder(request, 0);
                    if (result.IsSuccessed)
                    {
                        var kq2 = await _orderApiClient.DeleteCart(Guid.Parse(UserId));
                        TempData["success"] = "Thêm đơn hàng thành công";
                        return RedirectToAction("CheckOutDetail", "Cart");
                    }
                    else
                    {
                        return RedirectToAction("CheckOutView", "Cart");
                    }
                }
                else
                {
                    return RedirectToAction("CheckOutView", "Cart");
                }
            }
            else
            {
                return RedirectToAction("CheckOutView", "Cart");
            }
        }
        [HttpGet]
        public async Task<IActionResult> CheckOutDetail()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Order = await _orderApiClient.GetAllByUserId(Guid.Parse(UserId));
            return View(Order.ResultObj);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int OrderId, OrderStatus status)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _orderApiClient.ChangeStatus(OrderId, status);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Đổi trạng thái thành công";
                return RedirectToAction("CheckOutView", "Cart");
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
                return RedirectToAction("CheckOutView", "Cart");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCheckOut(int OrderId)
        {
            var kq = await _orderApiClient.Delete(OrderId);
            if (kq.IsSuccessed)
            {
                return RedirectToAction("CheckOutDetail", "Cart");
            }

            return RedirectToAction("CheckOutDetail", "Cart");
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int ProductId, int Quantity, int OptionId)
        {
            if (Quantity == 0)
            {
                TempData["failed"] = "Sản phẩm đã hết hàng";
                Redirect("/Product/Detail/" + ProductId);
            }
            var Cart = new CreateCartRequest()
            {
                UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ProductId = ProductId,
                Quantity = Quantity,
                OptionId = OptionId
            };
            var kq = await _cartApiClient.CreateCart(Cart);
            if (kq.IsSuccessed)
            {
                return RedirectToAction("Index", "Cart");
            }
            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeQuantity(int cartId, int Quantity)
        {


            var Cart = new UpdateCartRequest()
            {
                id = cartId,
                Quantity = Quantity,
            };
            var kq = await _cartApiClient.UpdateCart(cartId, Cart);

            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> DeleteToCart(int CartId)
        {
            var kq = await _cartApiClient.Delete(CartId);
            if (kq)
            {
                return RedirectToAction("CheckOutDetail", "Cart");
            }
            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return RedirectToAction("CheckOutDetail", "Cart");
        }
        [HttpGet]
        public async Task<IActionResult> ApplyVoucher(string voucherCode)
        {

            var listVoucher = await _voucherApiClient.GetAll();
            var voucher = listVoucher.ResultObj.Where(x => x.VoucherCode == voucherCode).FirstOrDefault();
            if (voucher != null)
            {
                if (voucher.BeginDate > DateTime.Now || voucher.ExpiredDate < DateTime.Now || voucher.Quantity <= 0)
                {
                    return Json(new { success = false, voucher = "", mess = "Voucher đã hết hạn" });
                }
                else
                {
                    return Json(new { success = true, voucher = voucher, mess = "" });
                }
            }
            else
            {
                return Json(new { success = false, voucher = "", mess = "Voucher không chính xác" });
            }
        }
    }
}
