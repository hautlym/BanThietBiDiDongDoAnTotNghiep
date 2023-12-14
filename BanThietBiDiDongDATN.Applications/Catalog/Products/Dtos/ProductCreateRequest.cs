using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Products.Dtos
{
    public class ProductCreateRequest
    {
        [Required(ErrorMessage= "Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; }
      
        [Required(ErrorMessage = "Vui lòng nhập Mô tả")]
        public string ProductDescription { get; set; }
        public int BrandId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giảm giá")]
        [Range(0, 99, ErrorMessage = "Giá trị lớn hơn hoặc bằng 0 và nhỏ hơn 100.")]
        public double Discount { get; set; }
        public bool isActived { get; set; }
        public DateTime? BeginDateDiscount { get; set; }
        public DateTime? ExpiredDateDiscount { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? productImg1 { get; set; }
        public IFormFile? productImg2 { get; set; }
        public IFormFile? productImg3 { get; set; }
        public IFormFile? productImg4 { get; set; }
        [Required(ErrorMessage= "Vui lòng điền option")]
        public List<ProductOptionViewModel> Options { get; set; }
    }
}
