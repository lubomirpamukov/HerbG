using Herbg.Data;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers;

public class CategoryController(ICategoryService category) : Controller
{
    private readonly ICategoryService _category = category;
    
    public async Task<IActionResult> Index()
    {
        var categories = await _category.GetAllCategoriesAsync();
        return View(categories);
    }
}
