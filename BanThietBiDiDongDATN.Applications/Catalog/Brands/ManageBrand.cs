using BanThietBiDiDongDATN.Application.Catalog.Brands.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Categories;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.EF;
using BanThietBiDiDongDATN.Data.entities;
using BanThietBiDiDongDoAn.Applications.Brands;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Brands
{
    public class ManageBrand : IManageBrand
    {
        private readonly BanThietBiDiDongDATNDbContext _context;
        private readonly IStorageService _storageService;
        public ManageBrand(BanThietBiDiDongDATNDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<ApiResult<bool>> Delete(int brandId)
        {
            var brand = await _context.brands.Where(x => x.id == brandId).FirstOrDefaultAsync();
            if (brand == null) throw new BanThietBiDiDongDATNException("Can not find brand");
            if (brand.logo != null)
            {
                 await _storageService.DeleteFileAsync("brand", brand.logo);
            }
            _context.brands.Remove(brand);
            var kq =  await _context.SaveChangesAsync();
            if(kq>0)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Xóa không thành công");
        }

        public async Task<ApiResult<List<Brand>>> GetAll()
        {
            var data = await _context.brands.Select(x => new Brand()
            {
                id = x.id,
                BrandName = x.BrandName,
                description = x.description,
                logo = x.logo,
                ModifyBy = x.ModifyBy,
                LastModifiedDate = x.LastModifiedDate,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
            }).ToListAsync();
            return new ApiSuccessResult<List<Brand>>();
        }

        public async Task<ApiResult<Brand>> GetById(int brandId)
        {
            var brand = await _context.brands.Where(x => x.id == brandId).FirstOrDefaultAsync();

            if (brand != null)
            {
                var brandItem = new Brand()
                {
                    id = brand.id,
                    BrandName = brand.BrandName,
                    description = brand.description,
                    logo = brand.logo,
                    ModifyBy = brand.ModifyBy,
                    LastModifiedDate = brand.LastModifiedDate,
                    CreateBy = brand.CreateBy,
                    CreateDate = brand.CreateDate,
                };
                return new ApiSuccessResult<Brand>(brandItem);
            }
            else
            {
                return new ApiErrorResult<Brand>("Không tìm thấy thương hiệu"); ;
            }
        }

        public async Task<ApiResult<bool>> Create(CreateBrandRequest request)
        {
            try
            {
                var checkExist = await _context.brands.Where(x => x.BrandName == request.BrandName).FirstOrDefaultAsync();
                if (checkExist != null)
                {
                    return new ApiErrorResult<bool>("Thương hiệu đã tồn tại");
                }
                var brand = new Brand()
                {
                    BrandName = request.BrandName,
                    description = request.description,
                    CreateDate = DateTime.Now,
                    CreateBy = request.UserId,
                };
                if (request.logo != null)
                {
                    brand.logo = await this.SaveFile("brand", request.logo);
                }
                _context.brands.Add(brand);
                var kq = await _context.SaveChangesAsync();
                if (kq > 0)
                {
                    return new ApiSuccessResult<bool>();
                }
                else
                {
                    return new ApiErrorResult<bool>("Thêm thương hiệu không thành công");
                }
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>("Thêm thương hiệu không thành công");
            }
            
        }
        public async Task<string> SaveFile(string folder,IFormFile file)
        {

            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(),folder ,fileName);
            return fileName;
        }
        public async Task<ApiResult<PageResult<Brand>>> GetAlllPaging(getBrandRequest request)
        {
            var query = from c in _context.brands
                        select c;
            if (!string.IsNullOrEmpty(request.keyword))
            {
                query = query.Where(x => x.BrandName.Contains(request.keyword));
            }
            var totalRow = query.Count();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new Brand()
            {
                id = x.id,
                BrandName = x.BrandName,
                description = x.description,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
                logo = x.logo,
                LastModifiedDate = x.LastModifiedDate,
                ModifyBy = x.ModifyBy,
            }).ToListAsync();
            var pageResult = new PageResult<Brand>
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return new ApiSuccessResult<PageResult<Brand>>(pageResult);
        }

        public async Task<ApiResult<bool>> Update(UpdateBrandRequest request)
        {
            var brand = await _context.brands.Where(x => x.id == request.id).FirstOrDefaultAsync();
            if (brand == null) 
                return new ApiErrorResult<bool>("thương hiệu không tồn tại");
            var checkExist =await _context.brands.Where(x=>x.BrandName == request.BrandName).ToListAsync();
            if(checkExist.Count>1)
            {
                return new ApiErrorResult<bool>("thương hiệu đã tồn tại");
            }
            brand.BrandName = request.BrandName;
            brand.description = request.description;
            if (request.logo != null)
            {
                brand.logo = await this.SaveFile("brand", request.logo);
            }
            brand.LastModifiedDate = DateTime.Now;
            brand.ModifyBy = request.Userid;
            _context.brands.Update(brand);
            var kq= await _context.SaveChangesAsync();
            if(kq>0)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Sửa thương hiệu không thành công");
            }
        }

        public async Task<ApiResult<List<Brand>>> GetAllBrand()
        {
            var data = await _context.brands.Select(x => new Brand()
            {
                id = x.id,
                BrandName = x.BrandName,
                description = x.description,
                logo = x.logo,
            }).ToListAsync();
            return new ApiSuccessResult<List<Brand>>(data);
        }
    }
}
