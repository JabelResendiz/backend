
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GeneralQuery")]
public class VaccinationCenterController : ControllerBase
{
    private readonly IVaccinationCenterCommandService _vaccinationCenterCommandService;
    private readonly IVaccinationCenterQueryService _vaccinationCenterQueryService;

    public VaccinationCenterController(
        IVaccinationCenterCommandService vaccinationCenterCommandService,
        IVaccinationCenterQueryService vaccinationCenterQueryService)
    {
        _vaccinationCenterCommandService = vaccinationCenterCommandService;
        _vaccinationCenterQueryService = vaccinationCenterQueryService;
    }

    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(VaccinationCenterDto dto)
    {

        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "Request body cannot be null");

        await _vaccinationCenterCommandService.CreateVaccinationCenter(dto);

        return Ok(new
        {
            message = "Vaccination center successfully created"
        });

    }

    [HttpGet("getByMunicipality")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAllByMunicipality(
        [FromQuery] int municipalityId,
        [FromQuery] int provinceId
    )
    {

        var result = await _vaccinationCenterQueryService.GetByMunicipality(municipalityId, provinceId);

        return Ok(result);

    }


    [HttpGet("getBySectionResponsible")]
    [Authorize(Roles = "SectionResponsible")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAllBySectionResponsible()
    {

        var result = await _vaccinationCenterQueryService.GetBySectionResponsible();

        return Ok(result);

    }

}