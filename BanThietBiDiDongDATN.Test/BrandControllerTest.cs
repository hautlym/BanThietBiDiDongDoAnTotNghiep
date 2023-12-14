using BanThietBiDiDongDATN.Application.Catalog.Brands.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.BackendAPI.Controllers;
using BanThietBiDiDongDATN.Data.entities;
using BanThietBiDiDongDoAn.Applications.Brands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Test
{
    public class BrandControllerTest
    {
        private readonly Mock<IManageBrand> mockBrand;
        public BrandControllerTest()
        {
                mockBrand = new Mock<IManageBrand>();
        }
        [Fact]
        public void BrandController_ShouldCreate_NotNull()
        {
            var brandController = new BrandController(mockBrand.Object);
            Assert.NotNull(brandController);
        }
        [Fact]
        public async Task CreateBrand_ValidInput_Success()
        {
            mockBrand.Setup(x => x.Create(It.IsAny<CreateBrandRequest>())).ReturnsAsync(new ApiSuccessResult<bool>());
            var brandController = new BrandController(mockBrand.Object);
            var result =await brandController.Create(new CreateBrandRequest() { BrandName = "test", description = "test"});
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task CreateBrand_ValidInput_Failed()
        {
            mockBrand.Setup(x => x.Create(It.IsAny<CreateBrandRequest>())).ReturnsAsync(new ApiErrorResult<bool>());
            var brandController = new BrandController(mockBrand.Object);
            var result =await brandController.Create(new CreateBrandRequest() { BrandName = "test", description = "test" });
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task getBrand_ValidInput_Success()
        {
            mockBrand.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new ApiSuccessResult<Brand>());
            var brandController = new BrandController(mockBrand.Object);
            var result = await brandController.GetbyId(12);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task getBrand_ValidInput_Failed()
        {
            mockBrand.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new ApiErrorResult<Brand>());
            var brandController = new BrandController(mockBrand.Object);
            var result = await brandController.GetbyId(12);
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
