using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Common.ValidationConstants;

public static  class Company
{
    public const int CompanyNameMaxLength = 40;
    public const int CompanyNameMinLength = 2;
    public const string CompanyNameLengthErrorMessage = "Company name must be between 2 and 40 characters long";

    public const int CompanyTaxMaxLength = 15;
    public const string CompanyTaxValidationRegex = "^[A-Z]{0,2}[A-Za-z0-9]{9,15}$";
    public const string CompanyTaxErrorMessage = "Tax must start with 2 characters and followed by 7 to 13 digits";

    public const int CompanyAddressMaxLength = 250;
    public const int CompanyAddressMinLength = 10;
    public const string CompanyAddressLengthError = "Company must be between 10 and 250 chgaracters long";
}
