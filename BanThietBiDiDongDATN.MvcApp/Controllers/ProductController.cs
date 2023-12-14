using Microsoft.AspNetCore.Mvc;

namespace BanThietBiDiDongDATN.MvcApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
    }
}
