using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.Products.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Products.ProductImgs;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Products
{
    public interface IManageProduct
    {
        Task<ApiResult<bool>> Create(ProductCreateRequest request);
        Task<ApiResult<bool>> Update(ProductUpdateRequest request);
        Task<ApiResult<ProductViewModel>> GetById(int productId);
        Task<ApiResult<bool>> Delete(int productId);
        Task<ApiResult<List<ProductViewModel>>> GetAll();
        Task<ApiResult<PageResult<ProductViewModel>>> GetAllPaging(GetProductPagingRequest request);
        Task<ApiResult<PageResult<ProductViewModel>>> PublicGetAll(GetPublicProductRequest request);
        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        Task<int> RemoveImage(int imageId);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetListImages(int productId);
    }
}
