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
            ManufactorerId = product.ManufactorerId,  // Set ManufacturerId
                                                      // Other properties...
        };

        // Pass categories and manufacturers to ViewBag
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
        ViewBag.Manufacturers = new SelectList(_context.Manufactorers, "Id", "Name", model.ManufactorerId);

        return View(model);
    }


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

        //Checking for image file
        if (model.ImageFile!= null)
        {
            //If it dosnt exist
            var fileName = $"{Guid.NewGuid}_{model.ImageFile.FileName}"; // creating a unique name

            //Creating path to the wwwroot
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "/wwwroot/images/products", fileName);

            //Open a stream to transfer the file binary to the wwwroot/images/products directory
            using (var stream = new FileStream(filePath, FileMode.Create)) 
            {
                await model.ImageFile.CopyToAsync(stream);
            }

            productToEdit.ImagePath = $"/images/products/{fileName}";
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Product", new { area = "Admin" });
    }

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

    [HttpPost]
    public async Task<IActionResult> AddProduct(CreateProductViewModel model, IFormFile ImageFile)
    {
        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            // Repopulate dropdowns in case of validation errors
            ViewBag.Manufacturers = await _context.Manufactorers
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name,
                })
                .ToListAsync();

            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
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
        };

        // Check if the image file is not null and save the image
        if (model.ImageFile != null)
        {
            try
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.ImageFile.FileName)}";
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                newProduct.ImagePath = $"/images/products/{fileName}";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while uploading the image.");
                // Log the exception (use a logger like Serilog, NLog, etc.)
                Console.WriteLine(ex.Message);
                return View(model);
            }

        }

        // Add the new product to the database
        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();

        // Redirect to the product index page after adding the product
        return RedirectToAction("Index", "Product", new { area = "Admin" });
    }

}
