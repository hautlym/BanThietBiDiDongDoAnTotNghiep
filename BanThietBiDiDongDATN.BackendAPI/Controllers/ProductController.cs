using BanThietBiDiDongDATN.Application.Catalog.Products;
using BanThietBiDiDongDATN.Application.Catalog.Products.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanThietBiDiDongDATN.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IManageProduct _manageProduct;

        public ProductController(IManageProduct manageProduct)
        {
            _manageProduct = manageProduct;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var product = await _manageProduct.GetAll();
            return Ok(product);
        }

    
        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] GetProductPagingRequest request)
        {
            var product = await _manageProduct.GetAllPaging(request);
            return Ok(product);
        }

        [HttpGet("{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int productId)
        {
            var product = await _manageProduct.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProduct.Create(request);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Result = await _manageProduct.Update(request);
            if (!Result.IsSuccessed)
                return BadRequest(Result);
            return Ok(Result);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var Result = await _manageProduct.Delete(productId);
            if (!Result.IsSuccessed)
                return BadRequest(Result);

            return Ok(Result);
        }
    }
}
