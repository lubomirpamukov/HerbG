using Herbg.Data;
using Herbg.ViewModels.Product;
using Herbg.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext dbcontext)
        {
            _context = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
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

        public async Task<IActionResult> Details(int id) 
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Include(p => p.Reviews)
                .ThenInclude(r => r.Client)
                .Select(p => new ProductDetailsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category.Name,
                    Description = p.Description,
                    ImagePath = p.ImagePath,
                    Price = p.Price,
                    Reviews = p.Reviews.Select(r => new ReviewViewModel 
                    {
                        Id = r.Id,
                        Description = r.Description!,
                        Rating = r.Rating,
                        ReviewerName = r.Client.UserName!
                       
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (product == null)
            { 
                return NotFound();
            }

            return View(product);

        }
    }
}
