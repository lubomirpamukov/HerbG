using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Models;

public class CartItem
{
    public string CartId { get; set; } = null!;

    [ForeignKey(nameof(CartId))]
    public Cart Cart { get; set; } = null!;

    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal Price { get; set; } 
}

