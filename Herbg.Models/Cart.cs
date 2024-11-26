using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Models;

public class Cart
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ClientId { get; set; } = null!;

    [ForeignKey(nameof(ClientId))]
    public ApplicationUser User { get; set; } = null!;

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}

