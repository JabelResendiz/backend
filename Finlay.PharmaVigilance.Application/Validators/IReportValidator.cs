
namespace Finlay.PharmaVigilance.Application.Validators;

/// <summary>
/// Interface for validating AEFI reports with business rule enforcement.
/// Each implementation validates a specific aspect of the report.
/// </summary>
public interface IReportValidator<T>
{
    /// <summary>
    /// Validates a public AEFI report asynchronously.
    /// </summary>
    /// <param name="reportDto">The report data to validate</param>
    /// <exception cref="ArgumentException">Thrown when validation fails</exception>
    /// <exception cref="KeyNotFoundException">Thrown when referenced entities don't exist</exception>
    /// <exception cref="InvalidOperationException">Thrown for other validation failures</exception>
    Task ValidateAsync(T reportDto);
}
