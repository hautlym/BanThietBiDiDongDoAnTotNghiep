using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanThietBiDiDongDATN.Application.Catalog.Vouchers.Dtos;

namespace BanThietBiDiDongDATN.Application.Catalog.Vouchers
{
    public interface IManageVoucher
    {
        Task<ApiResult<List<VoucherViewModel>>> GetAll();
        Task<ApiResult<PageResult<VoucherViewModel>>> GetAlllPaging(GetVoucherRequest request);
        Task<ApiResult<bool>> Create(CreateVoucherRequest request);
        Task<ApiResult<bool>> Update(UpdateVoucherRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<VoucherViewModel>> GetById(int id);
    }
}
