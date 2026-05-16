
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Controllers.CatalogControllers;

/// <summary>
/// API Controller responsible for managing Medical Reviewer user operations.
/// Provides endpoints for registration of Medical Reviewer users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GeneralQuery")]
public class GetCatalogController : ControllerBase
{
    private readonly IVaccineQueryService _vaccineQueryService;
    private readonly ISymptomQueryService _symptomQueryService;

    /// <summary>
    /// Initializes a new instance of the CatalogController class.
    /// </summary>
    public GetCatalogController(
        IVaccineQueryService vaccineQueryService,
        ISymptomQueryService symptomQueryService)
    {
        _vaccineQueryService = vaccineQueryService;
        _symptomQueryService = symptomQueryService;
    }


    [HttpGet("vaccines/actives")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetActiveVaccines()
    {
        var result = await _vaccineQueryService.GetActiveVaccinesLookup();

        return Ok(result);

    }

    [HttpGet("symptoms/actives")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetActiveSymptoms()
    {
        var result = await _symptomQueryService.GetActiveSymptomsLookup();

        return Ok(result);

    }


    [HttpGet("vaccine")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetVaccineByNameAndStatus(
        [FromQuery] PagedRequestDto paged,
        [FromQuery] string? search,
        [FromQuery] bool? status)
    {
        var result = await _vaccineQueryService.GetByFilters(paged, search, status);

        return Ok(result);

    }

    [HttpGet("symptom")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetSymptomByNameAndStatus(
        [FromQuery] PagedRequestDto paged,
        [FromQuery] string? search,
        [FromQuery] bool? status)
    {
        var result = await _symptomQueryService.GetByFilters(paged, search, status);

        return Ok(result);

    }



    [HttpGet("finlayVaccines")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetFinlayVaccines()
    {
        var result = await _vaccineQueryService.GetSelfVaccines();

        return Ok(result);

    }


}