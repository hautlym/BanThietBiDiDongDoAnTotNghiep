using BanThietBiDiDongDATN.Application.Catalog.Carts.Dtos;
using BanThietBiDiDongDATN.Data.EF;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            try
            {
                var product = _context.products.Where(x => x.Id == request.ProductId).Select(x=>new { productOptions= x.productOptions, Discount = x.Discount }).FirstOrDefault();
                if (request == null|| product==null)
                {
                    return -1;
                }
                var option = product.productOptions.Where(x => x.Id == request.OptionId).FirstOrDefault();
                var cart = new Cart()
                {
                    OptionId = request.OptionId,
                    ProductId = request.ProductId,
                    AppUserId = request.UserId,
                    Quantity = request.Quantity,
                    Price = product.Discount > 0 ? ((Convert.ToDecimal(option.OptionPrice) - (Convert.ToDecimal(option.OptionPrice) * ((decimal)product.Discount / 100))))*request.Quantity : Convert.ToDecimal(option.OptionPrice)*request.Quantity
                };
                _context.carts.Add(cart);
                await _context.SaveChangesAsync();
                return cart.Id;
            }catch(Exception ex)
            {
                return -1;
            }
            
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
            var cart = from p in _context.products
                       join c in _context.carts on p.Id equals c.ProductId
                       join u in _context.appUsers on c.AppUserId equals u.Id
                       join o in _context.productOptions on p.Id equals o.ProductId
                       select new
                       {
                           UserName = u.FirstName + u.LastName,
                           CartId = c.Id,
                           ProductId = p.Id,
                           ProductName = p.ProductName,
                           Discount = p.Discount,
                           Quantity = c.Quantity,
                           optionColor = o.ColorOption,
                           optionSize = o.SizeOption,
                           ProductPrice = o.OptionPrice,
                           totalPrice = c.Price,
                           Img = p.productImgs.Count > 0 ? p.productImgs[0].ImagePath : "",
                       };

            var data = await cart.Select(x => new CartViewModel()
            {
                id = x.CartId,
                UserName = x.UserName,
                Discount = x.Discount,
                ProductNane = x.ProductName,
                ProductPrice = Convert.ToDouble(x.ProductPrice),
                ProductOriginal = x.Discount > 0 ? (Convert.ToDouble(x.ProductPrice) / (1 - (x.Discount / 100))) : Convert.ToDouble(x.ProductPrice),
                Quantity = x.Quantity,
                totalPrice = x.totalPrice.ToString(),
                ImgUrl = x.Img,
                ProductId = x.ProductId,
                OptionColor = x.optionColor,
                OptionSize = x.optionSize,
            }).ToListAsync();
            return data;
        }

        public async Task<List<CartViewModel>> GetAllCartByUserId(Guid UserId)
        {
            var cart = from p in _context.products
                       join c in _context.carts on p.Id equals c.ProductId
                       join u in _context.appUsers on c.AppUserId equals u.Id
                       join o in _context.productOptions on c.OptionId equals o.Id
                       where u.Id == UserId
                       select new
                       {
                           UserName = u.FirstName + u.LastName,
                           CartId = c.Id,
                           ProductName = p.ProductName,
                           Discount = p.Discount,
                           ProductId = p.Id,
                           Quantity = c.Quantity,
                           totalPrice = c.Price,
                           optionColor = o.ColorOption,
                           optionSize = o.SizeOption,
                           ProductPrice = o.OptionPrice,
                           Img = p.productImgs.Count > 0 ? p.productImgs[0].ImagePath : "",
                       };

            var data = await cart.Select(x => new CartViewModel()
            {
                id = x.CartId,
                UserName = x.UserName,
                Discount = x.Discount,
                ProductNane = x.ProductName,
                ProductPrice = Convert.ToDouble(x.ProductPrice),
                ProductOriginal = x.Discount > 0 ? (Convert.ToDouble(x.ProductPrice) / (1 - (x.Discount / 100))) : Convert.ToDouble(x.ProductPrice),
                Quantity = x.Quantity,
                ImgUrl = x.Img,
                totalPrice=x.totalPrice.ToString(),
                OptionColor = x.optionColor,
                OptionSize = x.optionSize,
                ProductId = x.ProductId
            }).ToListAsync();
            return data;
        }

        public async Task<CartViewModel> GetById(int CartId)
        {
            var cart = from p in _context.products
                       join c in _context.carts on p.Id equals c.ProductId
                       join u in _context.appUsers on c.AppUserId equals u.Id
                       join o in _context.productOptions on c.OptionId equals o.Id
                       where c.Id == CartId
                       select new
                       {
                           UserName = u.FirstName + u.LastName,
                           CartId = c.Id,
                           ProductName = p.ProductName,
                           Discount = p.Discount,
                           Quantity = c.Quantity,
                           ProductId = p.Id,
                           optionColor = o.ColorOption,
                           optionSize = o.SizeOption,
                           ProductPrice = o.OptionPrice,
                           totalPrice = c.Price,
                           Img = p.productImgs.Count > 0 ? p.productImgs[0].ImagePath : "",
                       };

            var data = await cart.Select(x => new CartViewModel()
            {
                id = x.CartId,
                UserName = x.UserName,
                Discount = x.Discount,
                ProductNane = x.ProductName,
                ProductPrice = Convert.ToDouble(x.ProductPrice),
                ProductOriginal = x.Discount > 0 ? (Convert.ToDouble(x.ProductPrice) / (1 - (x.Discount / 100))) : Convert.ToDouble(x.ProductPrice),
                Quantity = x.Quantity,
                ImgUrl = x.Img,
                totalPrice = x.totalPrice.ToString(),
                OptionColor = x.optionColor,
                OptionSize = x.optionSize,
                ProductId = x.ProductId
            }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<int> Update(UpdateCartRequest request)
        {
            var cart = await _context.carts.Where(x => x.Id == request.id).FirstOrDefaultAsync();
            if (cart == null) 
                return -1;
            var originPrice = cart.Price / cart.Quantity;
            cart.Price = originPrice*request.Quantity;
            cart.Quantity = request.Quantity;
            _context.Update(cart);
            return await _context.SaveChangesAsync();
        }
    }
}
