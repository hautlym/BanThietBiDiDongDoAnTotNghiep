using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BanThietBiDiDongDATN.Application.Catalog.Brands.Dtos
{
    public class UpdateBrandRequest
    {
        public int id { get; set; }
        [Required(ErrorMessage= "Vui lòng điền tên thương hiệu")]
        public string BrandName { get; set; }
        public string? ImgLogo { get; set; }
        public IFormFile? logo { get; set; }
        [Required(ErrorMessage = "Vui lòng điền tên mô tả")]
        public string description { get; set; }
        public Guid? Userid { get; set; }
    }
}
