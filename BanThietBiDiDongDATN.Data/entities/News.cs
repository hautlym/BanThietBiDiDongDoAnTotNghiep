using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class News
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid Author { get; set; }
        public string Content { get; set; }
        public DateTime DatePost { get; set; }

    }
}
