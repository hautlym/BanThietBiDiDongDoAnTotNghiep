using BanThietBiDiDongDATN.Application.Catalog.Categories.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Categories;
using BanThietBiDiDongDATN.Data.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BanThietBiDiDongDoAn.Applications.Brands;
using BanThietBiDiDongDATN.Application.Catalog.Brands.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Commom;

namespace BanThietBiDiDongDATN.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IManageBrand _manage;

        public BrandController(IManageBrand manage)
        {
            _manage = manage;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var brand = await _manage.GetAllBrand();
            return Ok(brand);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] getBrandRequest request)
        {
            var categories = await _manage.GetAlllPaging(request);
            return Ok(categories);
        }

        [HttpGet("{brandId}")]
        public async Task<IActionResult> GetbyId(int brandId)
        {
            var brand = await _manage.GetById(brandId);
            if (!brand.IsSuccessed)
            {
                return NotFound(brand);
            }
            else
            {
                return Ok(brand);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBrandRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manage.Create(request);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut]

        public async Task<IActionResult> Update([FromForm] UpdateBrandRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Result = await _manage.Update(request);
            if (!Result.IsSuccessed)
                return BadRequest(Result);
            return Ok(Result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int brandID)
        {
            var Result = await _manage.Delete(brandID);
            if (!Result.IsSuccessed)
                return BadRequest(Result);
            return Ok(Result);
        }
    }
}
