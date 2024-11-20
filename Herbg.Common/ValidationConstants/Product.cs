using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Common.ValidationConstants;

public static class Product
{
    public const int ProductNameMaxLength = 100;
    public const int ProductNameMinLength = 4;
    public const string ProductNameErrorMessage = "Product name must be between 4 and 100 characters";

    public const double ProductPriceMaxValue = 10000;
    public const double ProductPriceMinvalue = 4;
    public const string ProductPriceErrorMessage = "Price range must be between 4$ and 10000$";

    public const int ProductDescriptionMaxLength = 500;
    public const int ProductDescriptionMinLength = 10;
    public const string ProducDescriptionErrorMessage = "Description length must be between 10 and 500 characters";

    
}
