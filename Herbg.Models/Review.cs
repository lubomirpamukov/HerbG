using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.Reviews;

namespace Herbg.Models;

public class Review
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Client))]
    public string ClientId { get; set; } = null!;

    public virtual ApplicationUser Client { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    [MaxLength(ReviewDescriptionMaxLength, ErrorMessage = ReviewDescriptionLengthError)]
    public string? Description { get; set; }

    [Range(RatingMinValue, RatingMaxValue, ErrorMessage = RatingErrorMessage)]
    public int Rating { get; set; }

    public bool IsDeleted { get; set; }
}
