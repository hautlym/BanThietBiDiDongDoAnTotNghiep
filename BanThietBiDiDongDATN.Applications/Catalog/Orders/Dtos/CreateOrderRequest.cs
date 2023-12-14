using BanThietBiDiDongDATN.Application.Catalog.Carts.Dtos;
using BanThietBiDiDongDATN.Data.entities;
using BanThietBiDiDongDATN.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Orders.Dtos
{
    public class CreateOrderRequest
    {
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipNumberPhone { get; set; }
        public string ShipDescription { get; set; }
        public string typeOrder { get; set; }
        public Guid AppUserId { get; set; }
        public OrderStatus status { get; set; } 
        public string? voucherId { get; set; }
        public TypeCheckOut typePayment { get; set; }
        public List<CreateCartRequest>? carts { get; set; }
    }
}
