using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Products.Dtos
{
    public class ProductOptionViewModel

    {
        public int? id { get; set; }
        [Required(ErrorMessage = "Vui lòng điền màu")]
        public string ColorOption { get; set; }
        [Required(ErrorMessage = "Vui lòng điền dung lượng")]
        public string SizeOption { get; set; }
        [Required(ErrorMessage = "Vui lòng điền giá")]
        public string OptionPrice { get; set; }
        [Required(ErrorMessage = "Vui lòng điền số lượng")]
        public int Quantity { get; set; }
    }
}
