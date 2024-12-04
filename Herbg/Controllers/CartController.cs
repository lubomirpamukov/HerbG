using Herbg.Data;
using Herbg.Models;
using Herbg.ViewModels.Cart;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Herbg.Services.Interfaces;


namespace Herbg.Controllers
{
    public class CartController(UserManager<ApplicationUser> userManager,ICartService cartService) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ICartService _cartService = cartService;

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

            var isMovedToWishlist = await _cartService.MoveCartItemToWishListAsync(clientId, id);

            if (!isMovedToWishlist) 
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Cart");
        }
    }
}
