using Herbg.Common.ValidationConstants;
using Herbg.ViewModels.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.ViewModels.Product;

public class ProductDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string ImagePath { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string Manufactorer { get; set; } = null!;
    public List<ReviewViewModel> Reviews { get; set; } = new();
}
