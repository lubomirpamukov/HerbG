using Herbg.ViewModels.Wishlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Interfaces;

public interface IWishlistService
{
    public Task<IEnumerable<WishlistItemViewModel>> GetClientWishlistAsync(string clientId);

    public Task<bool> AddToWishlistAsync(string clientId, int productId);

    public Task<bool> RemoveFromWishlistAsync(string clientId, int productId);

    public Task<bool> MoveToCartAsync(string clientId, int productId);

    public Task<int> GetWishlistItemCountAsync(string clientId);
}
