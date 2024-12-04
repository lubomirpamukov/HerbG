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

    public Task<ICollection<ProductCardViewModel>> GetAllProductsAsync();
}
