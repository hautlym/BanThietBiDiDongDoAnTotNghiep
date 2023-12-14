using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Products.Dtos
{
    public class ProductUpdateRequest
    {
        
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int BrandId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giảm giá")]
        [Range(0, 99, ErrorMessage = "Giá trị lớn hơn hoặc bằng 0 và nhỏ hơn 100.")]
        public double Discount { get; set; }
        public bool isActived { get; set; }
        public int CategoryId { get; set; }
        public string? ImgUrl1 { get; set; }
        public string? ImgUrl2 { get; set; }
        public string? ImgUrl3 { get; set; }
        public string? ImgUrl4 { get; set; }
        public DateTime? BeginDateDiscount { get; set; }
        public DateTime? ExpiredDateDiscount { get; set; }
        public IFormFile? productImg1 { get; set; }
        public IFormFile? productImg2 { get; set; }
        public IFormFile? productImg3 { get; set; }
        public IFormFile? productImg4 { get; set; }
        public List<ProductOptionViewModel> Options { get; set; }
    }
}
