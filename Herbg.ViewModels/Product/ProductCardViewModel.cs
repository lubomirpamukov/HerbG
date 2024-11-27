using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.Product;

namespace Herbg.ViewModels.Product;

public class ProductCardViewModel
{
    public int Id { get; set; }
    [Required]
    [StringLength(ProductNameMaxLength, MinimumLength = ProductNameMinLength, ErrorMessage = ProductNameErrorMessage)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(ProductDescriptionMaxLength, MinimumLength = ProductDescriptionMinLength, ErrorMessage = ProducDescriptionErrorMessage)]
    public string Description { get; set; } = null!;

    [Required]
    public string ImagePath { get; set; } = null!;

    [Required]
    [Range(ProductPriceMinvalue, ProductPriceMaxValue, ErrorMessage = ProductPriceErrorMessage)]
    public decimal Price { get; set; }
}
