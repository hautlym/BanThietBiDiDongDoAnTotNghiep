using BanThietBiDiDongDATN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class WareHouse:IDateTracking
    {
        public int Id { get; set; } 
        public int ProductOptionId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public ProductOption ProductOption { get; set; }
    }
}
