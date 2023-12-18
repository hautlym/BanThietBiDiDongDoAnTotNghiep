using BanThietBiDiDongDATN.Data.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BanThietBiDiDongDATN.Application.Catalog.Vouchers;
using BanThietBiDiDongDATN.Application.Catalog.Vouchers.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices;

namespace BanThietBiDiDongDATN.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VoucherController : ControllerBase
    {
        private readonly BanThietBiDiDongDATNDbContext _context;
        private readonly IManageVoucher _manageVoucher;

        public VoucherController(BanThietBiDiDongDATNDbContext context, IManageVoucher manageVoucher)
        {
            _context = context;
            _manageVoucher = manageVoucher;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var categories = await _manageVoucher.GetAll();
            return Ok(categories);
        }
        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetVoucherRequest request)
        {
            var categories = await _manageVoucher.GetAlllPaging(request);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetbyId(int id)
        {
            var item = await _manageVoucher.GetById(id);
            if (!item.IsSuccessed)
            {
                return NotFound(item);
            }
            else
            {
                return Ok(item);
            }
        }

        [HttpPost("add_voucher")]
        public async Task<IActionResult> Create(CreateVoucherRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageVoucher.Create(request);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateVoucherRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Result = await _manageVoucher.Update(request);
            if (!Result.IsSuccessed)
                return BadRequest(request);
            return Ok(Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Result = await _manageVoucher.Delete(id);
            if (Result.IsSuccessed)
                return BadRequest(Result);

            return Ok(Result);
        }
    }
}
