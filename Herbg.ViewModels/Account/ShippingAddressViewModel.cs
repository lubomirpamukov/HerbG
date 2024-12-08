using Herbg.Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Herbg.ViewModels.Account
{
    public class ShippingAddressViewModel
    {
        [Required(ErrorMessage = ApplicationUser.UserAddressErrorMessage)]
        [StringLength(ApplicationUser.UserAddressMaxLength, MinimumLength = ApplicationUser.UserAddressMinLength, ErrorMessage = ApplicationUser.UserAddressErrorMessage)]
        public string? ShippingInformationAddress { get; set; }

        [Required(ErrorMessage = ApplicationUser.FullNameLengthErrorMessage)]
        [StringLength(ApplicationUser.FullNameMaxLength, MinimumLength = ApplicationUser.FullNameMinLength, ErrorMessage = ApplicationUser.FullNameLengthErrorMessage)]
        public string? ShippingInformationFullName { get; set; }

        [Required(ErrorMessage = ApplicationUser.CityNameLengthErrorMessage)]
        [StringLength(ApplicationUser.CityNameMaxLength, MinimumLength = ApplicationUser.CityNameMinLength, ErrorMessage = ApplicationUser.CityNameLengthErrorMessage)]
        public string? ShippingInformationCity { get; set; }

        [Required(ErrorMessage = ApplicationUser.ZipCodeRequiredMessage)]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = ApplicationUser.ZipCodeFormatMessage)]
        public string? ShippingInformationZip { get; set; }

        [Required(ErrorMessage = ApplicationUser.CountryNameErrorMessage)]
        [StringLength(ApplicationUser.CountryNameMaxLength, MinimumLength = ApplicationUser.CountryNameMinLength, ErrorMessage = ApplicationUser.CountryNameErrorMessage)]
        public string? ShippingInformationCountry { get; set; }
    }
}
