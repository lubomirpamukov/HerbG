using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.Category;

namespace Herbg.ViewModels.Category;

public class CategoryCardViewModel
{
    [Required]
    public int Id { get; set; }

    [StringLength(CategoryNameMaxLength,MinimumLength = CategoryNameMinLength , ErrorMessage = CategoryNameErrorMessage)]
    public string Name { get; set; } = null!;
    [StringLength(CategoryImagePathMaxLength, ErrorMessage = CategoryImagePathLenghtErrorMessage)]
    public string ImagePath { get; set; } = null!;
    
    [StringLength(CategoryDescriptionMaxLength, MinimumLength = CategoryDescriptionMinLength, ErrorMessage = CategoryDescriptionLengthErrorMessage)]
    public string Description { get; set; } = null!;
    
}
