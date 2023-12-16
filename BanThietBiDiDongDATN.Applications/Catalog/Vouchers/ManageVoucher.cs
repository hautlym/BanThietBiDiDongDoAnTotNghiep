using BanThietBiDiDongDATN.Application.Catalog.Categories;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Vouchers.Dtos;
using BanThietBiDiDongDATN.Data.EF;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Vouchers
{
    public class ManageVoucher : IManageVoucher
    {
        private readonly BanThietBiDiDongDATNDbContext _context;
        public ManageVoucher(BanThietBiDiDongDATNDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> Create(CreateVoucherRequest request)
        {

            try
            {
                var checkexist = await _context.vouchers.Where(x => x.VoucherName == request.VoucherName).FirstOrDefaultAsync();
                if (checkexist != null)
                {
                    return new ApiErrorResult<bool>("Voucher này đã tồn tại");
                }
                var checkCodeExist = await _context.vouchers.Where(x => x.VoucherCode == request.VoucherCode).FirstOrDefaultAsync();
                if (checkCodeExist != null)
                {
                    return new ApiErrorResult<bool>("Code này đã tồn tại");
                }
                var category = new Voucher()
                {
                    VoucherCode = request.VoucherCode,
                    VoucherName = request.VoucherName,
                    BeginDate = request.BeginDate,
                    Discount=request.Discount,
                    ExpiredDate = request.ExpiredDate,
                    CreateDate=DateTime.Now,
                    Quantity = request.Quantity,
                };
                _context.vouchers.Add(category);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>("thêm voucher không thành công");
            }
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var item = await _context.vouchers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (item == null)
                new ApiErrorResult<bool>("Voucher không tồn tại");
            _context.vouchers.Remove(item);
            var kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Xóa Voucher không thành công");
            }
        }

        public async Task<ApiResult<List<VoucherViewModel>>> GetAll()
        {
            var data = await _context.vouchers.Select(x => new VoucherViewModel()
            {
                Id=x.Id,
                VoucherCode=x.VoucherCode,
                VoucherName =x.VoucherName,
                BeginDate=x.BeginDate,
                ExpiredDate=x.ExpiredDate,
                Discount=x.Discount,
                Quantity=x.Quantity,
            }).ToListAsync();
            return new ApiSuccessResult<List<VoucherViewModel>>(data);
        }

        public async Task<ApiResult<PageResult<VoucherViewModel>>> GetAlllPaging(GetVoucherRequest request)
        {
            var query = from c in _context.vouchers
                        select c;
            if (!string.IsNullOrEmpty(request.keyword))
            {
                query = query.Where(x => x.VoucherName.Contains(request.keyword)|| x.VoucherCode.Contains(request.keyword));
            }
            var totalRow = query.Count();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new VoucherViewModel()
            {
                Id = x.Id,
                VoucherCode = x.VoucherCode,
                VoucherName = x.VoucherName,
                BeginDate = x.BeginDate,
                ExpiredDate = x.ExpiredDate,
                Discount = x.Discount,
                Quantity = x.Quantity,

            }).ToListAsync();
            var pageResult = new PageResult<VoucherViewModel>
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return new ApiSuccessResult<PageResult<VoucherViewModel>>(pageResult);
        }

        public async Task<ApiResult<VoucherViewModel>> GetById(int id)
        {
            var item = await _context.vouchers.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (item != null)
            {
                var categoryViewModels = new VoucherViewModel()
                {
                    Id = item.Id,
                    VoucherName = item.VoucherName,
                    VoucherCode = item.VoucherCode,
                    BeginDate= item.BeginDate,
                    ExpiredDate = item.ExpiredDate,
                    Quantity= item.Quantity,
                    Discount= item.Discount,
                };
                return new ApiSuccessResult<VoucherViewModel>(categoryViewModels);
            }
            else
            {
                return new ApiErrorResult<VoucherViewModel>("Không tìm thấy voucher");
            }
        }

        public async Task<ApiResult<bool>> Update(UpdateVoucherRequest request)
        {
            var item = await _context.vouchers.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            if (item == null)
                return new ApiErrorResult<bool>("voucher không tồn tại");
            item.VoucherName = request.VoucherName;
            item.VoucherCode = request.VoucherCode;
            item.BeginDate = request.BeginDate;
            item.ExpiredDate = request.ExpiredDate;
            item.Quantity = request.Quantity;
            item.Discount = request.Discount;
            item.LastModifiedDate = DateTime.Now;
            _context.vouchers.Update(item);
            var kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Sửa voucher không thành công");
            }
        }
    }
}
