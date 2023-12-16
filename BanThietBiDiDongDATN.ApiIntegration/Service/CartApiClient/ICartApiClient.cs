﻿using BanThietBiDiDongDATN.Application.Catalog.Carts;
using BanThietBiDiDongDATN.Application.Catalog.Carts.Dtos;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.ApiIntegration.Service.CartApiClient
{
    public interface ICartApiClient
    {
        Task<List<CartViewModel>> GetAll();
        Task<List<CartViewModel>> GetAllByUserId(Guid UserId);
        Task<ApiResult<bool>> CreateCart(CreateCartRequest request);
        Task<bool> UpdateCart(int id, UpdateCartRequest request);
        Task<CartViewModel> GetById(int id);
        Task<bool> Delete(int id);
    }
}
