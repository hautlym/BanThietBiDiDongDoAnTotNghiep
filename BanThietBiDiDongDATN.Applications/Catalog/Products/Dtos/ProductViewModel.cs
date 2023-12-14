using BanThietBiDiDongDATN.Data.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Products.Dtos
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public int BrandId { get; set; }
        public double Discount { get; set; }
        public bool isActived { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
  
        public DateTime? BeginDateDiscount { get; set; }
        public DateTime? ExpiredDateDiscount { get; set; }
        public List<ProductImage> ProductImg { get; set; }
        public List<ProductOptionViewModel> Options { get; set; }
    }
}
