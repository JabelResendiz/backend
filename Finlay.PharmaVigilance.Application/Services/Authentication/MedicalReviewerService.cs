using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finlay.PharmaVigilance.Application.Authentication;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Application.Validators;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.Services.Authentication;

/// <summary>
/// Service implementation for managing Medical Reviewer registration and authentication.
/// </summary>
public class MedicalReviewerService : IMedicalReviewerService
{
    private readonly IIdentityManager _identityManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IMedicalReviewerRepository _medical;
    private readonly IEnumerable<IReportValidator<RegisterMedicalReviewerDto>> _validators;
    private readonly ILogger<MedicalReviewerService> _logger;

    // private readonly IEventBus _eventBus;

    private readonly IPublishEndpoint _publishEndpoint;

    /// <summary>
    /// Initializes a new instance of the MedicalReviewerService class.
    /// </summary>
    public MedicalReviewerService(
        IIdentityManager identityManager,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContextService userContextService,
        IMedicalReviewerRepository medical,
        IPublishEndpoint publishEndpoint,
        IEnumerable<IReportValidator<RegisterMedicalReviewerDto>> validators,
        ILogger<MedicalReviewerService> logger)
    {

        _identityManager = identityManager ?? throw new ArgumentNullException(nameof(identityManager)); ;
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService)); ;
        _medical = medical ?? throw new ArgumentNullException(nameof(medical)); ;
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    /// <summary>
    /// Registers a new Medical Reviewer user with their profile information.
    /// </summary>
    public async Task<string> RegisterMedicalReviewerAsync(RegisterMedicalReviewerDto registerDto)
    {
        _logger.LogInformation("Starting public AEFI report creation process");

        // Validate inputs
        if (registerDto == null)
            throw new ArgumentNullException(nameof(registerDto), "Registration DTO cannot be null.");


        try
        {
            foreach (var validator in _validators)
            {
                _logger.LogDebug("Executing {ValidatorCount} validators", _validators.Count());
                await validator.ValidateAsync(registerDto);
            }

            var userId = _userContextService.GetUserId();

            var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                            .FirstOrDefaultAsync(sr => sr.UserId == userId);

            if (sectionResponsible == null)
                throw new UnauthorizedAccessException("User is not a section responsible.");

            var provinceId = sectionResponsible.ProvinceId;
            var municipalityId = sectionResponsible.MunicipalityId;

            var user = _mapper.Map<User>(registerDto);
            user.UserRole = UserRole.MedicalReviewer.ToString();

            var createdUser = await _identityManager.CreateUserAsync(user, registerDto.Password);
            if (createdUser == null)
                throw new InvalidOperationException("Failed to create user account.");

            // Assign MedicalReviewer role
            await _identityManager.AddRoles(createdUser.Id.ToString(), UserRole.MedicalReviewer.ToString());

            var medicalReviewer = _mapper.Map<MedicalReviewer>(registerDto);
            medicalReviewer.UserId = createdUser.Id;
            medicalReviewer.User = createdUser;
            medicalReviewer.ProvinceId = provinceId;
            medicalReviewer.MunicipalityId = municipalityId;
            medicalReviewer.SectionResponsibleId = sectionResponsible.Id;
            medicalReviewer.SectionResponsible = sectionResponsible;

            // Add to repository and save
            await _unitOfWork.GetRepository<MedicalReviewer>().CreateAsync(medicalReviewer);
            await _unitOfWork.CompleteAsync();

            await _publishEndpoint.Publish(new MedicalReviewerRegisteredEvent
            {
                Email = createdUser.Email!,
                FullName = createdUser.UserName!
            });

            return "Medical Reviewer successfully registered";
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Error to register Medical Reviewer: {ex.Message}",
                ex);
        }

    }


    public async Task<IEnumerable<GetMedicalReviewerDto>> ListByMunicipalityAsync(int municipalityId, CancellationToken cancellationToken = default)
    {
        if (municipalityId <= 0 || municipalityId >= 16)
            throw new InvalidOperationException($"Invalid input {municipalityId}");

        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (sectionResponsible == null)
            throw new UnauthorizedAccessException("User is not a section responsible.");

        var provinceId = sectionResponsible.ProvinceId;

        var municipality = await _unitOfWork.GetRepository<Municipality>().GetByIdAsync(municipalityId);
        if (municipality == null || municipality.ProvinceId != provinceId)
            throw new KeyNotFoundException($"Municipality with ID {municipalityId} does not exist or does not belong to the specified province.");

        var medicalList = await _medical.GetByMunicipalityAsync(municipalityId);

        return _mapper.Map<IEnumerable<GetMedicalReviewerDto>>(medicalList);
    }


    public async Task<IEnumerable<GetMedicalReviewerDto>> ListByProvinceAsync(CancellationToken cancellationToken = default)
    {
        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (sectionResponsible == null)
            throw new UnauthorizedAccessException("User is not a section responsible.");

        var provinceId = sectionResponsible.ProvinceId;

        var medicalList = await _medical.GetByProvinceAsync(provinceId);

        return _mapper.Map<IEnumerable<GetMedicalReviewerDto>>(medicalList);
    }


    public async Task<PagedResultDto<GetMedicalReviewerDto>> GetMedicalReviewerForCurrentUserAsync(
        PagedRequestDto paged,
        MedicalReviewerFilterDto? filter)
    {
        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (sectionResponsible == null)
            throw new UnauthorizedAccessException("User is not a section responsible.");

        var provinceId = sectionResponsible.ProvinceId;
        var municipalityId = sectionResponsible.MunicipalityId;

        // var query = _medical.GetAllByItems(
        //     mr => mr.ProvinceId == provinceId && mr.MunicipalityId == municipalityId);

        var query = _medical.GetByFilter(provinceId, municipalityId, filter);

        var totalItems = await query.CountAsync();

        var items = await _medical.GetPaged(query, (paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                        .ProjectTo<GetMedicalReviewerDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();


        return new PagedResultDto<GetMedicalReviewerDto>
        {
            Items = items,
            TotalCount = totalItems,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalItems
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null

        };
    }


}
