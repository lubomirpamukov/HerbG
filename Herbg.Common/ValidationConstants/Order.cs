using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Common.ValidationConstants;

public static class Order
{
    public const int OrderAddressMaxLength = 250;
    public const int OrderAddressMinLength = 10;
    public const string OrderAddressLengthError = "Order must be between 10 and 250 chgaracters long";
}
