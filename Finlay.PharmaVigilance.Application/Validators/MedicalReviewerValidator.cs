using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.Helpers;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.Validators;

/// <summary>
/// Validates adverse event information including symptom existence, date consistency, and death-related data.
/// </summary>
public class MedicalReviewerValidator : IReportValidator<RegisterMedicalReviewerDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public MedicalReviewerValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task ValidateAsync(RegisterMedicalReviewerDto reportDto)
    {
        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto), "MedicalReviewer information is required.");

        if (!reportDto.DateOfBirth.HasValue)
            throw new ArgumentException("MedicalReviewer Date of Birth is required.", nameof(reportDto));

        var easternNow = TimeZoneHelper.GetEasternNow();

        if (reportDto.DateOfBirth > easternNow)
            throw new ArgumentException(
                "Medical Reviewer Date Of Birth cannot be in the future.",
                nameof(reportDto.DateOfBirth));


        ValidateIdentityNumberFormat(reportDto.IdentityNumber, reportDto.DateOfBirth);

        var existingMedical = await _unitOfWork.GetRepository<MedicalReviewer>()
                                    .FirstOrDefaultAsync(mr => mr.IdentityNumber == reportDto.IdentityNumber);


        if (existingMedical != null)
            throw new InvalidOperationException("IdentityNumber is already taken");


    }


    private static void ValidateIdentityNumberFormat(string identityNumber, DateTime? dateOfBirth)
    {
        // Basic validation: Identity number should have a reasonable length
        if (identityNumber.Length != 11 || !identityNumber.All(char.IsDigit))
            throw new ArgumentException(
                "Identity number must be 11 digits",
                nameof(identityNumber));

        string yy = identityNumber.Substring(0, 2);
        string mm = identityNumber.Substring(2, 2);
        string dd = identityNumber.Substring(4, 2);

        int year = int.Parse(yy);
        int month = int.Parse(mm);
        int day = int.Parse(dd);

        int currentYearTwoDigits = DateTime.Now.Year % 100;
        int fullYear = (year > currentYearTwoDigits) ? 1900 + year : 2000 + year;

        DateTime extractedDate;

        try
        {
            extractedDate = new DateTime(fullYear, month, day);
        }
        catch
        {
            throw new ArgumentException("Invalid date encoded in identity number.");
        }

        if (extractedDate.Date != dateOfBirth?.Date)
        {
            throw new ArgumentException("Date of birth does not match identity number.");
        }

    }
}
