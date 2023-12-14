using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BanThietBiDiDongDATN.Admin.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{statusCode}")]
        public IActionResult Index(int? statusCode)
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var errorMessage = exceptionFeature?.Error.Message;
            var stackTrace = exceptionFeature?.Error.StackTrace;

            ViewBag.StatusCode = statusCode;
            ViewBag.ErrorMessage = errorMessage;
            ViewBag.StackTrace = stackTrace;

            return View();
        }
    }
}
