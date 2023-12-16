using BanThietBiDiDongDATN.ApiIntegration.Service.CartApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.OrderApiClient;
using BanThietBiDiDongDATN.ApiIntegration.Service.VoucherApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Carts.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public CartController(ICartApiClient cartApiClient, IOrderService orderApiClient, IVoucherApiClient voucherApiClient)
        {
            _cartApiClient = cartApiClient;
            _orderApiClient = orderApiClient;
            _voucherApiClient = voucherApiClient;
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            //var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var Cart = await _cartApiClient.GetAllByUserId(Guid.Parse(UserId));

            return View();
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
                return RedirectToAction("Index", "Cart");
            }
            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View();
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
                    TempData["failed"] = "Voucher đã hết hạn";
                    return RedirectToAction("Index", "Cart");
                }
                else
                {
                    return Json(new { success=true,voucher=voucher});
                }
            }
            else
            {
                TempData["failed"] = "Voucher không chính xác";
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}
