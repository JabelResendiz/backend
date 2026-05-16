using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
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
public class MedicalReviewerController : ControllerBase
{
    private readonly IMedicalReviewerService _medicalReviewerService;

    /// <summary>
    /// Initializes a new instance of the MedicalReviewerController class.
    /// </summary>
    public MedicalReviewerController(IMedicalReviewerService medicalReviewerService)
    {
        _medicalReviewerService = medicalReviewerService;
    }

    /// <summary>
    /// Registers a new Medical Reviewer user with their profile information.
    /// </summary>
    /// <param name="registerDto">The DTO containing registration and profile details.</param>
    /// <returns>A response indicating successful registration.</returns>
    /// <response code="200">Medical Reviewer successfully registered.</response>
    /// <response code="400">Bad request - validation failed.</response>
    /// <response code="409">Conflict - user email or username already exists.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("register")]
    [Authorize(Roles = "SectionResponsible")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterMedicalReviewer(RegisterMedicalReviewerDto registerDto)
    {

        if (registerDto == null)
            throw new ArgumentNullException(nameof(registerDto), "Request body cannot be null");

        var result = await _medicalReviewerService.RegisterMedicalReviewerAsync(registerDto);

        return Ok(new
        {
            message = result,
            success = true
        });

    }

    // [HttpGet("getbyProvince")]
    // [Authorize(Roles = "SectionResponsible")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status409Conflict)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public async Task<ActionResult<IEnumerable<GetMedicalReviewerDto>>> GetMedicalReviewerByProvince()
    // {

    //     var users = await _medicalReviewerService.ListByProvinceAsync();
    //     return Ok(users);

    // }


    [HttpGet("by-current-user-municipality")]
    [Authorize(Roles = "SectionResponsible")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetMedicalReviewerByCurrentUserMunicipality(
        [FromQuery] PagedRequestDto paged
    )
    {
        if (paged == null)
            throw new ArgumentNullException(nameof(paged), "Query parameters cannot be null");

        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";


        var users = await _medicalReviewerService.GetMedicalReviewerForCurrentUserAsync(paged);
        return Ok(users);

    }

}