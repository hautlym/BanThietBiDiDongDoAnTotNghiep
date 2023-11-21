﻿using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.Products.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.ApiIntegration.Service.ProductApiClient
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<bool>> Create(ProductCreateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var requestContent = new MultipartFormDataContent();

            if (request.productImg1 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.productImg1.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.productImg1.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "productImg1", request.productImg1.FileName);
            }
            if (request.productImg1 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.productImg1.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.productImg1.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "productImg1", request.productImg1.FileName);
            }
            if (request.productImg2 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.productImg2.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.productImg2.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "productImg2", request.productImg2.FileName);
            }
            if (request.productImg3 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.productImg3.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.productImg3.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "productImg3", request.productImg3.FileName);
            }
            if (request.productImg4 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.productImg4.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.productImg4.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "productImg4", request.productImg4.FileName);
            }

            requestContent.Add(new StringContent(request.ProductName.ToString()), "ProductName");
            requestContent.Add(new StringContent(request.ProductPrice.ToString()), "ProductPrice");
            requestContent.Add(new StringContent(request.Quantity.ToString()), "Quantity");
            requestContent.Add(new StringContent(request.ProductDescription.ToString()), "ProductDescription");
            requestContent.Add(new StringContent(request.isActived.ToString()), "isActived");
            requestContent.Add(new StringContent(request.Discount.ToString()), "Discount");
            requestContent.Add(new StringContent(request.BrandId.ToString()), "BrandId");
            requestContent.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");
            var response = await client.PostAsync($"/api/Product", requestContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> Delete(int productId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.DeleteAsync($"/api/Product/{productId}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }

        public Task<ApiResult<List<ProductViewModel>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<ProductViewModel>>> GetAllPaging(GetProductPagingRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync($"/api/Product/paging?keyword={request.keyword}&PageIndex={request.PageIndex}&PageSize={request.PageSize}&CategoryId={request.CategoryId}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<PageResult<ProductViewModel>>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<PageResult<ProductViewModel>>>(body);
        }

        public async Task<ApiResult<ProductViewModel>> GetById(int productId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync($"/api/Product/{productId}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<ProductViewModel>>(body);

            return JsonConvert.DeserializeObject<ApiSuccessResult<ProductViewModel>>(body);
        }

        public async Task<ApiResult<bool>> Update(ProductUpdateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var requestContent = new MultipartFormDataContent();

            if (request.productImg1 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.productImg1.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.productImg1.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "productImg1", request.productImg1.FileName);
            }
            if (request.productImg1 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.productImg1.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.productImg1.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "productImg1", request.productImg1.FileName);
            }
            if (request.productImg2 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.productImg2.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.productImg2.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "productImg2", request.productImg2.FileName);
            }
            if (request.productImg3 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.productImg3.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.productImg3.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "productImg3", request.productImg3.FileName);
            }
            if (request.productImg4 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.productImg4.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.productImg4.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "productImg4", request.productImg4.FileName);
            }
            requestContent.Add(new StringContent(request.Id.ToString()), "Id");
            requestContent.Add(new StringContent(request.ProductName.ToString()), "ProductName");
            requestContent.Add(new StringContent(request.ProductPrice.ToString()), "ProductPrice");
            requestContent.Add(new StringContent(request.Quantity.ToString()), "Quantity");
            requestContent.Add(new StringContent(request.ProductDescription.ToString()), "ProductDescription");
            requestContent.Add(new StringContent(request.isActived.ToString()), "isActived");
            requestContent.Add(new StringContent(request.Discount.ToString()), "Discount");
            requestContent.Add(new StringContent(request.BrandId.ToString()), "BrandId");
            requestContent.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");

            var response = await client.PutAsync($"/api/Product", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }
    }
}
