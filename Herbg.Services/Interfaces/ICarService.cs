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
}
