using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Categories.Dtos
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage= "Vui lòng điền tên danh mục")] 
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền mô tả")]
        public string Description { get; set; }

    }
}
