using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.Manufactorer;

namespace Herbg.Models;

public class Manufactorer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ManufactorerNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(ManufactorerAddressMaxLength)]
    public string Address { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
