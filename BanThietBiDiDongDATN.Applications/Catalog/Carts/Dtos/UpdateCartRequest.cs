using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Carts.Dtos
{
    public class UpdateCartRequest
    {
        public int id { get; set; }
        public int Quantity { get; set; }
    }
}
