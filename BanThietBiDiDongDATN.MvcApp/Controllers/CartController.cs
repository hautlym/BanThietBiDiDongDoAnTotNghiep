using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BanThietBiDiDongDATN.MvcApp.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
    }
}
