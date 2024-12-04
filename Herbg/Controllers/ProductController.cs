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
        var product = await _productService.GetProductDetailsAsync(id);

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
