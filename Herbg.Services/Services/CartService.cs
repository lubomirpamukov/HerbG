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

    public async Task<int> GetCartItemsCountAsync(string clientId)
    {
        var cart = await _cart.GetAllAttachedAsync()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);

        if (cart == null)
        {
            var newCart = new Cart
            {
                ClientId = clientId!
            };

           await _cart.AddAsync(newCart);
        }

        var cartItemCount = cart?.CartItems?.Sum(ci => ci.Quantity) ?? 0;

        return cartItemCount;
    }

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

    public async Task<bool> RemoveCartItemAsync(string clientId, int productId)
    {
        // Load the cart with its items in a single query
        var clientCart = await _cart.GetAllAttachedAsync()
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.ClientId == clientId);

        if (clientCart == null)
        {
            return false;
        }

        var productToRemove = clientCart.CartItems.FirstOrDefault(ci => ci.ProductId.Equals(productId));

        if (productToRemove == null)
        {
            return false;
        }

        clientCart.CartItems.Remove(productToRemove);
        await _cart.UpdateAsync(clientCart);
        return true;
    }
}
