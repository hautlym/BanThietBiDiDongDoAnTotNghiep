using BanThietBiDiDongDATN.Application.Catalog.Brands.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Http;

namespace BanThietBiDiDongDoAn.Applications.Brands
{
    public interface IManageBrand
    {
        public Task<ApiResult< List<Brand>>> GetAllBrand();
        public Task<ApiResult<PageResult<Brand>>> GetAlllPaging(getBrandRequest request);
        public Task<ApiResult<bool>> Create(CreateBrandRequest request);
        public Task<ApiResult<bool>> Update(UpdateBrandRequest request);
        public Task<ApiResult<bool>> Delete(int CategoryId);
        public Task<ApiResult<Brand>> GetById(int categoryId);
    }
}
