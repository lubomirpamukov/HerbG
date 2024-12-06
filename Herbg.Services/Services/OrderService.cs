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

public class OrderService(IRepository<Cart> cart, IRepository<Order>order) : IOrderService
{
    private readonly IRepository<Order> _order = order;
    private readonly IRepository<Cart> _cart = cart;

    public async Task<ICollection<OrderSummaryViewModel>> GetAllOrdersAsync(string clientId)
    {
        //Get user orders 
        var orders = await _order
            .GetAllAttached()
        .Where(o => o.ClientId == clientId)
        .Include(o => o.ProductOrders)
        .ToListAsync();

        //Create view model
        List<OrderSummaryViewModel> viewModel = new List<OrderSummaryViewModel>();

        foreach (var order in orders)
        {
            var newOrder = new OrderSummaryViewModel
            {
                OrderId = order.Id,
                Date = order.Date,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod.ToString(),
                TotalItems = order.ProductOrders.Sum(po => po.Quantity)
            };
            viewModel.Add(newOrder);
        }

        return viewModel;
    }

    public async Task<CheckoutViewModel> GetCheckout(string clientId,string cartId)
    {
        var clientCart = await _cart
            .GetAllAttached()
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

    public async Task<string> GetOrderConfirmed(string clientId, CheckoutViewModel model)
    {
        // Fetch the cart and associated items
        var cartToRemove = await _cart
            .GetAllAttached()
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product) // Ensure Product details are included if needed for validation
            .FirstOrDefaultAsync(c => c.ClientId == clientId);

        if (cartToRemove == null)
        {
            return null;
        }

        // Calculate the total price dynamically from cart items
        decimal calculatedTotal = cartToRemove.CartItems.Sum(item => item.Price * item.Quantity);

        // Create the new order
        var newOrder = new Order
        {
            ClientId = clientId,
            Address = model.Address,
            PaymentMethod = model.PaymentMethod,
            TotalAmount = calculatedTotal,
            ProductOrders = new List<ProductOrder>()
        };

        // Convert cart items to ProductOrder entries
        foreach (var item in cartToRemove.CartItems)
        {
            var newItem = new ProductOrder
            {
                ProductId = item.ProductId,
                Price = item.Price,
                Quantity = item.Quantity
            };

            newOrder.ProductOrders.Add(newItem);
        }

        // Save the new order and remove the cart
        await _order.AddAsync(newOrder);
        await _cart.DeleteAsync(cartToRemove);
        return newOrder.Id;
    }

    public async Task<OrderDetailsViewModel> GetOrderDetailsAsync(string orderId)
    {
        //Finds order and includes all needed tables
        var order = await _order
            .GetAllAttached()
            .Where(o => o.Id == orderId)
            .Include(op => op.ProductOrders)
            .ThenInclude(po => po.Product)
            .Include(o => o.Client)
            .FirstOrDefaultAsync();

        //Check if order exist
        if (order == null)
        {
            return null;
        }

        //Crate order view model
        var orderDetailsViewModel = new OrderDetailsViewModel
        {
            OrderId = order.Id,
            Date = order.Date,
            TotalAmount = order.TotalAmount,
            CustomerName = order.Client.UserName ?? "Anonymouse",
            Address = order.Address,
            CustomerEmail = order.Client.Email ?? "Anonymouse",
            PaymentMethod = order.PaymentMethod,
            OrderedProducts = order.ProductOrders.Select(product => new OrderedProductViewModel
            {
                Price = product.Price,
                ProductName = product.Product.Name,
                Quantity = product.Quantity

            }).ToList(),
        };

        return orderDetailsViewModel;
    }
}
