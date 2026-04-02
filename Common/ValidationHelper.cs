using System;
using System.Net.Mail;

namespace EducationCentreSystem.Common;

/// <summary>
/// Centralized validation and normalization helpers.
/// Normalization prepares user input for consistent storage and comparisons.
/// Validation checks whether input satisfies the required format.
/// </summary>
/// <remarks>
/// Keeping these rules in one place avoids duplication between the console UI and the WinForms UI.
/// It also helps ensure consistent behavior when switching repositories (in-memory vs MySQL).
/// </remarks>
public static class ValidationHelper
{
    /// <summary>
    /// Trims email input and returns null when blank.
    /// </summary>
    public static string? NormalizeEmail(string? email)
    {
        var value = (email ?? string.Empty).Trim();
        return value.Length == 0 ? null : value;
    }

    /// <summary>
    /// Validates email format using System.Net.Mail parsing.
    /// </summary>
    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            // Check if the address matches the input to catch partial matches like "user@domain" vs "user@domain.com"
            return string.Equals(addr.Address, email, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Trims and normalizes telephone numbers by removing separators.
    /// Allows either digits only or a leading '+' followed by digits.
    /// Returns null when blank or invalid.
    /// </summary>
    public static string? NormalizeTelephone(string? telephone)
    {
        var value = (telephone ?? string.Empty).Trim();
        if (value.Length == 0) return null;

        var cleaned = value
            .Replace(" ", string.Empty)
            .Replace("-", string.Empty)
            .Replace("(", string.Empty)
            .Replace(")", string.Empty);

        if (cleaned.StartsWith("+", StringComparison.Ordinal))
        {
            if (cleaned.Length == 1) return null;
            for (var i = 1; i < cleaned.Length; i++)
            {
                if (!char.IsDigit(cleaned[i])) return null;
            }
        }
        else
        {
            for (var i = 0; i < cleaned.Length; i++)
            {
                if (!char.IsDigit(cleaned[i])) return null;
            }
        }

        var digitCount = cleaned[0] == '+' ? cleaned.Length - 1 : cleaned.Length;
        if (digitCount < 9 || digitCount > 15) return null;

        return cleaned;
    }

    /// <summary>
    /// Normalizes job type input into either "Full-time" or "Part-time".
    /// Returns null when the input is blank or not recognized.
    /// </summary>
    public static string? NormalizeJobType(string? value)
    {
        var v = (value ?? string.Empty).Trim();
        if (v.Length == 0) return null;

        var key = v.Replace(" ", string.Empty).Replace("-", string.Empty).ToUpperInvariant();
        return key switch
        {
            "FULLTIME" => "Full-time",
            "PARTTIME" => "Part-time",
            _ => null
        };
    }
}
