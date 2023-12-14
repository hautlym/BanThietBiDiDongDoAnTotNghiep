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

namespace BanThietBiDiDongDATN.ApiIntegration.Service.OrderApiClient
{
    public interface IOrderService
    {
        Task<ApiResult<List<OrderViewModel>>> GetAll();
        Task<ApiResult<PageResult<OrderViewModel>>> GetAllPaging(GetOrderRequest request);
        Task<ApiResult<List<OrderViewModel>>> GetAllByUserId(Guid UserId);
        Task<ApiResult<bool>> CreateOrder(CreateOrderRequest request,int typeCreate);
        Task<ApiResult<OrderViewModel>> GetById(int id);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> DeleteCart(Guid OrderId);
        Task<ApiResult<bool>> DeleteOrderDetail(int OrderId);
        Task<ApiResult<bool>> ChangeStatus(int OrderId, OrderStatus status);
    }
}
