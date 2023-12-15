using BanThietBiDiDongDATN.Application.Catalog.Orders.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using BanThietBiDiDongDATN.Data.Enums;

namespace BanThietBiDiDongDATN.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IManageOrder _manageOrder;

        public OrderController(IManageOrder manageOrder)
        {
            _manageOrder = manageOrder;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetOrderRequest request)
        {
            var cart = await _manageOrder.GetAlllPaging(request);
            return Ok(cart);
        }
        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var cart = await _manageOrder.getAllOrder();
            return Ok(cart);
        }
        [HttpGet("UserId/{UserId}")]
        public async Task<IActionResult> GetByUserId(Guid UserId)
        {
            var cart = await _manageOrder.GetAllCartByUserId(UserId);
            if (cart.ResultObj.Count == 0)
            {
                return NotFound(cart);
            }
            return Ok(cart);
        }

        [HttpGet("{OrderId}")]
        public async Task<IActionResult> GetbyId(int OrderId)
        {
            var category = await _manageOrder.GetById(OrderId);
            if (!category.IsSuccessed)
            {
                return NotFound(category);
            }
            else
            {
                return Ok(category);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, int typeCreate)
        {
            var result = await _manageOrder.Create(request, typeCreate);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete([FromQuery] int CartID)
        {
            var Result = await _manageOrder.Delete(CartID);
            if (!Result.IsSuccessed)
                return BadRequest(Result);

            return Ok(Result);
        }
        [HttpDelete("Cart/{UserId}")]
        public async Task<IActionResult> DeleteCart(Guid UserId)
        {
            var Result = await _manageOrder.DeleteCart(UserId);
            if (!Result.IsSuccessed)
                return BadRequest(Result);

            return Ok(Result);
        }
        [HttpDelete("OrderDetail/{OrderId}")]
        public async Task<IActionResult> DeleteOrderDetail(int OrderId)
        {
            var Result = await _manageOrder.DeleteOrderTail(OrderId);
            if (!Result.IsSuccessed)
                return BadRequest(Result);

            return Ok(Result);
        }
        [HttpGet("changeStatus")]
        public async Task<IActionResult> ChangeStatus(int OrderId, OrderStatus status)
        {

            var Result = await _manageOrder.ChangeStatus(OrderId,status);
            if (!Result.IsSuccessed)
                return BadRequest(Result);
            return Ok(Result);
        }
    }
}
