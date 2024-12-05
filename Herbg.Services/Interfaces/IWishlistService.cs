﻿using Herbg.ViewModels.Wishlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Interfaces;

public interface IWishlistService
{
    public Task<IEnumerable<WishlistItemViewModel>> GetClientWishlistAsync(string clientId);
}
