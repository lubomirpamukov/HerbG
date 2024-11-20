using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Common.ValidationConstants;

public static class Manufactorer
{
    public const int ManufactorerNameMaxLength = 100;
    public const int ManufactorerNameMinLength = 4;
    public const string ManufactorerNameErrorMessage = "Manufactorer name must be between 4 and 100 characters";

    public const int ManufactorerAddressMaxLength = 250;
    public const int ManufactorerAddressMinLength = 10;
    public const string ManufactorerAddressErrorMessage = "Manufactorer address must be between 10 and 250 characters long";


}
