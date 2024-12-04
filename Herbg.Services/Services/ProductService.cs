using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Services;

public class ProductService(IRepositroy<Product> product) : IProductService
{
    private readonly IRepositroy<Product> _product = product;

    public async Task<ICollection<ProductCardViewModel>> GetAllProductsAsync()
    {
        var products = await _product
             .GetAllAttachedAsync()
             .Select(p => new ProductCardViewModel
             {
                 Id = p.Id,
                 Name = p.Name,
                 Description = p.Description,
                 ImagePath = p.ImagePath,
                 Price = p.Price
             })
             .ToArrayAsync(); 
        return products;
    }

    public async Task<Product> GetProductByIdAync(int productId)
    {
        var productToAdd = await _product.FindByIdAsync(productId);

        return productToAdd!;
    }
}
