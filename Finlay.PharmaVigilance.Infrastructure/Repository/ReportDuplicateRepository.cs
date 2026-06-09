using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class ReportDuplicateRepository : GenericRepository<ReportDuplicate>, IReportDuplicateRepository
{
    private readonly IMapper _mapper;
    public ReportDuplicateRepository(FinlayDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public async Task<ReportDuplicateDetailDto> GetDetailByIdAsync(Guid id)
    {
        return await _entity
        .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<ReportDuplicateDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException("Duplicate not found.");
    }


    public async Task<PagedResultDto<ReportDuplicateDto>> GetPendingAsync(
    Guid userId,
    PagedRequestDto paged)
    {
        var sectionResponsible = await _context.SectionResponsibles
            .FirstOrDefaultAsync(sr => sr.UserId == userId)
            ?? throw new UnauthorizedAccessException("User is not a section responsible.");

        var query = _context.ReportDuplicates
            .Where(d =>
                d.EnumReportDuplicate == EnumReportDuplicate.IsPossibleDuplicate &&
                _context.Alerts.Any(a =>
                    a.IsActive &&
                    a.SectionResponsibleId == sectionResponsible.Id &&
                    a.AefiReportId == d.AefiReportOriginalId));

        var totalCount = await query.CountAsync();

        var items = await GetPaged(query, (paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
            .Select(d => new ReportDuplicateDto
            {
                Id = d.Id,
                AefiReportOriginalId = d.AefiReportOriginalId,
                AefiReportCopyId = d.AefiReportCopyId,
                SubjectName = d.AefiReportOriginal.VaccinatedSubject.FullName,
                OriginalReportDate = d.AefiReportOriginal.ReportDate,
                CopyReportDate = d.AefiReportCopy.ReportDate,
                EnumReportDuplicate = d.EnumReportDuplicate,
                OriginalReportStatus = d.AefiReportOriginal.Status,

                MedicalReviewerName =
                    _context.MedicalReviewAssignments
                        .Where(m => m.AefiReportId == d.AefiReportOriginalId)
                        .Select(m => m.MedicalReviewer.User.UserName)
                        .FirstOrDefault()
            })
            .ToListAsync();

        // 5. Resultado final paginado
        return new PagedResultDto<ReportDuplicateDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalCount
                ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                : null,
            PreviousPageUrl = paged.PageNumber > 1
                ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                : null
        };
    }


    public async Task<AefiReport?> ValidateDuplicate(AefiReport report)
    {
        // 1. Normalizar input (en memoria)
        var incomingPairs = report.Vaccinations
            .Select(v => new
            {
                VaccineId = v.Lot.VaccineId,
                v.AdministrationDate
            })
            .ToList();

        // 2. Query base optimizada (solo filtro estructural SQL)
        var candidatesQuery = _context.AefiReport
            .Where(ar =>
                ar.VaccinatedSubject.IdentityNumber.Value ==
                report.VaccinatedSubject.IdentityNumber.Value &&
                ar.Id != report.Id)
            .Include(ar => ar.Vaccinations)
                .ThenInclude(v => v.Lot);

        // 3. Traer candidatos a memoria (controlado)
        var candidates = await candidatesQuery.ToListAsync();

        // 4. Evaluación de dominio en memoria (correcta y segura)
        var duplicate = candidates.FirstOrDefault(ar =>
            ar.Vaccinations.Any(v =>
                incomingPairs.Any(ip =>
                    ip.VaccineId == v.Lot.VaccineId &&
                    ip.AdministrationDate == v.AdministrationDate
                )
            )
        );

        return duplicate;
    }

}