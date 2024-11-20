using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Common.ValidationConstants;

public static class CreditCard
{
    public const int CardNumberMaxLength = 16;
    public const int CardNumberMinLength = 13;
    public const string CardNumberErrorMessage = "Card number must be between 13 and 16 digits";

    public const int CardHolderNameMaxLength = 100;
    public const int CardHolderNameMinLength = 2;
    public const string CardHolderNameErrorMessage = "Card holder name must be between 2 and 100 characters";

    public const string CVVErrorMessage = "CVV must be between 3 and 4 digits";
    public const string ExpirationDateFormat = "{0:MM/yyyy}";

}
