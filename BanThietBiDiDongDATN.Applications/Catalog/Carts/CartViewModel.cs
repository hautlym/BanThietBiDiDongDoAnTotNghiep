﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Carts
{
    public class CartViewModel
    {
        public int id { get; set; }
        public int ProductId { get; set; }

        public string ProductNane { get; set; }
        public int Quantity { get; set; }
        public string OptionColor { get; set; }
        public string OptionSize { get; set; }
        public double ProductPrice { get; set; }
        public double ProductOriginal { get; set; }
        public string UserName { get; set; }
        public double Discount { get; set; }
        public string? ImgUrl { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? totalPrice { get; set; }

        public int ProductQuantity { get; set; }
    }
}
