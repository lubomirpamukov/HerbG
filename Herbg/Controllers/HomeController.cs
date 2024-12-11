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

    public IActionResult Subscribe() 
    {
        //To do: add email collector service
        return View();
    }

    public IActionResult Error(int statusCode)
    {
        ViewData["StatusCode"] = statusCode;

        // If you have specific views (404.cshtml, 500.cshtml), you can return them here.
        if (statusCode == 404)
        {
            return View("404");  // Render the 404 custom page
        }
        else if (statusCode == 500)
        {
            return View("500");  // Render the 500 custom page
        }

        return View("Error");  // Default error page
    }

  
}
