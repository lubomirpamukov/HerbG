using Azure;
using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Product;
using Herbg.ViewModels.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Services;

public class ProductService(IRepository<Product> product) : IProductService
{
    private readonly IRepository<Product> _product = product;

    public async Task<(ICollection<ProductCardViewModel> Movies, int totalPages)> GetAllProductsAsync(
        string? searchQuery = null,
        string? category = null,
        string? manufactorer = null,
        int pageNumber = 1, int pageSize = 3)
    {
        var products = _product
            .GetAllAttached()
            .Include(p => p.Category)
            .Include(p => p.Manufactorer)
            .Where(p => !p.IsDeleted);

        // Apply search query filter
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.ToLower().Trim();
            products = products.Where(p => p.Name.ToLower().Contains(searchQuery));
        }

        // Apply category filter
        if (!string.IsNullOrWhiteSpace(category))
        {
            category = category.ToLower().Trim();
            products = products.Where(p =>
                p.Category != null &&
                p.Category.Name.ToLower().Contains(category));
        }

        // Apply manufacturer filter
        if (!string.IsNullOrWhiteSpace(manufactorer))
        {
            manufactorer = manufactorer.ToLower().Trim();
            products = products.Where(p =>
                p.Manufactorer != null &&
                p.Manufactorer.Name.ToLower().Contains(manufactorer));
        }

        //Calculate the total number of pages
        int totalProducts = await products.CountAsync();
        int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

        //apply Pagination
        products = products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var productView = await products
            .Select(p => new ProductCardViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImagePath = p.ImagePath ?? "default-image-path.jpg",
                Price = p.Price
            })
            .ToArrayAsync();


        return (productView, totalPages);
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
