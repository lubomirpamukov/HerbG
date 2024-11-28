using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.ViewModels.Cart;

public class CartItemViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
