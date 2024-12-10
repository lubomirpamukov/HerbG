using Azure;
using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Product;
using Herbg.ViewModels.Review;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Services;

public class ProductService(
    IRepository<Product> product,
    ICategoryService category,
    IManufactorerService manufactorer) : IProductService
{
    private readonly IRepository<Product> _product = product;
    private readonly ICategoryService _categoryService = category;
    private readonly IManufactorerService _manufactorerService = manufactorer;

    public IQueryable<Product> ApplyFilters(IQueryable<Product> query, string? searchQuery, string? category, string? manufactorer)
    {
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.ToLower().Trim();
            query = query.Where(p => p.Name.ToLower().Contains(searchQuery));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            category = category.ToLower().Trim();
            query = query.Where(p => p.Category != null && p.Category.Name.ToLower().Contains(category));
        }

        if (!string.IsNullOrWhiteSpace(manufactorer))
        {
            manufactorer = manufactorer.ToLower().Trim();
            query = query.Where(p => p.Manufactorer != null && p.Manufactorer.Name.ToLower().Contains(manufactorer));
        }

        return query;
    }

    public async Task<(ICollection<ProductCardViewModel> Products, int TotalPages, IEnumerable<string> Categories, IEnumerable<string> Manufactorers)>
     GetAllProductsAsync(
         string? searchQuery = null,
         string? category = null,
         string? manufactorer = null,
         int pageNumber = 1,
         int pageSize = 3)
    {
        // Base query with includes
        var productsQuery = _product
            .GetAllAttached()
            .Include(p => p.Category)
            .Include(p => p.Manufactorer)
            .Where(p => !p.IsDeleted);

        // Apply filtering
        productsQuery = ApplyFilters(productsQuery, searchQuery, category, manufactorer);

        // Calculate total count and pages
        int totalProducts = await productsQuery.CountAsync();
        int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

        // Apply pagination
        var pagedProducts = productsQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        // Map to view model
        var productViewModels = await pagedProducts
            .Select(p => new ProductCardViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImagePath = p.ImagePath ?? "default-image-path.jpg",
                Price = p.Price
            })
            .ToArrayAsync();

        // Retrieve categories and manufacturers
        var categories = await _categoryService.GetCategoriesNamesAsync();
        var manufactorers = await _manufactorerService.GetManufactorersNamesAsync();

        // Return data
        return (productViewModels, totalPages, categories, manufactorers);
    }

    public async Task<ICollection<ProductCardViewModel>> GetHomePageProductsAsync()
    {
        return await _product
            .GetAllAttached()
            .Select(p => new ProductCardViewModel
            {
                Id = p.Id,
                Name = p.Name,
                ImagePath = p.ImagePath,
                Description = p.Description,
                Price = p.Price
            }).ToArrayAsync();

        
    }

    public async Task<Product> GetProductByIdAsync(int productId)
    {
        var productToAdd = await _product.FindByIdAsync(productId);

        return productToAdd!;
    }

    

    public async Task<ProductDetailsViewModel> GetProductDetailsAsync(int productId)
    {
        var product = await _product
            .GetAllAttached()
            .Where(p => p.Id == productId)
            .Include(p => p.Category)
            .Include(p => p.Manufactorer)
            .Include(p => p.Reviews)
            .ThenInclude(r => r.Client)
            .Select(p => new ProductDetailsViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category.Name,
                Description = p.Description,
                ImagePath = p.ImagePath!,
                Price = p.Price,
                Manufactorer = p.Manufactorer.Name,
                Reviews = p.Reviews.Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Description = r.Description!,
                    Rating = r.Rating,
                    ReviewerName = r.Client.UserName!

                }).ToList()
            })
            .FirstOrDefaultAsync();

        return product;
    }

    public async Task<ICollection<ProductCardViewModel>> GetProductsByCategoryAsync(int categoryId)
    {
        var productsByCategory = await _product
            .GetAllAttached()
            .Where(p => p.CategoryId == categoryId && p.IsDeleted == false)
            .ToArrayAsync();

        var viewModelCollection = new List<ProductCardViewModel>();
        foreach (var product in productsByCategory)
        {
            var newProduct = new ProductCardViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImagePath = product.ImagePath!,
                Price = product.Price
            };

            viewModelCollection.Add(newProduct);
        }
        return viewModelCollection;
    }
}
