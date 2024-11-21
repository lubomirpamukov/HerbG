using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Common.ValidationConstants;

public static class Reviews
{
    public const int ReviewDescriptionMaxLength = 500;
    public const string ReviewDescriptionLengthError = "Review description must not exceed 500 characters";

    public const int RatingMaxValue = 5;
    public const int RatingMinValue = 1;
    public const string RatingErrorMessage = "Rating must be between 1 and 5";
}
