namespace LibraryManagementSystem.Services;

/// Validates ISBN-13 format (13 digits, with optional hyphens). Validates the check digit using the ISBN-13 algorithm.
public static class IsbnValidator
{
    /// Validates that the string is a valid 13-digit ISBN (ISBN-13). Accepts digits only or digits with hyphens (e.g. 978-0-13-235088-4).
    public static bool IsValidIsbn13(string? isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return false;

        // Remove hyphens and spaces
        var digits = isbn.Where(char.IsDigit).ToList();
        if (digits.Count != 13)
            return false;

        // All characters (after removing hyphens) must be digits
        var onlyDigits = string.Concat(digits);
        if (onlyDigits.Length != 13)
            return false;

        // Validate check digit (ISBN-13 uses 1-3-1-3-1-3-1-3-1-3-1-3 weighting)
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            int digit = digits[i] - '0';
            sum += (i % 2 == 0) ? digit : digit * 3;
        }
        int checkDigit = (10 - (sum % 10)) % 10;
        return checkDigit == (digits[12] - '0');
    }

    /// Normalizes ISBN to digits only (no hyphens) for storage/comparison.
    public static string NormalizeIsbn(string isbn) =>
        string.Concat(isbn.Where(char.IsDigit));
}
