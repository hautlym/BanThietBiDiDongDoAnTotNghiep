
using BanThietBiDiDongDATN.ApiIntegration.Service.VoucherApiClient;
using BanThietBiDiDongDATN.Application.Catalog.Vouchers.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BanThietBiDiDongDATN.Admin.Controllers
{
    public class VoucherController : Controller
    {
        private readonly IVoucherApiClient _voucherApiClient;

        public VoucherController(IVoucherApiClient voucherApiClient)
        {
           _voucherApiClient = voucherApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int PageIndex = 1, int PageSize = 10)
        {
            var request = new GetVoucherRequest()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                keyword = keyword,
            };
            var data = await _voucherApiClient.GetAlllPaging(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuscessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateVoucherRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _voucherApiClient.Create(request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Thêm voucher thành công";
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
                    TempData["failed"] = "Thêm voucher không thành công";
                }
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _voucherApiClient.GetById(id);
            if (result != null)
            {
                var updateRequest = new UpdateVoucherRequest()
                {
                    Id = result.ResultObj.Id,
                    VoucherCode = result.ResultObj.VoucherCode,
                    VoucherName = result.ResultObj.VoucherName,
                    BeginDate = result.ResultObj.BeginDate,
                    ExpiredDate = result.ResultObj.ExpiredDate,
                    Quantity = result.ResultObj.Quantity,
                    Discount = result.ResultObj.Discount,
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateVoucherRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _voucherApiClient.Update(request);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Sửa voucher thành công";
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
                    TempData["failed"] = "Sửa voucher không thành công";
                }
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", " Cập nhật không thành công");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var result =await _voucherApiClient.GetById(id);
            if(result.IsSuccessed)
            {
                return View(result.ResultObj);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id )
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _voucherApiClient.Delete(id);
            if (result.IsSuccessed)
            {
                TempData["success"] = "Xóa voucher thành công";
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
                    TempData["failed"] = "Xóa voucher không thành công";
                }
                return RedirectToAction("Index");
            }
        }
    }
}
