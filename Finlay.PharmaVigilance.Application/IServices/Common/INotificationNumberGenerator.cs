namespace Finlay.PharmaVigilance.Application.IServices;

/// <summary>
/// Provides functionality to generate unique and secure notification numbers
/// for AEFI (Adverse Events Following Immunization) reports.
/// </summary>
/// <remarks>
/// The generated notification number is intended to:
/// - Uniquely identify a report.
/// - Be safe for public use (non-predictable).
/// - Avoid sequential or guessable patterns.
/// 
/// Format example:
/// AEFI-YYYYMMDD-XXXXXXXX
/// Where:
/// - "AEFI" is a fixed prefix.
/// - "YYYYMMDD" represents the UTC date of generation.
/// - "XXXXXXXX" is a cryptographically secure random alphanumeric string.
/// </remarks>
public interface INotificationNumberGenerator
{
    /// <summary>
    /// Generates a unique, non-predictable notification number for an AEFI report.
    /// </summary>
    /// <returns>
    /// A string representing the generated notification number.
    /// </returns>
    string Generate();
}