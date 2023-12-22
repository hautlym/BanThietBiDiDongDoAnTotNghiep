using BanThietBiDiDongDATN.Application.Catalog.Products;
using BanThietBiDiDongDoAn.Applications.Brands;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Test
{
    public class ProductControllerTest
    {
        private readonly Mock<IManageProduct> mockProduct;
        public ProductControllerTest()
        {
            mockProduct = new Mock<IManageProduct>();
        }
    }
}
