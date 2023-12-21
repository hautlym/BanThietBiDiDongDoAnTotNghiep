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
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Pkcs;

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
                BrandId = request.BrandId,
                isActived = request.isActived,
                CreateDate = DateTime.Now,
                BeginDateDiscount = request.BeginDateDiscount,
                ExpiredDateDiscount = request.ExpiredDateDiscount,
            };
            var listOption = new List<ProductOption>();
            foreach (var item in request.Options)
            {
                var option = new ProductOption()
                {
                    ColorOption = item.ColorOption,
                    SizeOption = item.SizeOption,
                    OptionPrice = item.OptionPrice,
                    Quantity = item.Quantity,
                };
                listOption.Add(option);
            }
            product.productOptions = listOption;
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

            var image = _context.productImgs.Where(x => x.ProductId == productId).ToList();
            if (image.Count > 0)
            {
                foreach (var img in image)
                {
                    await _storageService.DeleteFileAsync("product", img.ImagePath);
                }
            }
            _context.products.Remove(product);
            var kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Xóa sản phẩm không thành công");
        }

        public async Task<ApiResult<List<ProductViewModel>>> GetAll()
        {
            var query = await _context.products.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                ProductName = x.ProductName,
                ProductDescription = x.ProductDescription,
                Options = x.productOptions.Select(x => new ProductOptionViewModel()
                {
                    id = x.Id,
                    ColorOption = x.ColorOption,
                    SizeOption = x.SizeOption,
                    OptionPrice = x.OptionPrice,
                    Quantity = x.Quantity
                }).ToList(),
                CategoryName = x.category.CategoryName,
                Discount = x.Discount,
                CategoryId = x.CategoryId,
                BrandId = x.BrandId,
                BrandName = x.brand.BrandName,
                isActived = x.isActived,
                Quantity = x.productOptions.Select(x => x.Quantity).Sum(),
                BeginDateDiscount = x.BeginDateDiscount,
                ExpiredDateDiscount = x.ExpiredDateDiscount,
                ProductImg = x.productImgs,
                CreateDate = x.CreateDate,
            }).ToListAsync();
            return new ApiSuccessResult<List<ProductViewModel>>(query);

        }

        public async Task<ApiResult<PageResult<ProductViewModel>>> GetAllPaging(GetProductPagingRequest request)
        {
            var query = from product in _context.products
                        join c in _context.Categories on product.CategoryId equals c.Id
                        join p in _context.brands on product.BrandId equals p.id
                        select new { product, c, p };
            var test = _context.products.ToList();
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
                    Options = x.product.productOptions.Select(x => new ProductOptionViewModel()
                    {
                        id = x.Id,
                        ColorOption = x.ColorOption,
                        SizeOption = x.SizeOption,
                        OptionPrice = x.OptionPrice,
                        Quantity = x.Quantity
                    }).ToList(),
                    CategoryName = x.product.category.CategoryName,
                    Discount = x.product.Discount,
                    CategoryId = x.product.CategoryId,
                    BrandId = x.product.BrandId,
                    BrandName = x.product.brand.BrandName,
                    isActived = x.product.isActived,
                    Quantity = x.product.productOptions.Select(x => x.Quantity).Sum(),
                    BeginDateDiscount = x.product.BeginDateDiscount,
                    ExpiredDateDiscount = x.product.ExpiredDateDiscount,
                    ProductImg = x.product.productImgs
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
            var products = await _context.products.Where(x => x.Id == productId).Select(product => new ProductViewModel()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Options = product.productOptions.Select(x => new ProductOptionViewModel()
                {
                    id = x.Id,
                    ColorOption = x.ColorOption,
                    SizeOption = x.SizeOption,
                    OptionPrice = x.OptionPrice,
                    Quantity = x.Quantity
                }).ToList(),
                Quantity = product.productOptions.Select(x => x.Quantity).Sum(),
                ProductDescription = product.ProductDescription,
                Discount = product.Discount,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                BrandName = product.brand.BrandName,
                CategoryName = product.category.CategoryName,
                BeginDateDiscount = product.BeginDateDiscount,
                ExpiredDateDiscount = product.ExpiredDateDiscount,
                isActived = product.isActived,
                ProductImg = product.productImgs,
                CreateDate = product.CreateDate
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
            try
            {
                var product = _context.products.Where(x => x.Id == request.Id).FirstOrDefault();
                if (product == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy sản phẩm");
                }
                product.ProductName = request.ProductName;
                product.ProductDescription = request.ProductDescription;
                product.BrandId = request.BrandId;
                product.isActived = request.isActived;
                product.Discount = request.Discount;
                product.CategoryId = request.CategoryId;
                product.BeginDateDiscount = request.BeginDateDiscount;
                product.ExpiredDateDiscount = request.ExpiredDateDiscount;
                product.productImgs = _context.productImgs.Where(x => x.ProductId == product.Id).ToList();
                var listOption1 = _context.productOptions.Where(x => x.ProductId == request.Id).ToList();
                _context.productOptions.RemoveRange(listOption1);
                var listOption = new List<ProductOption>();
                foreach (var item in request.Options)
                {
                    var option = new ProductOption()
                    {
                        ColorOption = item.ColorOption,
                        SizeOption = item.SizeOption,
                        OptionPrice = item.OptionPrice,
                        Quantity = item.Quantity,
                    };
                    listOption.Add(option);
                }
                product.productOptions = listOption;
                //var image = _context.productImgs.Where(x => x.ProductId == request.Id);
                if (product.productImgs.Count <= 0)
                {
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
                        var productImgs2 = new ProductImage()
                        {
                            Caption = "Thumbnail image",
                            DateCreated = DateTime.Now,
                            FileSize = request.productImg2.Length,
                            ImagePath = await this.SaveFile(request.productImg2),
                            IsDefault = true,
                            SortOrder = 2
                        };
                        listImg.Add(productImgs2);
                    }
                    if (request.productImg3 != null)
                    {
                        var productImgs3 = new ProductImage()
                        {
                            Caption = "Thumbnail image",
                            DateCreated = DateTime.Now,
                            FileSize = request.productImg3.Length,
                            ImagePath = await this.SaveFile(request.productImg3),
                            IsDefault = true,
                            SortOrder = 3
                        };
                        listImg.Add(productImgs3);
                    }
                    if (request.productImg4 != null)
                    {
                        var productImgs4 = new ProductImage()
                        {
                            Caption = "Thumbnail image",
                            DateCreated = DateTime.Now,
                            FileSize = request.productImg4.Length,
                            ImagePath = await this.SaveFile(request.productImg4),
                            IsDefault = true,
                            SortOrder = 4
                        };
                        listImg.Add(productImgs4);
                    }
                    product.productImgs = listImg;
                }
                else
                {
                    if (request.productImg1 != null)
                    {
                        var item = product.productImgs.Where(x => x.SortOrder == 1).FirstOrDefault();
                        if (item != null)
                        {
                            product.productImgs.Remove(item);
                            await _storageService.DeleteFileAsync("product", item.ImagePath);
                        }
                        var productImgs1 = new ProductImage()
                        {
                            Caption = "Thumbnail image",
                            DateCreated = DateTime.Now,
                            FileSize = request.productImg1.Length,
                            ImagePath = await this.SaveFile(request.productImg1),
                            IsDefault = true,
                            SortOrder = 1
                        };
                        product.productImgs.Add(productImgs1);
                    }
                    if (request.productImg2 != null && product.productImgs != null)
                    {
                        var item = product.productImgs.Where(x => x.SortOrder == 2).FirstOrDefault();
                        if (item != null)
                        {
                            product.productImgs.Remove(item);
                            await _storageService.DeleteFileAsync("product", item.ImagePath);

                        }
                        var productImgs2 = new ProductImage()
                        {
                            Caption = "Thumbnail image",
                            DateCreated = DateTime.Now,
                            FileSize = request.productImg2.Length,
                            ImagePath = await this.SaveFile(request.productImg2),
                            IsDefault = true,
                            SortOrder = 2
                        };
                        product.productImgs.Add(productImgs2);
                    }
                    if (request.productImg3 != null && product.productImgs != null)
                    {
                        var item = product.productImgs.Where(x => x.SortOrder == 3).FirstOrDefault();
                        if (item != null)
                        {
                            product.productImgs.Remove(item);
                            await _storageService.DeleteFileAsync("product", item.ImagePath);
                        }
                        var productImgs3 = new ProductImage()
                        {
                            Caption = "Thumbnail image",
                            DateCreated = DateTime.Now,
                            FileSize = request.productImg3.Length,
                            ImagePath = await this.SaveFile(request.productImg3),
                            IsDefault = true,
                            SortOrder = 3
                        };
                        product.productImgs.Add(productImgs3);
                    }
                    if (request.productImg4 != null && product.productImgs != null)
                    {

                        var item = product.productImgs.Where(x => x.SortOrder == 4).FirstOrDefault();
                        if (item != null)
                        {
                            product.productImgs.Remove(item);
                            await _storageService.DeleteFileAsync("product", item.ImagePath);
                        }
                        var productImgs4 = new ProductImage()
                        {
                            Caption = "Thumbnail image",
                            DateCreated = DateTime.Now,
                            FileSize = request.productImg4.Length,
                            ImagePath = await this.SaveFile(request.productImg4),
                            IsDefault = true,
                            SortOrder = 4
                        };
                        product.productImgs.Add(productImgs4);
                    }
                }
                _context.Update(product);
                var kq = await _context.SaveChangesAsync();
                if (kq > 0)
                {
                    return new ApiSuccessResult<bool>();
                }
                else
                {
                    return new ApiErrorResult<bool>("Thay đổi thông tin không thành công");
                }
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống");
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

        public async Task<ApiResult<PageResult<ProductViewModel>>> PublicGetAll(GetPublicProductRequest request)
        {
            var query = from product in _context.products
                        join c in _context.Categories on product.CategoryId equals c.Id
                        join p in _context.brands on product.BrandId equals p.id
                        where product.isActived == true
                        select new { product, c, p };
            var test = _context.products.ToList();
            if (request.BrandId != null && request.BrandId != 0)
            {
                query = query.Where(x => x.p.id == request.BrandId);
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => x.c.Id == request.CategoryId);
            }
            if (!string.IsNullOrEmpty(request.Keywword))
            {
                query = query.Where(x => x.product.ProductName.Contains(request.Keywword));
            }
            
            var data = await query.Select(x => new ProductViewModel()
            {
                Id = x.product.Id,
                ProductName = x.product.ProductName,
                ProductDescription = x.product.ProductDescription,
                Options = x.product.productOptions.Select(x => new ProductOptionViewModel()
                {
                    id = x.Id,
                    ColorOption = x.ColorOption,
                    SizeOption = x.SizeOption,
                    OptionPrice = x.OptionPrice,
                    Quantity = x.Quantity
                }).ToList(),
                CategoryName = x.product.category.CategoryName,
                Discount = x.product.Discount,
                CategoryId = x.product.CategoryId,
                BrandId = x.product.BrandId,
                BrandName = x.product.brand.BrandName,
                isActived = x.product.isActived,
                Quantity = x.product.productOptions.Select(x => x.Quantity).Sum(),
                BeginDateDiscount = x.product.BeginDateDiscount,
                ExpiredDateDiscount = x.product.ExpiredDateDiscount,
                ProductImg = x.product.productImgs
            }).ToListAsync();
          
            if (request.Price != null && request.Price != 0)
            {
                switch (request.Price)
                {
                    case 1:
                        data = data.Where(x => Convert.ToDouble(x.Options.OrderBy(x => x.OptionPrice).Select(x => x.OptionPrice).FirstOrDefault()) < 2000000).ToList();
                        break;
                    case 2:
                        data = data.Where(x => Convert.ToDouble(x.Options.OrderBy(x => x.OptionPrice).Select(x => x.OptionPrice).FirstOrDefault()) > 2000000&&
                        Convert.ToDouble(x.Options.OrderBy(x => x.OptionPrice).Select(x => x.OptionPrice).FirstOrDefault()) < 5000000
                        ).ToList();
                        break;
                    case 3:
                        data = data.Where(x => Convert.ToDouble(x.Options.OrderBy(x => x.OptionPrice).Select(x => x.OptionPrice).FirstOrDefault()) > 5000000 &&
                        Convert.ToDouble(x.Options.OrderBy(x => x.OptionPrice).Select(x => x.OptionPrice).FirstOrDefault()) < 10000000
                        ).ToList();
                        break;
                    case 4:
                        data = data.Where(x => Convert.ToDouble(x.Options.OrderBy(x => x.OptionPrice).Select(x => x.OptionPrice).FirstOrDefault()) > 10000000 ).ToList();
                        break;
                    default:
                        break;
                }
            }
            if(request.SortBy != null && request.SortBy != 0)
            {
                switch (request.SortBy)
                {
                    case 1:
                        data = data.OrderBy(x => Convert.ToDouble(x.Options.OrderBy(x => x.OptionPrice).Select(x => x.OptionPrice).FirstOrDefault())).ToList();
                        break;
                    case 2:
                        data = data.OrderByDescending(x => Convert.ToDouble(x.Options.OrderBy(x => x.OptionPrice).Select(x => x.OptionPrice).FirstOrDefault())).ToList();
                        break;
                    default:
                        break;
                }
            }
            int totalRow = data.Count;
             data = data.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
            var pageResult = new PageResult<ProductViewModel>
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return new ApiSuccessResult<PageResult<ProductViewModel>>(pageResult);
        }
    }
}
