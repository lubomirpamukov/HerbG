using Herbg.Models;
using Herbg.Data;
using Herbg.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProductCardViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImagePath = p.ImagePath,
                    Description = p.Description,
                    Price = p.Price
                })
                .ToArrayAsync();


            return View(products);
        }

        public async Task<IActionResult> Delete(int productId) 
        {
            //Find the product for deletion
            var productToDelete = await _context.Products
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (productToDelete == null)
            {
                return NotFound();
            }

            //SoftDelete the object
            productToDelete.IsDeleted = true;

            //Delete from all user carts
            var productInCarts = await _context.CartItems
                .Where(ci => ci.ProductId == productId)
                .ToArrayAsync();
            _context.CartItems.RemoveRange(productInCarts);

            //Remove from all wishlists
            var productInWishlists = await _context.Wishlists
                .Where(w => w.ProductId == productId)
                .ToArrayAsync();
            _context.Wishlists.RemoveRange(productInWishlists);

            //Remove Reviews for the product
            var reviews = productToDelete.Reviews.ToArray();
            _context.Reviews.RemoveRange(reviews);


            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home", new {area = "Admin"});
        }
    }
}
