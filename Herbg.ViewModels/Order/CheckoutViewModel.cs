using Herbg.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.CheckoutViewModel;

namespace Herbg.ViewModels.Order;

public class CheckoutViewModel
{
    [Required(ErrorMessage = FullNameRequiredMessage)]
    [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength, ErrorMessage = FullNameLengthErrorMessage)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = AddressRequiredMessage)]
    [StringLength(AddressMaxLength, MinimumLength = AddressMinLength, ErrorMessage = AddressLengthErrorMessage)]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = CityRequiredMessage)]
    [StringLength(CityMaxLength, MinimumLength = CityMinLength, ErrorMessage = CityLengthErrorMessage)]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = ZipCodeRequiredMessage)]
    [RegularExpression(@"^\d{4,10}$", ErrorMessage = ZipCodeLengthErrorMessage)]
    public string ZipCode { get; set; } = string.Empty;

    [Required(ErrorMessage = CountryRequiredMessage)]
    [StringLength(CountryMaxLength, ErrorMessage = CountryLengthErrorMessage)]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = PaymentMethodRequiredMessage)]
    public string PaymentMethod { get; set; } = string.Empty;

    [StringLength(CardNumberMaxLength, MinimumLength = CardNumberMinLength, ErrorMessage = CardNumberErrorMessage)]
    [RegularExpression(@"^\d{13,16}$", ErrorMessage = CardNumberRegexErrorMessage)]
    public string? CardNumber { get; set; }

    [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = ExpiryDateRegexErrorMessage)]
    public string? ExpiryDate { get; set; }

    [StringLength(CVVLength, MinimumLength = CVVLength, ErrorMessage = CVVErrorMessage)]
    [RegularExpression(@"^\d{3}$", ErrorMessage = CVVErrorMessage)]
    public string? CVV { get; set; }

    public List<CartItemViewModel> CartItems { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Total { get; set; }
}
