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

namespace Herbg.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class HomeController(
    IProductService productService,
    ICategoryService categoryService,
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager) : Controller
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IProductService _productService = productService;
    private readonly ICategoryService _categoryService = categoryService; 

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
        var item = await _context.Products.FindAsync(productId); 
        if (item == null)
        {
            return NotFound();
        }

        var model = new DeleteConfirmationViewModel
        {
            ItemId = item.Id,
            ItemName = item.Name,
            ItemDescription = item.Description
        };

        return View(model);
    }

    [HttpPost]
    //Refactor to work with service and write tests
    public async Task<IActionResult> ConfirmDelete(int productId) 
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

    //Refactor to work with service and write tests
    [HttpGet]
    public IActionResult EditProduct(int productId)
    {
        var product = _context.Products
             .Include(p => p.Category)
             .Include(p => p.Manufactorer)  // Include Manufacturer if necessary
             .FirstOrDefault(p => p.Id == productId);

        if (product == null)
        {
            return NotFound();
        }

        var model = new CreateProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            ImagePath = product.ImagePath,
            CategoryId = product.CategoryId,
            ManufactorerId = product.ManufactorerId,  
                                                     
        };

        
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
        ViewBag.Manufacturers = new SelectList(_context.Manufactorers, "Id", "Name", model.ManufactorerId);

        return View(model);
    }

    //Refactor to work with service and write tests
    [HttpPost]
    public async Task<IActionResult> EditProduct(CreateProductViewModel model) 
    {
        //Check if model state is valid
        if (!ModelState.IsValid) 
        {
            //Repopulate dropdowns in case of validation errors
            ViewBag.Manufacturers = await _context.Manufactorers
                .Select(m => new SelectListItem 
                {
                    Value = m.Id.ToString(),
                    Text = m.Name,
                })
                .ToListAsync();

            ViewBag.Manufacturers = await _context.Manufactorers
                 .Select(m => new SelectListItem
                 {
                     Value = m.Id.ToString(),
                     Text = m.Name,
                 })
                 .ToListAsync();

            return View(model);
        }

        //Get the product to edit
        var productToEdit = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == model.Id);
        if (productToEdit == null)
        {
            return  NotFound();
        }

        productToEdit.Name = model.Name;
        productToEdit.Description = model.Description;
        productToEdit.CategoryId = model.CategoryId;
        productToEdit.Price = model.Price;
        productToEdit.ManufactorerId = model.ManufactorerId;
        productToEdit.ImagePath = model.ImagePath;

        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home", new { area = "Admin" });
    }

    //Refactor to work with service and write tests
    [HttpGet]
    public async Task<IActionResult> AddProduct() 
    {
        //Create model
        var createProductViewModel = new CreateProductViewModel();

        //Create categories viewbag
        ViewBag.Categories = await _context.Categories
            .Select(c => new SelectListItem 
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToListAsync();

        //Create manufacturers viewbag
        ViewBag.Manufacturers = await _context.Manufactorers
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToListAsync();

        return View(createProductViewModel);
    }

    //Refactor to work with service and write tests
    [HttpPost]
    public async Task<IActionResult> AddProduct(CreateProductViewModel model)
    {
        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            // Repopulate dropdowns
            ViewBag.Manufacturers = await _context.Manufactorers
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                })
                .ToListAsync();

            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            return View(model);
        }

        // Create a new product object
        var newProduct = new Product
        {
            Name = model.Name,
            Price = model.Price,
            Description = model.Description,
            ManufactorerId = model.ManufactorerId,
            CategoryId = model.CategoryId,
            ImagePath = model.ImagePath  // Directly use the ImagePath URL entered by the user
        };

        // Save product to the database
        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home", new { area = "Admin" });
    }

    //Refactor to work with service and write tests
    public async Task<IActionResult> CategoryIndex() 
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
            
        return View(categories);
    }
    
    //Refactor to work with service and write tests
    [HttpGet]
    public IActionResult AddCategory() 
    {
        var categoryView = new CategoryCardViewModel();
        return View(categoryView);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryCardViewModel model) 
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var categoryToAdd = new Category 
        {
            Name = model.Name,
            ImagePath = model.ImagePath,
            Description = model.Description,
        };

        _context.Categories.Add(categoryToAdd);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(CategoryIndex));
    }

    [HttpGet]
    public async Task<IActionResult> EditCategory(int categoryId) 
    {
        var categoryViewModel = await _context.Categories
            .Where(c => c.Id == categoryId)
            .Select(c => new CategoryCardViewModel 
            {
                Id = c.Id,
                Name = c.Name,
                ImagePath = c.ImagePath!,
                Description = c.Description
            })
            .FirstOrDefaultAsync();

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

        var categoryToEdit = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == model.Id);

        categoryToEdit!.Name = model.Name;
        categoryToEdit.Description = model.Description;
        categoryToEdit.ImagePath = model.ImagePath;

        _context.Categories.Update(categoryToEdit);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(CategoryIndex));
    }

    public async Task<IActionResult> DeleteCategory(int categoryId) 
    {
        var categoryToDelete = await _context.Categories.FindAsync(categoryId);

        if (categoryToDelete == null)
        {
            return NotFound();
        }

        categoryToDelete.IsDeleted = true;
        _context.Categories.Update(categoryToDelete);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(CategoryIndex));

    }
}
