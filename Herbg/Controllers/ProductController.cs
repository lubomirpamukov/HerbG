using Herbg.Data;
using Herbg.ViewModels.Product;
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
    }
}
