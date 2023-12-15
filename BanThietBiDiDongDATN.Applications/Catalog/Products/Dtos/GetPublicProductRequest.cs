using BanThietBiDiDongDATN.Application.Catalog.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Products.Dtos
{
    public class GetPublicProductRequest : PagingRequestBase
    {
        public string? Keywword { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? Price { get; set; }
        public int? SortBy { get; set; }
    }
}
