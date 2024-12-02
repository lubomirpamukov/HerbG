using Herbg.Data;
using Herbg.Models;
using Herbg.ViewModels.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public IActionResult Index()
    {
        return NotFound();
    }

    public async Task<IActionResult> Checkout(string cartId) 
    {
        var clientId = _userManager.GetUserId(User);

        if (string.IsNullOrEmpty(clientId))
        {
            return NotFound();
        }

        var clientCart = await _context.Clients
            .Where(c => c.Id == clientId)
            .Include(c => c.Cart)
            .ThenInclude(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync();

        if (clientCart == null)
        {
            return NotFound();
        }

        var totalProductPrice = clientCart.Cart.CartItems.Sum(c => c.Price * c.Quantity);

        var checkoutView = new CheckoutViewModel
        {
            Address = clientCart.Address,
            CartItems = clientCart.Cart.CartItems.Select(c => new ViewModels.Cart.CartItemViewModel
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
        

        
        return View(checkoutView);
    }

    public async Task<IActionResult> ConfirmOrder(CheckoutViewModel model)
    {
        // Validate user ID
        var clientId = _userManager.GetUserId(User);

        if (string.IsNullOrWhiteSpace(clientId))
        {
            return NotFound();
        }

        // Fetch the cart and associated items
        var cartToRemove = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product) // Ensure Product details are included if needed for validation
            .FirstOrDefaultAsync(c => c.ClientId == clientId);

        if (cartToRemove == null)
        {
            return NotFound();
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
        _context.Orders.Add(newOrder);
        _context.Carts.Remove(cartToRemove);
        await _context.SaveChangesAsync();

        return RedirectToAction("ThankYou", "Order", new { orderNumber = newOrder.Id });
    }


    public async Task<IActionResult> Details(string id) 
    {
        //Finds order and includes all needed tables
        var order = await _context.Orders
            .Where(o => o.Id == id)
            .Include(op => op.ProductOrders)
            .ThenInclude(po => po.Product)
            .Include(o => o.Client)
            .FirstOrDefaultAsync();

        //Check if order exist
        if (order == null) 
        {
            return NotFound();
        }

        //Crate order view model
        var orderDetailsViewModel = new OrderDetailsViewModel 
        {
            OrderId = order.Id,
            Date = order.Date,
            TotalAmount = order.TotalAmount,
            CustomerName = order.Client.UserName?? "Anonymouse",
            Address = order.Address,
            CustomerEmail = order.Client.Email?? "Anonymouse",
            PaymentMethod = order.PaymentMethod,
            OrderedProducts = order.ProductOrders.Select(product => new OrderedProductViewModel 
            {
                Price = product.Price,
                ProductName = product.Product.Name,
                Quantity = product.Quantity

            }).ToList(),
        };

        return View(orderDetailsViewModel);
    }

    public async Task<IActionResult> Orders() 
    {
        //Check for valid user
        var clientId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(clientId))
        {
            return NotFound();
        }

        //Get user orders 
        var orders = await _context.Orders
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

        return View(viewModel);
    }

    public IActionResult ThankYou(string orderNumber)
    {
        var viewModel = new ThankYouViewModel
        {
            OrderNumber = orderNumber
        };

        return View(viewModel);
    }
}
