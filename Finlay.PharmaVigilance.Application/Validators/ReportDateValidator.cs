using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.Helpers;

namespace Finlay.PharmaVigilance.Application.Validators;

/// <summary>
/// Validates that the report date is not in the future.
/// All date validations use Eastern Time (UTC-5) as the reference timezone.
/// </summary>
public class ReportDateValidator : IReportValidator<ReportDto>
{
    /// <summary>
    /// Validates that the report date is less than or equal to the current date in Eastern Time (UTC-5).
    /// </summary>
    /// <param name="reportDto">The report data to validate</param>
    /// <exception cref="ArgumentException">Thrown when report date is in the future</exception>
    public Task ValidateAsync(ReportDto reportDto)
    {
        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto));

        if (!reportDto.ReportDate.HasValue)
            throw new ArgumentException("Report date is required.", nameof(reportDto.ReportDate));

        var easternNow = TimeZoneHelper.GetEasternNow();

        if (reportDto.ReportDate > easternNow)
            throw new ArgumentException(
                "Report date cannot be in the future. The report date must be less than or equal to the current date (Eastern Time UTC-5).",
                nameof(reportDto.ReportDate));

        return Task.CompletedTask;
    }
}
