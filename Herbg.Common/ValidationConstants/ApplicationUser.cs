namespace Herbg.Common.ValidationConstants;

public static class ApplicationUser
{
    public const int UserAddressMaxLength = 250;
    public const int UserAddressMinLength = 4;
    public const string UserAddressErrorMessage = "Address must be between 4 and 250 characters long";
}
