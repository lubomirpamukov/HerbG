using Herbg.Data;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Product;
using Herbg.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers;

public class ProductController(ApplicationDbContext dbcontext,IProductService productService) : Controller
{
    private readonly ApplicationDbContext _context = dbcontext;
    private readonly IProductService _productService = productService;

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProductsAsync();


        return View(products);
    }

    public async Task<IActionResult> Details(int id) 
    {
        var product = await _context.Products
            .Where(p => p.Id == id)
            .Include(p => p.Category)
            .Include(p => p.Manufactorer)
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
                Manufactorer = p.Manufactorer.Name,
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

    public async Task<IActionResult> ProductByCategory(int categoryId) 
    {
        var productsByCategory = await _context.Categories
            .Where(c => c.Id == categoryId && c.IsDeleted == false)
            .Include(p => p.Products)
            .FirstOrDefaultAsync();

        if (productsByCategory == null)
        {
            return NotFound();
        }

        var viewModelCollection = new List<ProductCardViewModel>();

        foreach (var product in productsByCategory.Products)
        {
            var newProduct = new ProductCardViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImagePath = product.ImagePath,
                Price = product.Price
            };

            viewModelCollection.Add(newProduct);
        }

        return View(viewModelCollection);
    }
}
