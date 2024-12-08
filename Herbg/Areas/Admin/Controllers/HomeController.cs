using Herbg.Models;
using Herbg.Data;
using Herbg.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace Herbg.Areas.Admin.Controllers;

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

    //Refactor to work with service and write tests
    public async Task<IActionResult> DeleteProduct(int productId) 
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

}
