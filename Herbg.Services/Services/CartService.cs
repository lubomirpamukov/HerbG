using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herbg.Infrastructure;
using Herbg.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Services.Services;

public class CartService(IRepositroy<Cart> cart) : ICarService
{
    private readonly IRepositroy<Cart> _cart = cart;
    public async Task<CartViewModel> GetUserCartAsync(string clientId)
    {
        var clientCart = await _cart.GetAllAttachedAsync()
              .Where(c => c.ClientId == clientId)
              .Select(c => new CartViewModel
              {
                  Id = c.Id,
                  CartItems = c.CartItems.Select(ci => new CartItemViewModel
                  {
                      ProductId = ci.ProductId,
                      ImagePath = ci.Product.ImagePath,
                      Name = ci.Product.Name,
                      Price = ci.Price,
                      Quantity = ci.Quantity
                  }).ToList()
              })
              .FirstOrDefaultAsync();

        if (clientCart == null)
        {
            var newCart = new Cart
            {
                ClientId = clientId!
            };

            await _cart.AddAsync(newCart);
        }

        return clientCart!;
    }
}
