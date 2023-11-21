using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Brands.Dtos
{
    public class CreateBrandRequest
    {
        [Required(ErrorMessage= "Vui lòng điền tên thương hiệu")]
        public string BrandName { get; set; }
        public IFormFile? logo { get; set; }
        [Required(ErrorMessage = "Vui lòng điền tên mô tả")]
        public string description { get; set; }
        public Guid? UserId { get; set; }
    }
}
