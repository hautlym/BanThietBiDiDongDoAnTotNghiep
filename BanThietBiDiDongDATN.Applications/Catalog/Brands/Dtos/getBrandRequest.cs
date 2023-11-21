using BanThietBiDiDongDATN.Application.Catalog.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Brands.Dtos
{
    public class getBrandRequest: PagingRequestBase
    {
        public string? keyword { get; set; }
    }
}
