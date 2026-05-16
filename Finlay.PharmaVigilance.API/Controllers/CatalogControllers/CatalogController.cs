
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Controllers;

/// <summary>
/// API Controller responsible for managing Medical Reviewer user operations.
/// Provides endpoints for registration of Medical Reviewer users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GeneralQuery")]
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



    [HttpPost("updateStatus/symptom")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateSymptomStatus(
        [FromQuery] Guid symptomId,
        [FromQuery] bool isActive)
    {

        var result = await _catalogCommandService.UpdateSymptomStatus(symptomId, isActive);

        return Ok(new
        {
            message = result
        });

    }



    [HttpPost("updateStatus/vaccine")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateVaccineStatus(
        [FromQuery] Guid vaccineId,
        [FromQuery] bool isActive)
    {

        var result = await _catalogCommandService.UpdateVaccineStatus(vaccineId, isActive);

        return Ok(new
        {
            message = result
        });

    }


    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteVaccine(Guid vaccineId)
    {

        await _catalogCommandService.DeleteVaccine(vaccineId);

        return Ok(new
        {
            message = "Vaccine deleted succesffuly"
        });

    }

}