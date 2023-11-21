using BanThietBiDiDongDATN.Application.Catalog.Categories.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.EF;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Categories
{
    public class ManageCategory : IManageCategory
    {
        private readonly BanThietBiDiDongDATNDbContext _context;
        public ManageCategory(BanThietBiDiDongDATNDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> Create(CreateCategoryRequest request)
        {
            try
            {
                var checkExist =await _context.Categories.Where(x => x.CategoryName==request.CategoryName).FirstOrDefaultAsync();
                if (checkExist != null)
                {
                    return new ApiErrorResult<bool>("Danh mục này đã tồn tại");
                }
                var category = new Category()
                {
                    CategoryName = request.CategoryName,
                    Description = request.Description
                };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>("Thêm danh mục không thành công");
            }
        }

        public async Task<ApiResult<bool>> Delete(int CategoryId)
        {
            var categories = await _context.Categories.Where(x => x.Id == CategoryId).FirstOrDefaultAsync();
            if (categories == null)
                new ApiErrorResult<bool>("Danh mục không tồn tại");
            _context.Categories.Remove(categories);
            var kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Xóa danh mục không thành công");
            }
        }
        public async Task<ApiResult<List<CategoryViewModels>>> GetAllCategory()
        {
            var data = await _context.Categories.Select(x => new CategoryViewModels()
            {
                Id = x.Id,
                CategoryName = x.CategoryName,
                Description = x.Description
            }).ToListAsync();
            return new ApiSuccessResult<List<CategoryViewModels>>(data);
        }

        public async Task<ApiResult<PageResult<CategoryViewModels>>> GetAlllPaging(GetCategoryRequest request)
        {
            var query = from c in _context.Categories
                        select c;
            if (!string.IsNullOrEmpty(request.keyword))
            {
                query = query.Where(x => x.CategoryName.Contains(request.keyword));
            }
            var totalRow = query.Count();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new CategoryViewModels()
            {
                Id = x.Id,
                CategoryName = x.CategoryName,
                Description = x.Description
            }).ToListAsync();
            var pageResult = new PageResult<CategoryViewModels>
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return new ApiSuccessResult<PageResult<CategoryViewModels>>(pageResult); 
        }

        public async Task<ApiResult<CategoryViewModels>> GetById(int categoryId)
        {
            var categories = await _context.Categories.Where(x => x.Id == categoryId).FirstOrDefaultAsync();

            if (categories != null)
            {
                var categoryViewModels = new CategoryViewModels()
                {
                    Id = categories.Id,
                    CategoryName = categories.CategoryName,
                    Description = categories.Description
                };
                return new ApiSuccessResult<CategoryViewModels>(categoryViewModels);
            }
            else
            {
                return new ApiErrorResult<CategoryViewModels>("Không tìm thấy danh mục");
            }
        }

        public async Task<ApiResult<bool>> Update(UpdateCategoryRequest request)
        {
            var category = await _context.Categories.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            if (category == null)
                return new ApiErrorResult<bool>("Danh mục không tồn tại");
            category.CategoryName = request.CategoryName;
            category.Description = request.Description;
            _context.Categories.Update(category);
            var kq= await _context.SaveChangesAsync();
            if(kq>0)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Sửa danh mục không thành công");
            }
        }
    }
}
