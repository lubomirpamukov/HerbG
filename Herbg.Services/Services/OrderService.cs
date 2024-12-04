using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herbg.ViewModels.Order;


namespace Herbg.Services.Services;

public class OrderService(IRepositroy<Cart> cart) : IOrderService
{
    private readonly IRepositroy<Cart> _cart = cart;
    public async Task<CheckoutViewModel> GetCheckout(string clientId,string cartId)
    {
        var clientCart = await _cart
            .GetAllAttachedAsync()
            .Where(c => c.Id == cartId)
            .Include(c => c.Client)
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync();

        if (clientCart == null)
        {
            return null;
        }

        var totalProductPrice = clientCart.CartItems.Sum(c => c.Price * c.Quantity);

        var checkoutView = new ViewModels.Order.CheckoutViewModel
        {
            Address = clientCart.Client.Address!,
            CartItems = clientCart.CartItems.Select(c => new ViewModels.Cart.CartItemViewModel
            {
                ProductId = c.ProductId,
                ImagePath = c.Product.ImagePath,
                Name = c.Product.Name,
                Price = c.Price,
                Quantity = c.Quantity
            }).ToList(),
            Subtotal = totalProductPrice,
            ShippingCost = 10,
            Total = totalProductPrice + 10
        };

        return checkoutView;
    }
}
