using Herbg.Data;
using Herbg.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Select(c => new CategoryCardViewModel 
            {
                Id = c.Id,
                Name = c.Name,
                ImagePath = c.ImagePath,
                Description = c.Description
            })
            .ToArrayAsync();

        return View(categories);
    }
}
