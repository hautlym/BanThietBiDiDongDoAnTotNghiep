using BanThietBiDiDongDATN.Data.entities;
using BanThietBiDiDongDATN.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Orders.Dtos
{
    public class UpdateOrderRequest
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipNumberPhone { get; set; }
        public string ShipDescription { get; set; }
        public Guid AppUserId { get; set; }
        public OrderStatus status { get; set; } 
        public int? voucherId { get; set; }
        public TypeCheckOut typePayment { get; set; }
    }
}
