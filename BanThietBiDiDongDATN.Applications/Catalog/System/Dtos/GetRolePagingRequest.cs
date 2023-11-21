using BanThietBiDiDongDATN.Application.Catalog.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.System.Dtos
{
    public class GetRolePagingRequest: PagingRequestBase
    {
        public string? Keyword { get; set; }
    }
}
