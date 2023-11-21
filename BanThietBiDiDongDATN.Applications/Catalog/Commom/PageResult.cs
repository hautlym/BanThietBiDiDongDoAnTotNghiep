using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Commom
{
    public class PageResult<T> : PageResultBase
    {
        public List<T> Items { set; get; }
    }
}
