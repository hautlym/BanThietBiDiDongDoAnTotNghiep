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
    public class ProductCreateRequest
    {
        [Required(ErrorMessage= "Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá")]
        public double ProductPrice { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Mô tả")]
        public string ProductDescription { get; set; }
        public int BrandId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giảm giá")]
        public double Discount { get; set; }
        public bool isActived { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public int Quantity { get; set; }
        public IFormFile? productImg1 { get; set; }
        public IFormFile? productImg2 { get; set; }
        public IFormFile? productImg3 { get; set; }
        public IFormFile? productImg4 { get; set; }
    }
}
