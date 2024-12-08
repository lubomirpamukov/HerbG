namespace Herbg.Common.ValidationConstants;

public static class ApplicationUser
{
    public const int UserAddressMaxLength = 250;
    public const int UserAddressMinLength = 4;
    public const string UserAddressErrorMessage = "Address must be between 4 and 250 characters long";

    public const int FullNameMaxLength = 100;
    public const int FullNameMinLength = 5;
    public const string FullNameLengthErrorMessage = "Full name must be between 5 and 100 characters long.";

    public const int CityNameMaxLength = 50;
    public const int CityNameMinLength = 2;
    public const string CityNameLengthErrorMessage = "City name must be between 2 and 50 characters long.";

    public const string ZipCodeRequiredMessage = "Zip code is required.";
    public const string ZipCodeFormatMessage = "Invalid zip code format. Expected format: 12345 or 12345-6789.";

    public const int CountryNameMaxLength = 50;
    public const int CountryNameMinLength = 3;
    public const string CountryNameErrorMessage = "Country name must be between 3 and 50 chacarters long.";
}
