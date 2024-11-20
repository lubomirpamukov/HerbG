using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Models;

public class ProductSizeEnum
{
    public string ProductId { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public int SizeId { get; set; }

    public virtual ProductSizeEnum Size { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }
}
