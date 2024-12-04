using Herbg.Data;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOrderService order)
    {
        _context = context;
        _userManager = userManager;
        _orderService = order;
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

        var clientCartView = await _orderService.GetCheckout(clientId, cartId);

        if (clientCartView == null) 
        {
            return NotFound();
        }
        
        return View(clientCartView);
    }

    public async Task<IActionResult> ConfirmOrder(CheckoutViewModel model)
    {
        // Validate user ID
        var clientId = _userManager.GetUserId(User);

        if (string.IsNullOrWhiteSpace(clientId))
        {
            return NotFound();
        }

        var orderId = await _orderService.GetOrderConfirmed(clientId, model);
        if (orderId == null)
        {
            return NotFound();
        }
        return RedirectToAction("ThankYou", "Order", new { orderNumber = orderId });

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
