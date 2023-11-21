using BanThietBiDiDongDATN.Application.Catalog.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATNApplication.Catalog.Orders.Dtos
{
    public class GetOrderRequest: PagingRequestBase
    {
        public string? Keyword { get; set; }
    }
}
