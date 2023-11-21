using BanThietBiDiDongDATN.Data.entities;
using BanThietBiDiDongDATN.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Data.EF;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using BanThietBiDiDongDATN.Application.Catalog.Products.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Products.ProductImgs;

namespace BanThietBiDiDongDATN.Application.Catalog.Products
{
    public class ManageProduct : IManageProduct
    {
        private readonly BanThietBiDiDongDATNDbContext _context;
        private readonly IStorageService _storageService;
        public ManageProduct(BanThietBiDiDongDATNDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<string> SaveFile(IFormFile file)
        {

            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), "product", fileName);
            return fileName;
        }
        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.productImgs.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<ApiResult<bool>> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                ProductName = request.ProductName,
                ProductDescription = request.ProductDescription,
                Discount = request.Discount,
                CategoryId = request.CategoryId,
                ProductPrice = request.ProductPrice,
                BrandId = request.BrandId,
                isActived = request.isActived,
                CreateDate = DateTime.Now,
                Quantity = request.Quantity,
            };
            var listImg = new List<ProductImage>();
            if (request.productImg1 != null)
            {
                var productImgs1 = new ProductImage()
                {
                    Caption = "Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.productImg1.Length,
                    ImagePath = await this.SaveFile(request.productImg1),
                    IsDefault = true,
                    SortOrder = 1
                };
                listImg.Add(productImgs1);
            }
            if (request.productImg2 != null)
            {
                var productImgs1 = new ProductImage()
                {
                    Caption = "Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.productImg2.Length,
                    ImagePath = await this.SaveFile(request.productImg2),
                    IsDefault = true,
                    SortOrder = 2
                };
                listImg.Add(productImgs1);
            }
            if (request.productImg3 != null)
            {
                var productImgs1 = new ProductImage()
                {
                    Caption = "Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.productImg3.Length,
                    ImagePath = await this.SaveFile(request.productImg3),
                    IsDefault = true,
                    SortOrder = 3
                };
                listImg.Add(productImgs1);
            }
            if (request.productImg4 != null)
            {
                var productImgs1 = new ProductImage()
                {
                    Caption = "Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.productImg4.Length,
                    ImagePath = await this.SaveFile(request.productImg4),
                    IsDefault = true,
                    SortOrder = 4
                };
                listImg.Add(productImgs1);
            }
            product.productImgs = listImg;
            _context.products.Add(product);
            var kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Thêm sản phẩm không thành công");
        }

        public async Task<ApiResult<bool>> Delete(int productId)
        {
            var product = await _context.products.Where(x => x.Id == productId).FirstOrDefaultAsync();
            if (product == null)
                return new ApiErrorResult<bool>("Không tìm thấy sản phẩm");

            var image = _context.productImgs.Where(x => x.ProductId == productId);
            foreach (var img in image)
            {
                await _storageService.DeleteFileAsync("product", img.ImagePath);
            }
            _context.products.Remove(product);
            var kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Xóa sản phẩm không thành công");
        }

        public Task<ApiResult<List<ProductViewModel>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<ProductViewModel>>> GetAllPaging(GetProductPagingRequest request)
        {
            var query = from product in _context.products
                        join c in _context.Categories on product.CategoryId equals c.Id
                        join p in _context.brands on product.BrandId equals p.id
                        select new { product, c, p };
            if (!string.IsNullOrEmpty(request.keyword))
            {
                query = query.Where(x => x.product.ProductName.Contains(request.keyword));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => x.c.Id == request.CategoryId);
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.product.Id,
                    ProductName = x.product.ProductName,
                    ProductDescription = x.product.ProductDescription,
                    ProductPrice = x.product.ProductPrice,
                    CategoryName = x.c.CategoryName,
                    Discount = x.product.Discount,
                    CategoryId = x.product.CategoryId,
                    BrandId = x.product.BrandId,
                    BrandName = x.p.BrandName,
                    isActived = x.product.isActived,
                    Quantity = x.product.Quantity,
                    ProductImg = x.product.productImgs,
                }).ToListAsync();
            var pageResult = new PageResult<ProductViewModel>
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return new ApiSuccessResult<PageResult<ProductViewModel>>(pageResult);
        }

        public async Task<ApiResult<ProductViewModel>> GetById(int productId)
        {
            var products =await _context.products.Where(x => x.Id == productId).Select(product => new ProductViewModel()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductDescription = product.ProductDescription,
                Discount = product.Discount,
                CategoryId = product.CategoryId,
                ProductImg = product.productImgs
            }).FirstOrDefaultAsync();
            //if(product == null) 
            //    throw new BTL_KTPMException("Can not find product");
            return new ApiSuccessResult<ProductViewModel>(products);
        }

        public Task<ProductImageViewModel> GetImageById(int imageId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.productImgs.FindAsync(imageId);
            if (productImage == null)
                return 0;
            _context.productImgs.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<ApiResult<bool>> Update(ProductUpdateRequest request)
        {
            var product = _context.products.Where(x=>x.Id==request.Id).FirstOrDefault();
            if (product == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy sản phẩm");
            }
            product.ProductName= request.ProductName;
            product.ProductPrice = request.ProductPrice;
            product.ProductDescription = request.ProductDescription;
            product.BrandId = request.BrandId;
            product.isActived = request.isActived;
            product.Discount = request.Discount;
            product.CategoryId = request.CategoryId;
            var listImg = new List<ProductImage>();
            if (request.productImg1 != null)
            {
                
                var productImgs1 = new ProductImage()
                {
                    Caption = "Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.productImg1.Length,
                    ImagePath = await this.SaveFile(request.productImg1),
                    IsDefault = true,
                    SortOrder = 1
                };
                listImg.Add(productImgs1);
            }
            if (request.productImg2 != null)
            {
                var productImgs1 = new ProductImage()
                {
                    Caption = "Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.productImg2.Length,
                    ImagePath = await this.SaveFile(request.productImg2),
                    IsDefault = true,
                    SortOrder = 2
                };
                listImg.Add(productImgs1);
            }
            if (request.productImg3 != null)
            {
                var productImgs1 = new ProductImage()
                {
                    Caption = "Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.productImg3.Length,
                    ImagePath = await this.SaveFile(request.productImg3),
                    IsDefault = true,
                    SortOrder = 3
                };
                listImg.Add(productImgs1);
            }
            if (request.productImg4 != null)
            {
                var productImgs1 = new ProductImage()
                {
                    Caption = "Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.productImg4.Length,
                    ImagePath = await this.SaveFile(request.productImg4),
                    IsDefault = true,
                    SortOrder = 4
                };
                listImg.Add(productImgs1);
            }
            var image = _context.productImgs.Where(x => x.ProductId == request.Id);
            foreach (var img in image)
            {
                if(listImg.Where(x=>x.SortOrder== img.SortOrder).ToList().Count>0)
                await _storageService.DeleteFileAsync("product", img.ImagePath);
            }
            product.productImgs = listImg;
            _context.Update(product);
            var kq= await _context.SaveChangesAsync();
            if(kq>0)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Thay đổi thông tin không thành công");
            }
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.productImgs.FindAsync(imageId);
            if (productImage == null)
                return 0;

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.productImgs.Update(productImage);
            return await _context.SaveChangesAsync();
        }
    }
}
