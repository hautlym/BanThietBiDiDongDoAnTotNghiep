
using BanThietBiDiDongDATN.Application.Catalog.Categories;
using BanThietBiDiDongDATN.Application.Catalog.Categories.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;

namespace BanThietBiDiDongDATN.ApiIntegration.Service.CategoryApiClient
{
    public interface ICategoriesApiClient
    {
        Task<ApiResult<List<CategoryViewModels>>> GetAll();
        Task<ApiResult<PageResult<CategoryViewModels>>> GetAllPaging(GetCategoryRequest request);
        Task<ApiResult<bool>> CreateCategory(CreateCategoryRequest request);
        Task<ApiResult<bool>> UpdateCategory(int id, UpdateCategoryRequest request);
        Task<ApiResult<CategoryViewModels>> GetById(int id);
        Task<ApiResult<bool>> Delete(int id);
    }
}
