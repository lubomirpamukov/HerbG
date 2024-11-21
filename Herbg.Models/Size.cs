using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herbg.Common.Enums;

namespace Herbg.Models;

public class Size
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ProductSize { get; set; } = null!;

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}
