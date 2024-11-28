using Herbg.Data;
using Herbg.Models;
using Herbg.ViewModels.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var clientId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(clientId))
            {
                return NotFound();
            }

            var clientCart = await _context.Carts
                .Where(c => c.ClientId == clientId /*&& c.Id == cartId*/)
                .Include(c => c.Client)
                .FirstOrDefaultAsync();

            if (clientCart == null)
            {
                return NotFound();
            }

            var newOrder = new Order
            {
                ClientId = clientId,
                Address = clientCart.Client.Address,
                PaymentMethod = clientCart.PaymentMethod,
                TotalAmount = clientCart.CartItems.Sum(ci => ci.Product.Price),
                ProductOrders = new List<ProductOrder>()
            };

            foreach (var item in clientCart.CartItems)
            {
                var newItem = new ProductOrder
                {
                    OrderId = newOrder.Id,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.Quantity
                };

                newOrder.ProductOrders.Add(newItem);
            }

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
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
            //Validate data
            //convert CartItems => ProductOrder
            //create Order
            //clear Cart

            return NotFound();
        }
    }
}
