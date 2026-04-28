using System.Security.Cryptography;
using System.Text;
using Finlay.PharmaVigilance.Application.IServices;

namespace Finlay.PharmaVigilance.Application.Services;

public class NotificationNumberGenerator : INotificationNumberGenerator
{
    /// <inheritdoc />
    public string Generate()
    {
        var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
        var randomPart = GenerateSecureCode(8);

        return $"AEFI-{datePart}-{randomPart}";
    }

    /// <summary>
    /// Generates a cryptographically secure random alphanumeric string.
    /// </summary>
    /// <param name="length">Desired length of the generated string.</param>
    /// <returns>A secure random string composed of uppercase letters and digits.</returns>
    private string GenerateSecureCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var bytes = new byte[length];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        var result = new StringBuilder(length);

        foreach (var b in bytes)
        {
            result.Append(chars[b % chars.Length]);
        }

        return result.ToString();
    }
}