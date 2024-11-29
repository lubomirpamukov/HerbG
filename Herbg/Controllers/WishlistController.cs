using Herbg.Data;
using Herbg.Models;
using Herbg.ViewModels.Wishlist;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishlistController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //Check if client exist
            var clientId = _userManager.GetUserId(User);
            if (clientId == null)
            { 
                return NotFound();
            }

            //Check client wishlist
            var clientWishlists = await _context.Wishlists
                .Where(w => w.ClientId == clientId)
                .Include(w => w.Product)
                .Select(c => new WishlistItemViewModel 
                {
                    Id = c.ProductId,
                    Description = c.Product.Description,
                    ImagePath = c.Product.ImagePath,
                    Name = c.Product.Name,
                    Price = c.Product.Price
                })
                .ToArrayAsync();



            return View(clientWishlists);
        }

        public async Task<IActionResult> Add(int productId) 
        {
            //Check if client exist
            var clientId = _userManager.GetUserId(User);
            if (clientId == null)
            {
                return NotFound();
            }

            //Check if wishlist item exist
            var wishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(c => c.ClientId == clientId && c.ProductId == productId);
            if (wishlistItem == null)
            {
                var newWishlistItem = new Wishlist { ClientId = clientId, ProductId = productId };
                _context.Wishlists.Add(newWishlistItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index","Wishlist");
        }

        public async Task<IActionResult> RemoveFromWishlist(int productId) 
        {
            //Check if client exist
            var clientId = _userManager.GetUserId(User);
            if (clientId == null)
            {
                return NotFound();
            }

            //Check if wishlist item exist
            var wishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(c => c.ClientId == clientId && c.ProductId == productId);
            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Wishlist");
        }
    }
}
