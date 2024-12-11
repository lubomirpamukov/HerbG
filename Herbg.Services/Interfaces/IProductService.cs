using Herbg.Models;
using Herbg.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Interfaces;

public interface IProductService
{
    public Task<Product> GetProductByIdAsync(int productId);

    public Task<(ICollection<ProductCardViewModel> Products, int TotalPages, IEnumerable<string> Categories, IEnumerable<string> Manufactorers)>
      GetAllProductsAsync(
          string? searchQuery = null,
          string? category = null,
          string? manufactorer = null,
          int pageNumber = 1,
          int pageSize = 3);

    public Task<ProductDetailsViewModel> GetProductDetailsAsync(int productId);

    public Task<ICollection<ProductCardViewModel>> GetProductsByCategoryAsync(int categoryId);

    public abstract IQueryable<Product> ApplyFilters(
    IQueryable<Product> query,
    string? searchQuery,
    string? category,
    string? manufactorer);

    public Task<ICollection<ProductCardViewModel>> GetHomePageProductsAsync();

    public Task<bool> SoftDeleteProductAsync(int productId);

    public Task<CreateProductViewModel?> GetProductForEditAsync(int productId);

    public Task<bool> UpdateProductAsync(CreateProductViewModel model);

    public Task<bool> AddProductAsync(CreateProductViewModel model);
}
