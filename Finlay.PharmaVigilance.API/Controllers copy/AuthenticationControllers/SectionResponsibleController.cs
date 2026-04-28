using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.Api.Controllers;

/// <summary>
/// API Controller responsible for managing Section Responsible user operations.
/// Provides endpoints for registration of Section Responsible users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SectionResponsibleController : ControllerBase
{
    private readonly ISectionResponsibleService _sectionResponsibleService;

    /// <summary>
    /// Initializes a new instance of the SectionResponsibleController class.
    /// </summary>
    public SectionResponsibleController(ISectionResponsibleService sectionResponsibleService)
    {
        _sectionResponsibleService = sectionResponsibleService;
    }

    /// <summary>
    /// Registers a new Section Responsible user with their profile information.
    /// </summary>
    /// <param name="registerDto">The DTO containing registration and profile details.</param>
    /// <returns>A response indicating successful registration.</returns>
    /// <response code="200">Section Responsible successfully registered.</response>
    /// <response code="400">Bad request - validation failed.</response>
    /// <response code="409">Conflict - user email or username already exists.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterSectionResponsible(RegisterSectionResponsibleDto registerDto)
    {
        if (registerDto == null)
            throw new ArgumentNullException(nameof(registerDto), "Request body cannot be null");

        var result = await _sectionResponsibleService.RegisterSectionResponsibleAsync(registerDto);

        return Ok(new
        {
            message = result,
            success = true
        });

    }



    [HttpGet("GetAll")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetSectionResponsible(
        [FromQuery] PagedRequestDto paged,
        [FromQuery] string? search)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _sectionResponsibleService.GetByFilters(paged, search);

        return Ok(result);

    }



    [HttpGet("searchbymunicipality")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SearchSectionResponsibleByMunicipality([FromQuery] PagedRequestDto paged, int municipalityId)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _sectionResponsibleService.SearchByMunicipality(paged, municipalityId);

        return Ok(new
        {
            message = result,
            success = true
        });

    }
}
