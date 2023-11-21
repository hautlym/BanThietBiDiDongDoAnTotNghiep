using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Products.Dtos
{
    public class ProductUpdateRequest
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public int BrandId { get; set; }
        public double Discount { get; set; }
        public bool isActived { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public string ImgUrl1 { get; set; }
        public string ImgUrl2 { get; set; }
        public string ImgUrl3 { get; set; }
        public string ImgUrl4 { get; set; }
        public IFormFile? productImg1 { get; set; }
        public IFormFile? productImg2 { get; set; }
        public IFormFile? productImg3 { get; set; }
        public IFormFile? productImg4 { get; set; }
    }
}
