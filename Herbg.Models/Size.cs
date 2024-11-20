using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herbg.Common.Enums;

namespace Herbg.Models;

public class Size
{
    [Key]
    public int Id { get; set; }

    public ProductSizeEnum ProductSize { get; set; } = null!;

}
