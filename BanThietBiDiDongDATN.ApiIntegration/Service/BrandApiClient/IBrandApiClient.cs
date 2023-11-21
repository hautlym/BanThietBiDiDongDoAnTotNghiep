using BanThietBiDiDongDATN.Application.Catalog.Brands.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.ApiIntegration.Service.BrandApiClient
{
    public interface IBrandApiClient
    {
        public Task<ApiResult<List<Brand>>> GetAllBrand();
        public Task<ApiResult<PageResult<Brand>>> GetAlllPaging(getBrandRequest request);
        public Task<ApiResult<bool>> Create(CreateBrandRequest request);
        public Task<ApiResult<bool>> Update(UpdateBrandRequest request);
        public Task<ApiResult<bool>> Delete(int CategoryId);
        public Task<ApiResult<Brand>> GetById(int categoryId);
    }
}
