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
        private readonly ICartService _cartService;

        public CartController(ApplicationDbContext context,
                              UserManager<ApplicationUser> userManager,
                              ICartService cartService)
        {
            _context = context;
            _userManager = userManager;
            _cartService = cartService;
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
                var cartViewModel = await _cartService.GetUserCartAsync(clientId);
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

            var IsItemRemoved = await _cartService.RemoveCartItemAsync(clientId, id);

            if (IsItemRemoved)
            {
                return RedirectToAction("Index", "Cart");
            }
            else 
            {
                return NotFound();
            }
            
        }

        public async Task<IActionResult> GetCartItemCount()
        {
            var clientId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return NotFound();
            }

            var cartItemCount = await _cartService.GetCartItemsCountAsync(clientId);
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


            if (User?.Identity?.IsAuthenticated ?? false) 
            {
                //User is registered
                var isProducAdded = await _cartService.AddItemToCartAsync(clientId, id, quantity);
                if (!isProducAdded)
                {
                    return NotFound();
                }

                return RedirectToAction("Index", "Product");
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

            var isQuantityUpdated = await _cartService.UpdateCartItemQuantityAsync(clientId, id, quantity);
            if (!isQuantityUpdated)
            {
                return NotFound();
            }
            else
            {
                return RedirectToAction("Index");
            }
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
