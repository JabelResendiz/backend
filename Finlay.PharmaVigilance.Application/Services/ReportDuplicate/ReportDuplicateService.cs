using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.Services;

public class ReportDuplicateService : IReportDuplicateService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportDuplicateService(
        IUnitOfWork unitOfWork
    )
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<AefiReport?> ValidateDuplicate(AefiReport report)
    {

        var reportOrigin = _unitOfWork.GetRepository<AefiReport>()
                                .GetAllByItems(
                                    ar => ar.VaccinatedSubject.IdentityNumber.Value == report.VaccinatedSubject.IdentityNumber.Value &&
                                    ar.Reporter.IdentityNumber.Value == report.Reporter.IdentityNumber.Value &&
                                    ar.ReportDate == report.ReportDate)
                                .FirstOrDefault();

        return reportOrigin;

    }

    public async Task CreateAsync(ReportDuplicate report)
    {
        try
        {
            await _unitOfWork.GetRepository<ReportDuplicate>()
                    .CreateAsync(report);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"{ex}");
        }
    }
}