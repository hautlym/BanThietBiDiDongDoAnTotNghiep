using BanThietBiDiDongDATN.Application.Catalog.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Categories
{
    public class GetCategoryRequest: PagingRequestBase
    {
        public string? keyword { get; set; }
    }
}
