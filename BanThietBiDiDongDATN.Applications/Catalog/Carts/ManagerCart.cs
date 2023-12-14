using BanThietBiDiDongDATN.Application.Catalog.Carts.Dtos;
using BanThietBiDiDongDATN.Data.EF;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Carts
{
    public class ManagerCart : IManageCart
    {
        private readonly BanThietBiDiDongDATNDbContext _context;

        public ManagerCart(BanThietBiDiDongDATNDbContext context)
        {
            _context = context;
        }
        public async Task<int> Create(CreateCartRequest request)
        {
            var cart = new Cart()
            {
                ProductId = request.ProductId,
                AppUserId = request.UserId,
                Quantity = request.Quantity,
            };
            _context.carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart.Id;
        }

        public async Task<int> Delete(int CartId)
        {
            var cart = _context.carts.Where(x => x.Id == CartId).FirstOrDefault();
            if (cart == null) throw new NotImplementedException();
            _context.carts.Remove(cart);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<CartViewModel>> GetAllCart()
        {
            throw new NotImplementedException();
            //var cart = from p in _context.products
            //           join c in _context.carts on p.Id equals c.ProductId
            //           join u in _context.appUsers on c.AppUserId equals u.Id
            //           select new
            //           {
            //               UserName = u.FirstName + u.LastName,
            //               CartId = c.Id,
            //               p.ProductName,
            //               p.ProductPrice,
            //               p.Discount,
            //               c.Quantity,
            //               Img = p.productImgs.Count > 0 ? p.productImgs[0].ImagePath : "",
            //               u.Address,
            //               ProductOriginal = p.ProductOriginalPrice,
            //           };

            //var data = await cart.Select(x => new CartViewModel()
            //{
            //    id = x.CartId,
            //    UserName = x.UserName,
            //    Discount = x.Discount,
            //    ProductNane = x.ProductName,
            //    ProductPrice = x.ProductPrice,
            //    ProductOriginal = x.ProductOriginal,
            //    Quantity = x.Quantity,
            //    ImgUrl = x.Img,
            //    Address = x.Address
            //}).ToListAsync();
            //return data;
        }

        public async Task<List<CartViewModel>> GetAllCartByUserId(Guid UserId)
        {
            throw new NotImplementedException();
            //var cart = from p in _context.products
            //           join c in _context.carts on p.Id equals c.ProductId
            //           join u in _context.appUsers on c.AppUserId equals u.Id
            //           select new
            //           {
            //               UserId = u.Id,
            //               UserName = u.FirstName + u.LastName,
            //               CartId = c.Id,
            //               p.ProductName,
            //               p.ProductPrice,
            //               ProductOriginal = p.ProductOriginalPrice,
            //               p.Discount,
            //               c.Quantity,
            //               Img = p.productImgs.Count > 0 ? p.productImgs[0].ImagePath : "",
            //               u.Address
            //           };

            //var data = await cart.Where(x => x.UserId == UserId).Select(x => new CartViewModel()
            //{
            //    id = x.CartId,
            //    UserName = x.UserName,
            //    Discount = x.Discount,
            //    ProductNane = x.ProductName,
            //    ProductPrice = x.ProductPrice,
            //    ProductOriginal = x.ProductOriginal,
            //    Quantity = x.Quantity,
            //    ImgUrl = x.Img,
            //    Address = x.Address
            //}).ToListAsync();
            //return data;
        }

        public async Task<CartViewModel> GetById(int CartId)
        {
            throw new NotImplementedException();
            //var cart = from p in _context.products
            //           //join c in _context.Carts on p.Id equals c.ProductId
            //           join u in _context.appUsers on c.AppUserId equals u.Id
            //           select new
            //           {
            //               UserId = u.Id,
            //               UserName = u.FirstName + u.LastName,
            //               CartId = c.Id,
            //               p.ProductName,
            //               p.ProductPrice,
            //               p.Discount,
            //               c.Quantity,
            //               Img = p.productImgs.Count > 0 ? p.productImgs[0].ImagePath : "",
            //               u.Address,
            //               Phone = u.PhoneNumber
            //           };

            //var data = await cart.Where(x => x.CartId == CartId).Select(x => new CartViewModel()
            //{
            //    id = x.CartId,
            //    UserName = x.UserName,
            //    Discount = x.Discount,
            //    ProductNane = x.ProductName,
            //    ProductPrice = x.ProductPrice,
            //    Quantity = x.Quantity,
            //    ImgUrl = x.Img,
            //    Address = x.Address,
            //    Phone = x.Phone

            //}).FirstOrDefaultAsync();

            //if (data != null)
            //{
            //    return data;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public async Task<int> Update(UpdateCartRequest request)
        {
            var cart = await _context.carts.Where(x => x.Id == request.id).FirstOrDefaultAsync();
            //if (cart == null) throw new BTL_KTPMException("Can not find product");

            cart.ProductId = request.ProductId;
            cart.Quantity = request.Quantity;
            cart.Price = request.Price;
            _context.Update(cart);
            return await _context.SaveChangesAsync();
        }
    }
}
