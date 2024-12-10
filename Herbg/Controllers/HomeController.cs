using System.Diagnostics;
using Herbg.Data;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Category;
using Herbg.ViewModels.Home;
using Herbg.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers;

public class HomeController(ICategoryService category, IProductService product) : Controller
{
    private readonly ICategoryService _category = category;
    private readonly IProductService _product = product;

    public async Task<IActionResult> Index()
    {
        var categoreis = await _category.GetAllCategoriesAsync();
        var products = await _product.GetHomePageProductsAsync();
       
        var viewModel = new CategoryProductHomeViewModel
        {
            Categories = categoreis,
            Products = products
        };

        return View(viewModel);
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact() 
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
