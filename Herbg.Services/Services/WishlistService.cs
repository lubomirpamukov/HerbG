using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Wishlist;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Services;

public class WishlistService(IRepositroy<Wishlist>wishlist) : IWishlistService
{
    private readonly IRepositroy<Wishlist> _wishlist = wishlist;
    public async Task<IEnumerable<WishlistItemViewModel>> GetClientWishlistAsync(string clientId)
    {
        //Check client wishlist
        var clientWishlists = await _wishlist
            .GetAllAttachedAsync()
            .Where(w => w.ClientId == clientId)
            .Include(w => w.Product)
            .Select(c => new WishlistItemViewModel
            {
                Id = c.ProductId,
                Description = c.Product.Description,
                ImagePath = c.Product.ImagePath,
                Name = c.Product.Name,
                Price = c.Product.Price
            })
            .ToArrayAsync();

        return clientWishlists;
    }
}
