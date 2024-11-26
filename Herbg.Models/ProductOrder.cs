using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Models;

public class ProductOrder
{
    public string OrderId { get; set; } = null!;
    public virtual Order Order { get; set; } = null!;

    public string ProductId { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
