using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.Company;

namespace Herbg.Models;

public class Company
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(CompanyNameMaxLength, ErrorMessage = CompanyNameLengthErrorMessage)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(CompanyTaxMaxLength, ErrorMessage = CompanyTaxErrorMessage)]
    [RegularExpression(CompanyTaxValidationRegex)]
    public string Tax { get; set; } = null!;

    [Required]
    [MaxLength(CompanyAddressMaxLength,ErrorMessage = CompanyAddressLengthError)]
    public string Address { get; set; } = null!;

    public virtual ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
}
