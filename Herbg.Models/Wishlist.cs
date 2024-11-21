using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Models;

public class Wishlist
{
    [Key] 
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Client))]
    public string ClientId { get; set; } = null!;

    public virtual ApplicationUser Client { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Product))]
    public string ProductId { get; set; } = null!;
    public Product Product { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime AddedOn { get; set; } = DateTime.UtcNow;
}
