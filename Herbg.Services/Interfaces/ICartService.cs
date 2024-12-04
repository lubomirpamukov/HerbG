using Herbg.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Interfaces;

public interface ICartService
{
    public Task<CartViewModel> GetUserCartAsync(string clientId);

    public Task<bool> RemoveCartItemAsync(string clientId, int productId);

    public Task<int> GetCartItemsCountAsync(string clientId);

    public Task<bool>AddItemToCartAsync(string clientId, int productId, int quantity);

    public Task<bool> UpdateCartItemQuantityAsync(string clientId, int productId, int quantity);
}
