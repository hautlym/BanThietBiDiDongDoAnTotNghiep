using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.Products.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.ApiIntegration.Service.ProductApiClient
{
    public interface IProductApiClient
    {
        Task<ApiResult<bool>> Create(ProductCreateRequest request);
        Task<ApiResult<bool>> Update(ProductUpdateRequest request);
        Task<ApiResult<ProductViewModel>> GetById(int productId);
        Task<ApiResult<bool>> Delete(int productId);
        Task<ApiResult<List<ProductViewModel>>> GetAll();
        Task<ApiResult<PageResult<ProductViewModel>>> GetAllPaging(GetProductPagingRequest request);
    }
}
