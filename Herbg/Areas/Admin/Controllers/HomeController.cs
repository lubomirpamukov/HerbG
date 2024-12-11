using Herbg.Models;
using Herbg.Data;
using Herbg.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using Herbg.Infrastructure.Interfaces;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Category;
using Herbg.Services.Services;

namespace Herbg.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class HomeController(
    IProductService productService,
    ICategoryService categoryService,
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    IManufactorerService manufactorerService) : Controller
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IProductService _productService = productService;
    private readonly ICategoryService _categoryService = categoryService; 
    private readonly IManufactorerService _manufactorerService = manufactorerService;

    public async Task<IActionResult> Index(
     string? searchQuery = null,
     string? category = null,
     string? manufactorer = null,
     int pageNumber = 1,
     int pageSize = 3)
    {
        var (products, totalPages, categories, manufactorers) = await _productService.GetAllProductsAsync(
            searchQuery, category, manufactorer, pageNumber, pageSize);

        ViewData["SearchQuery"] = searchQuery;
        ViewData["Categories"] = categories;
        ViewData["Manufactorers"] = manufactorers;
        ViewData["CurrentPage"] = pageNumber;
        ViewData["TotalPages"] = totalPages;

        // Return the products to the view
        return View(products);
    }


    [HttpGet]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var productToDelete = await _productService.GetProductByIdAsync(productId); 
        if (productToDelete == null)
        {
            return NotFound();
        }

        var model = new DeleteConfirmationViewModel
        {
            ItemId = productToDelete.Id,
            ItemName = productToDelete.Name,
            ItemDescription = productToDelete.Description
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(int productId)
    {
        var result = await _productService.SoftDeleteProductAsync(productId);

        if (!result)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Product deleted successfully!";
        return RedirectToAction("Index", "Home", new { area = "Admin" });
    }


    [HttpGet]
    public async Task<IActionResult> EditProduct(int productId)
    {
        // Use the service to get the product
        var product = await _productService.GetProductForEditAsync(productId);

        if (product == null)
        {
            return NotFound();
        }

        // Use the service to get the dropdown data
        ViewBag.Categories = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name", product.CategoryId);
        ViewBag.Manufacturers = new SelectList(await _manufactorerService.GetManufacturersAsync(), "Id", "Name", product.ManufactorerId);

        return View(product);
    }


    [HttpPost]
    public async Task<IActionResult> EditProduct(CreateProductViewModel model)
    {
        // Validate the model state
        if (!ModelState.IsValid)
        {
            // Populate dropdowns in case of validation errors
            ViewBag.Categories = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name", model.CategoryId);
            ViewBag.Manufacturers = new SelectList(await _manufactorerService.GetManufacturersAsync(), "Id", "Name", model.ManufactorerId);

            return View(model);
        }

        // Use the product service to update the product
        var result = await _productService.UpdateProductAsync(model);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while updating the product.");
            ViewBag.Categories = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name", model.CategoryId);
            ViewBag.Manufacturers = new SelectList(await _manufactorerService.GetManufacturersAsync(), "Id", "Name", model.ManufactorerId);

            return View(model);
        }

        return RedirectToAction("Index", "Home", new { area = "Admin" });
    }


    public async Task<IActionResult> AddProduct()
    {
        // Create the model
        var createProductViewModel = new CreateProductViewModel();

        // Use services to populate dropdowns
        ViewBag.Categories = new SelectList(await _categoryService.GetCategoriesAsync(), "Id", "Name");
        ViewBag.Manufacturers = new SelectList(await _manufactorerService.GetManufacturersAsync(), "Id", "Name");

        return View(createProductViewModel);
    }


    [HttpPost]
    public async Task<IActionResult> AddProduct(CreateProductViewModel model)
    {
        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            // Repopulate dropdowns using services
            ViewBag.Manufacturers = await _manufactorerService.GetManufacturersAsync();
            ViewBag.Categories = await _categoryService.GetCategoriesAsync();

            return View(model);
        }

        // Delegate the creation of the new product to the ProductService
        var result = await _productService.AddProductAsync(model);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while saving the product.");
            return View(model);
        }

        return RedirectToAction("Index", "Home", new { area = "Admin" });
    }


    public async Task<IActionResult> CategoryIndex() 
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
            
        return View(categories);
    }
    
    [HttpGet]
    public IActionResult AddCategory() 
    {
        var categoryView = new CategoryCardViewModel();
        return View(categoryView);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryCardViewModel model)
    {
        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Delegate the category creation to the service
        var result = await _categoryService.AddCategoryAsync(model);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while adding the category.");
            return View(model);
        }

        return RedirectToAction(nameof(CategoryIndex));
    }


    [HttpGet]
    public async Task<IActionResult> EditCategory(int categoryId)
    {
        var categoryViewModel = await _categoryService.GetCategoryByIdAsync(categoryId);

        if (categoryViewModel == null)
        {
            return NotFound();
        }

        return View(categoryViewModel);
    }


    [HttpPost]
    public async Task<IActionResult> EditCategory(CategoryCardViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var isSuccess = await _categoryService.EditCategoryAsync(model);

        if (!isSuccess)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(CategoryIndex));
    }


    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        var isSuccess = await _categoryService.DeleteCategoryAsync(categoryId);

        if (!isSuccess)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(CategoryIndex));
    }

}
