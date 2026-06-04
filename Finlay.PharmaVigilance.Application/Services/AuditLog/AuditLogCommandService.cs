using AutoMapper;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.DTO;

public class AuditLogCommandService : IAuditLogCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuditLogCommandService> _logger;
    private readonly IMapper _mapper;
    public AuditLogCommandService(
        IUnitOfWork unitOfWork,
        ILogger<AuditLogCommandService> logger,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<AuditLogDto> CreateAsync(AuditLogDto dto)
    {

        try
        {
            var auditLog = _mapper.Map<AuditLog>(dto);

            var log = await _unitOfWork.GetRepository<AuditLog>()
                        .CreateAsync(auditLog);

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw new ArgumentNullException("asdasda");
        }


    }

    public async Task<AuditLogDto> UpdateAsync(AuditLogDto dto)
    {
        try
        {
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");

            throw new ArgumentException($"{ex}");
        }
    }

    public async Task DeleteAsync<Guid>(Guid dtoId)
    {
        try
        {
            var result = await _unitOfWork.GetRepository<AuditLog>()
                        .GetByIdAsync(dtoId);

            if (result == null)
                throw new ArgumentNullException($"No se encontró la auditLog");

            await _unitOfWork.GetRepository<AuditLog>()
                .DeleteByIdAsync(dtoId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");

            throw new ArgumentException($"{ex}");
        }
    }
}