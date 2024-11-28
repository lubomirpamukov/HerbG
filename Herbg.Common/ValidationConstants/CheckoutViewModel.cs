using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Common.ValidationConstants;

public class CheckoutViewModel
{
    // Full Name
    public const int FullNameMaxLength = 100;
    public const int FullNameMinLength = 10;
    public const string FullNameLengthErrorMessage = "Full Name must be between 10 and 100 characters long.";
    public const string FullNameRequiredMessage = "Full Name is required.";

    // Address
    public const int AddressMaxLength = 250;
    public const int AddressMinLength = 10;
    public const string AddressLengthErrorMessage = "Address must be between 10 and 250 characters long.";
    public const string AddressRequiredMessage = "Address is required.";

    // City
    public const int CityMaxLength = 50;
    public const int CityMinLength = 2;
    public const string CityLengthErrorMessage = "City must be between 2 and 50 characters long.";
    public const string CityRequiredMessage = "City is required.";

    // ZIP Code
    public const string ZipCodeRequiredMessage = "ZIP Code is required.";
    public const string ZipCodeLengthErrorMessage = "ZIP Code must be between 4 and 10 digits.";

    // Country
    public const int CountryMaxLength = 50;
    public const string CountryRequiredMessage = "Country is required.";
    public const string CountryLengthErrorMessage = "Country must not exceed 50 characters.";

    // Payment Method
    public const string PaymentMethodRequiredMessage = "Payment Method is required.";

    // Card Number
    public const int CardNumberMinLength = 13;
    public const int CardNumberMaxLength = 16;
    public const string CardNumberErrorMessage = "Card Number must be between 13 and 16 digits.";
    public const string CardNumberRegexErrorMessage = "Invalid Card Number.";

    // Expiry Date
    public const string ExpiryDateRegexErrorMessage = "Expiry Date must be in MM/YY format.";

    // CVV
    public const int CVVLength = 3;
    public const string CVVErrorMessage = "CVV must be 3 digits.";
}
