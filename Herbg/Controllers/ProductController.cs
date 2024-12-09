using Herbg.Data;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Product;
using Herbg.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers;

public class ProductController(IProductService productService) : Controller
{
    private readonly IProductService _productService = productService;

    public async Task<IActionResult> Index(string? searchQuery = null)
    {
        var products = await _productService.GetAllProductsAsync(searchQuery);
        ViewData["SearchQuery"] = searchQuery;
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
        var viewModelCollection = await _productService.GetProductsByCategoryAsync(categoryId);

        return View(viewModelCollection);
    }
}
