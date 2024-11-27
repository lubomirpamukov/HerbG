using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.ViewModels.Cart;

public class CartViewModel
{
    public List<CartItemViewModel> CartItems { get; set; } = null!;
}
