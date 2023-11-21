using BanThietBiDiDongDATN.Application.Catalog.Categories.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Categories
{
    public interface IManageCategory
    {
        Task<ApiResult<List<CategoryViewModels>>> GetAllCategory();
        Task<ApiResult<PageResult<CategoryViewModels>>> GetAlllPaging(GetCategoryRequest request);
        Task<ApiResult<bool>> Create(CreateCategoryRequest request);
        Task<ApiResult<bool>> Update(UpdateCategoryRequest request);
        Task<ApiResult<bool>> Delete(int CategoryId);
        Task<ApiResult<CategoryViewModels>> GetById(int categoryId);
    }
}
