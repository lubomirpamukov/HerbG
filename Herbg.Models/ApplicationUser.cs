using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.ApplicationUser;

namespace Herbg.Models;

public class ApplicationUser : IdentityUser
{
    // Shipping Address
    [MaxLength(UserAddressMaxLength, ErrorMessage = UserAddressErrorMessage)]
    public string? ShippingInformationAddress { get; set; }

    // Full Name (Optional but can be validated)
    [MaxLength(FullNameMaxLength, ErrorMessage = FullNameLengthErrorMessage)]
    public string? ShippingInformationFullName { get; set; }

    // City (Optional but can be validated)
    [MaxLength(CityNameMaxLength, ErrorMessage = CityNameLengthErrorMessage)]
    public string? ShippingInformationCity { get; set; }

    // Zip Code (Required, with a custom length range and format for validation)
    [Required(ErrorMessage = ZipCodeRequiredMessage)]
    [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = ZipCodeFormatMessage)]
    public string? ShippingInformationZip { get; set; }

    // Country (Optional but can be validated)
    [MaxLength(CountryNameMaxLength, ErrorMessage = CountryNameErrorMessage)]
    public string? ShippingInformationCountry { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

    public virtual ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Cart Cart { get; set; } = new Cart();
}
