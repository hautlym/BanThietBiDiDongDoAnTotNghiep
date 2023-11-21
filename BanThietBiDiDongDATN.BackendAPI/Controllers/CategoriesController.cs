
using BanThietBiDiDongDATN.Application.Catalog.Categories;
using BanThietBiDiDongDATN.Application.Catalog.Categories.Dtos;
using BanThietBiDiDongDATN.Data.EF;
using Microsoft.AspNetCore.Mvc;

namespace BTL_KTPM.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly BanThietBiDiDongDATNDbContext _context;
        private readonly IManageCategory _manageCategory;

        public CategoriesController(BanThietBiDiDongDATNDbContext context, IManageCategory manageCategory)
        {
            _context = context;
            _manageCategory = manageCategory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _manageCategory.GetAllCategory();
            return Ok(categories);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery]GetCategoryRequest request)
        {
            var categories = await _manageCategory.GetAlllPaging(request);
            return Ok(categories);
        }

        [HttpGet("{CategoryId}")]
        public async Task<IActionResult> GetbyId(int CategoryId)
        {
            var category = await _manageCategory.GetById(CategoryId);
            if (!category.IsSuccessed)
            {
                return NotFound(category);
            }
            else
            {
                return Ok(category);
            }
        }

        [HttpPost("add_category")]
        public async Task<IActionResult> Create( CreateCategoryRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageCategory.Create(request);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update( UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Result = await _manageCategory.Update(request);
            if (!Result.IsSuccessed)
                return BadRequest(request);
            return Ok(Result);
        }

        [HttpDelete("{CategoryId}")]
        public async Task<IActionResult> Delete(int CategoryId)
        {
            var Result = await _manageCategory.Delete(CategoryId);
            if (Result.IsSuccessed)
                return BadRequest(Result);

            return Ok(Result);
        }
    }
}