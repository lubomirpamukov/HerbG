using Herbg.Data;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Product;
using Herbg.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers;

public class ProductController(IProductService productService, ICategoryService categoryService,IManufactorerService manufactorerService) : Controller
{
    private readonly IProductService _productService = productService;
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IManufactorerService _manufactorerService = manufactorerService;

    public async Task<IActionResult> Index(
     string? searchQuery = null,
     string? category = null,
     string? manufactorer = null,
     int pageNumber = 1,int pageSize = 3)
    {
        var (products, totalPages) = await _productService.GetAllProductsAsync(
            searchQuery, category, manufactorer, pageNumber, pageSize);

        var categories = await _categoryService.GetCategoriesNamesAsync();
        var manufactorers = await _manufactorerService.GetManufactorersNamesAsync();

        ViewData["SearchQuery"] = searchQuery;
        ViewData["Categories"] = categories;
        ViewData["Manufactorers"] = manufactorers;
        ViewData["CurrentPage"] = pageNumber;
        ViewData["TotalPages"] = totalPages;

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
