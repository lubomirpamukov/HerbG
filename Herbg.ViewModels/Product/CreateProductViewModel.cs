using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.Product;

namespace Herbg.ViewModels.Product
{
    public class CreateProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(ProductPriceMinvalue, ProductPriceMaxValue, ErrorMessage = ProductPriceErrorMessage)]
        public decimal Price { get; set; }

        public string? ImagePath { get; set; } = null!;

        [Required]
        [MaxLength(ProductDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public int ManufactorerId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        // Optional: To allow users to assign sizes to do if time is left
        public List<int> ProductSizeIds { get; set; } = new List<int>();
    }
}
