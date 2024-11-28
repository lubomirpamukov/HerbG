using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.ViewModels.Cart;

public class CartViewModel
{
    public string Id { get; set; } = null!;
    public List<CartItemViewModel> CartItems { get; set; } = null!;
}
