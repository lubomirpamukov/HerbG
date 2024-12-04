using Herbg.Data;
using Herbg.Models;
using Herbg.ViewModels.Cart;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Herbg.Services.Interfaces;


namespace Herbg.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICarService _carService;

        public CartController(ApplicationDbContext context,
                              UserManager<ApplicationUser> userManager,
                              ICarService cartService)
        {
            _context = context;
            _userManager = userManager;
            _carService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var clientId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(clientId))
            {
                // User is not authenticated; redirect to login
                return RedirectToAction("Login", "Account");
            }

       

            if (User?.Identity?.IsAuthenticated ?? false) 
            {
                var cartViewModel = await _carService.GetUserCartAsync(clientId);
                return View(cartViewModel);
            }

            return NotFound();
        }

        public async Task<IActionResult> RemoveItem(int id)
        {
            var clientId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return Unauthorized(); 
            }

            // Load the cart with its items in a single query
            var clientCart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);

            if (clientCart == null)
            {
                return NotFound(); // Cart not found
            }

            var productToRemove = clientCart.CartItems.FirstOrDefault(ci => ci.ProductId.Equals(id));

            if (productToRemove == null)
            {
                return NotFound(); 
            }

            _context.CartItems.Remove(productToRemove);
            await _context.SaveChangesAsync();

            
            return RedirectToAction("Index", "Cart");
        }

        public async Task<IActionResult> GetCartItemCount()
        {
            var clientId = _userManager.GetUserId(User);
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);

            if (cart == null) 
            {
                var newCart = new Cart
                {
                    ClientId = clientId!
                };

                _context.Carts.Add(newCart);
                await _context.SaveChangesAsync();
            }

            var cartItemCount = cart?.CartItems?.Sum(ci => ci.Quantity) ?? 0;
            return Json(cartItemCount);  // Returns the count as JSON
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var clientId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(clientId))
            {
                // User is not authenticated; redirect to login
                return RedirectToAction("Login", "Account");
            }

            var productToAdd = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (productToAdd == null)
            {
                // Product not found
                return NotFound();
            }

            if (User?.Identity?.IsAuthenticated ?? false) 
            {
                var clientCart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);

                if (clientCart == null)
                {
                    var newCart = new Cart
                    {
                        ClientId = clientId!
                    };

                    _context.Carts.Add(newCart);
                    await _context.SaveChangesAsync();
                    clientCart = newCart;
                }

                var existingCartItem = clientCart.CartItems.FirstOrDefault(ci => ci.ProductId == productToAdd.Id);

                if (existingCartItem != null)
                {
                    // If the product is already in the cart, increase the quantity
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    // Add new item to the cart
                    var cartItemToAdd = new CartItem
                    {
                        CartId = clientCart.Id,
                        ProductId = productToAdd.Id,
                        Quantity = quantity,
                        Price = productToAdd.Price
                    };

                    clientCart.CartItems.Add(cartItemToAdd);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index","Product");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateQuantity(int id,int quantity) 
        {
            var clientId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return NotFound();
            }

            var clientCartItem = await _context.Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(ci => ci.ClientId == clientId && ci.CartItems.Any(p => p.ProductId == id));

            if (clientCartItem == null)
            {
                return NotFound();
            }

            var itemToUpdate = clientCartItem.CartItems.FirstOrDefault(c => c.ProductId == id);

            itemToUpdate!.Quantity = quantity;
            _context.CartItems.Update(itemToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MoveToWishlist(int id) 
        {
            //Check if client is logged
            var clientId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(clientId))
            {
                // Client is not authenticated; redirect to login
                return RedirectToAction("Login", "Account");
            }

            //Check if product is in client cart
            var isProductInClientCart = await _context.CartItems
                .Include(x => x.Cart)
                .FirstOrDefaultAsync(ci => ci.ProductId == id && ci.Cart.ClientId == clientId);
            if (isProductInClientCart == null)
            {
                //There is no product matching this id in client cart
                return NotFound();
            }

            //Add to wishlist
            var newWishlistItem = new Wishlist { ClientId = clientId, ProductId = isProductInClientCart.ProductId };
            await _context.Wishlists.AddAsync(newWishlistItem);
            //Remove from cart
            _context.CartItems.Remove(isProductInClientCart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Cart");
        }
    }
}
