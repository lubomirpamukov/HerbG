using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.Product;

namespace Herbg.Models;

public class Product
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(ProductNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [Range(ProductPriceMinvalue, ProductPriceMaxValue, ErrorMessage = ProductPriceErrorMessage)]
    public decimal Price { get; set; }

    [Required]
    public string ImagePath { get; set; } = null!;

    [Required]
    [MaxLength(ProductDescriptionMaxLength)]
    public string Description { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Manufactorer))]
    public int ManufactorerId { get; set; }

    [Required]
    public virtual Manufactorer Manufactorer { get; set; } = null!;

    public bool IsDeleted { get; set; }

    [Required]
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
