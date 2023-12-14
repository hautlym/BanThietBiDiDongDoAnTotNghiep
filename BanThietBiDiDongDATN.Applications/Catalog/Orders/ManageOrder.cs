using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.Orders.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.EF;
using BanThietBiDiDongDATN.Data.entities;
using BanThietBiDiDongDATN.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Orders
{
    public class ManageOrder : IManageOrder
    {
        private readonly BanThietBiDiDongDATNDbContext _context;
        public ManageOrder(BanThietBiDiDongDATNDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> Create(CreateOrderRequest request, int typeCreate = 1)
        {
            try
            {
                var ListCart = _context.appUsers.Where(x => x.Id == request.AppUserId).Select(x => x.Carts).FirstOrDefault();
                var ListOrder = new List<OrderDetail>();
                Voucher voucher = new Voucher();
                var flag = false;
                double discount = 0;
                if (request.voucherId != null)
                {
                    voucher = _context.vouchers.Where(x => x.VoucherCode == request.voucherId).FirstOrDefault();

                    if (voucher != null)
                    {
                        if (voucher.BeginDate > DateTime.Now || voucher.ExpiredDate < DateTime.Now || voucher.Quantity <= 0)
                        {
                            return new ApiErrorResult<bool>("Voucher đã hết hạn");
                        }
                        else
                        {
                            flag = true;
                            discount = voucher.Discount;
                        }
                    }
                    else
                    {
                        return new ApiErrorResult<bool>("Voucher không chính xác");
                    }
                }
                if (typeCreate == 0)
                {
                    foreach (var cart in ListCart)
                    {
                        if (!checkWareHouse(cart.OptionId, cart.Quantity))
                        {
                            return new ApiErrorResult<bool>($"Lượng hàng {cart.Product.ProductName} trong kho không đủ");
                        }
                        var product = _context.productOptions.Where(x => x.Id == cart.OptionId).FirstOrDefault();
                        product.Quantity = product.Quantity - cart.Quantity;
                        _context.productOptions.Update(product);
                        var price = Convert.ToDouble(product.OptionPrice) * cart.Quantity;
                        var OrderDetail = new OrderDetail()
                        {
                            ProductId = cart.ProductId,
                            Quantity = cart.Quantity,
                            OptionId = cart.OptionId,
                            Price = price - price * (discount / 100),
                        };
                        ListOrder.Add(OrderDetail);
                    }
                }
                if (typeCreate == 1)
                {
                    foreach (var cart in request.carts)
                    {
                        if (!checkWareHouse(cart.OptionId, cart.Quantity))
                        {
                            return new ApiErrorResult<bool>($"Lượng hàng {cart.ProductId} trong kho không đủ");
                        }
                        var product = _context.productOptions.Where(x => x.Id == cart.OptionId).FirstOrDefault();
                        product.Quantity = product.Quantity - cart.Quantity;
                        _context.productOptions.Update(product);
                        var price = Convert.ToDouble(product.OptionPrice) * cart.Quantity;
                        var OrderDetail = new OrderDetail()
                        {
                            ProductId = cart.ProductId,
                            Quantity = cart.Quantity,
                            Price = price - price * (discount / 100),
                            OptionId = cart.OptionId,
                        };
                        ListOrder.Add(OrderDetail);
                    }
                }
                var Orders = new Order()
                {
                    ShipName = request.ShipName,
                    ShipAddress = request.ShipAddress,
                    ShipDescription = request.ShipDescription,
                    ShipEmail = request.ShipEmail,
                    ShipNumberPhone = request.ShipNumberPhone,
                    typePayment = request.typePayment,
                    status = request.status,
                    OrderDate = DateTime.Now,
                    OrderDetails = ListOrder,
                    AppUserId = request.AppUserId,
                    typeOrder = request.typeOrder,
                };
                if (request.voucherId != null)
                {
                    if (flag)
                    {
                        Orders.voucherId = voucher.Id;
                        voucher.Quantity = voucher.Quantity - 1;
                        var voucher1 = _context.vouchers.Update(voucher);
                    }
                }
                var kq = await _context.Orders.AddAsync(Orders);
                var order = _context.Orders.ToList();

                var kq2 = await _context.SaveChangesAsync();
                if (kq2 > 0)
                {
                    return new ApiSuccessResult<bool>();
                }
                return new ApiErrorResult<bool>("Tạo đơn hàng không thành công");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>("Tạo đơn hàng không thành công");
            }
        }
        public bool checkWareHouse(int Optionid, int Quantity)
        {
            var option = _context.productOptions.Where(x => x.Id == Optionid).FirstOrDefault();
            if (option.Quantity < Quantity)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<ApiResult<bool>> Delete(int OrderId)
        {
            var order = _context.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
            if (order == null)
                return new ApiErrorResult<bool>("Không tìm thấy đơn hàng");
            _context.Orders.Remove(order);
            var kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Xóa không thành công");
        }

        public async Task<ApiResult<bool>> DeleteCart(Guid UserId)
        {
            foreach (var item in _context.carts)
            {
                if (item.AppUserId == UserId)
                {
                    _context.carts.Remove(item);
                }
            }
            var kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Xóa giỏ hàng không thành công");
            }
        }

        public Task<ApiResult<bool>> DeleteOrderTail(int OrderId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<List<OrderViewModel>>> GetAllCartByUserId(Guid UserId)
        {
            var order = _context.Orders;
            var data = await order.Where(x => x.AppUserId == UserId).Select(x => new OrderViewModel()
            {
                Id = x.Id,
                ShipName = x.ShipName,
                ShipAddress = x.ShipAddress,
                ShipDescription = x.ShipDescription,
                ShipEmail = x.ShipEmail,
                ShipNumberPhone = x.ShipNumberPhone,
                OrderDate = DateTime.Now,
                OrderDetails = x.OrderDetails
            }).ToListAsync();
            return new ApiSuccessResult<List<OrderViewModel>>(data);
        }

        public async Task<ApiResult<PageResult<OrderViewModel>>> GetAlllPaging(GetOrderRequest request)
        {
            var query = from c in _context.Orders
                        select c;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.ShipName.Contains(request.Keyword) || x.ShipNumberPhone.Contains(request.Keyword));
            }
            var totalRow = query.Count();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new OrderViewModel()
            {
                Id = x.Id,
                ShipName = x.ShipName,
                ShipNumberPhone = x.ShipNumberPhone,
                ShipAddress = x.ShipAddress,
                ShipDescription = x.ShipDescription,
                ShipEmail = x.ShipEmail,
                OrderDate = x.OrderDate,
                OrderDetails = x.OrderDetails,
                typePayment = x.typePayment,
                status = x.status,
            }).ToListAsync();
            var pageResult = new PageResult<OrderViewModel>
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return new ApiSuccessResult<PageResult<OrderViewModel>>(pageResult);
        }
        public Task<ApiResult<List<OrderViewModel>>> getAllOrder()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<OrderViewModel>> GetById(int OrderId)
        {
            try
            {
                
                var data = await _context.Orders.Where(x => x.Id == OrderId).Select(x => new OrderViewModel()
                {
                    Id = x.Id,
                    ShipName = x.ShipName,
                    ShipNumberPhone = x.ShipNumberPhone,
                    ShipAddress = x.ShipAddress,
                    ShipDescription = x.ShipDescription,
                    ShipEmail = x.ShipEmail,
                    OrderDate = x.OrderDate,
                    OrderDetails = x.OrderDetails,
                    typePayment = x.typePayment,
                    status = x.status,
                }).FirstOrDefaultAsync();
                if (data != null)
                {
                    return new ApiSuccessResult<OrderViewModel>(data);
                }
                else
                {
                    return new ApiErrorResult<OrderViewModel>("Không tìm thấy đơn hàng");
                }
            }
            catch(Exception ex)
            {
                return new ApiErrorResult<OrderViewModel>("Không tìm thấy đơn hàng");
            }
            
        }

        public async Task<ApiResult<bool>> ChangeStatus(int OrderId, OrderStatus status)
        {
            var order = await _context.Orders.Where(x => x.Id == OrderId).FirstOrDefaultAsync();
            if (order == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đơn hàng");
            }
            if (order.status == OrderStatus.Complete)
            {
                return new ApiErrorResult<bool>("Đơn hàng đã hoàn thành");
            }
            order.status = status;
            if(status == OrderStatus.Cancel) {
                var Detail = _context.OrderDetails.Where(x=>x.OrderId== order.Id).ToList();
                foreach (var item in Detail)
                {
                    var option = _context.productOptions.Where(x=>x.Id==item.OptionId).FirstOrDefault();
                    if(option != null)
                    {
                        option.Quantity = option.Quantity+item.Quantity;
                        _context.productOptions.Update(option);
                    }
                }
            }
            _context.Orders.Update(order);
            var kq = await _context.SaveChangesAsync();
            if (kq > 0)
            {
                return new ApiSuccessResult<bool>(); ;
            }
            else
            {
                return new ApiErrorResult<bool>("Thay đổi trang thái không thành công");
            }
        }
    }
}
