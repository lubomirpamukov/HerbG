using Herbg.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Interfaces;

public interface ICarService
{
    public Task<CartViewModel> GetUserCartAsync(string clientId);

    public Task<bool> RemoveCartItemAsync(string clientId, int productId);

    public Task<int> GetCartItemsCountAsync(string clientId);
}
