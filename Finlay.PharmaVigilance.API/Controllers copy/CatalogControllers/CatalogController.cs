
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.Api.Controllers;

/// <summary>
/// API Controller responsible for managing Medical Reviewer user operations.
/// Provides endpoints for registration of Medical Reviewer users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogCommandService _catalogCommandService;

    /// <summary>
    /// Initializes a new instance of the CatalogController class.
    /// </summary>
    public CatalogController(ICatalogCommandService catalogCommandService)
    {
        _catalogCommandService = catalogCommandService;
    }

    [HttpPost("register/symptom")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterSymptom(SymptomDto symptomDto)
    {

        if (symptomDto == null)
            throw new ArgumentNullException(nameof(symptomDto), "Request body cannot be null");

        var result = await _catalogCommandService.CreateSymptomAsync(symptomDto);

        return Ok(new
        {
            message = result,
            success = true
        });

    }


    [HttpPost("register/vaccine")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterVaccine(VaccineDto vaccineDto)
    {

        if (vaccineDto == null)
            throw new ArgumentNullException(nameof(vaccineDto), "Request body cannot be null");

        var result = await _catalogCommandService.CreateVaccineAsync(vaccineDto);

        return Ok(new
        {
            message = result,
            success = true
        });

    }

    [HttpPost("deactivate/vaccine/{vaccineId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeactivateVaccine(int vaccineId)
    {

        var result = await _catalogCommandService.DeactivateVaccine(vaccineId);

        return Ok(new
        {
            message = result,
            success = true
        });

    }


    [HttpPost("activate/vaccine/{vaccineId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActivateVaccine(int vaccineId)
    {

        var result = await _catalogCommandService.ActivateVaccine(vaccineId);

        return Ok(new
        {
            message = result,
            success = true
        });

    }


    [HttpPost("deactivate/symptom/{symptomId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeactivateSymptom(int symptomId)
    {

        var result = await _catalogCommandService.DeactivateSymptom(symptomId);

        return Ok(new
        {
            message = result,
            success = true
        });

    }


    [HttpPost("activate/symptom/{symptomId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActivateSymptom(int symptomId)
    {

        var result = await _catalogCommandService.ActivateSymptom(symptomId);

        return Ok(new
        {
            message = result,
            success = true
        });

    }
}