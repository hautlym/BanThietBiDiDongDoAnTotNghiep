using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.Orders.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Orders
{
    public interface IManageOrder
    {
        public Task<ApiResult<List<OrderViewModel>>> getAllOrder();
        public Task<ApiResult<List<OrderViewModel>>> GetAllCartByUserId(Guid UserId);
        public Task<ApiResult<PageResult<OrderViewModel>>> GetAlllPaging(GetOrderRequest request);
        public Task<ApiResult<bool>> Create(CreateOrderRequest request, int typeCreate);
        public Task<ApiResult<bool>> Delete(int OrderId);
        public Task<ApiResult<bool>> ChangeStatus(int OrderId,OrderStatus status);
        public Task<ApiResult<bool>> DeleteOrderTail(int OrderId);
        public Task<ApiResult<bool>> DeleteCart(Guid UserId);
        public Task<ApiResult<OrderViewModel>> GetById(int OrderId);
    }
}
