using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportQueryService _reportQueryService;
    private readonly IReportCommandService _reportCommandService;

    public ReportController(IReportQueryService reportQueryService,
                            IReportCommandService reportCommandService)
    {
        _reportQueryService = reportQueryService;
        _reportCommandService = reportCommandService;
    }

    [HttpPost("createPublic")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePublicReport([FromBody] PublicAefiReportDto reportDto)
    {
        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto), "Report data is required.");

        var result = await _reportCommandService.CreatePublicReportAsync(reportDto);

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Report successfully created",
            data = result
        });
    }


    [HttpPost("createMedical")]
    [Authorize(Roles = "MedicalReviewer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateMedicalReport([FromBody] MedicalReportDto reportDto)
    {
        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto), "Report data is required.");

        var result = await _reportCommandService.CreateMedicalReportAsync(reportDto);

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Report successfully created",
            data = result
        });
    }



    [HttpGet("get-report")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReportByNotificationNumber(string notificationNumber)
    {
        if (notificationNumber == null)
            throw new ArgumentNullException(nameof(notificationNumber), "notificationNumber is required.");

        var result = await _reportQueryService.GetReportByNotificationNumber(notificationNumber);

        return StatusCode(StatusCodes.Status202Accepted, new
        {
            message = "Report successfully search",
            data = result
        });
    }



    [HttpGet("get-report-assigment")]
    [Authorize(Roles = "MedicalReviewer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetReportAssigment(
        [FromQuery] PagedRequestDto pagedRequestDto
    )
    {
        if (pagedRequestDto == null)
            throw new ArgumentNullException(nameof(pagedRequestDto), "pagedRequestDto is required.");

        var result = await _reportQueryService.GetReportAssigment(pagedRequestDto);

        return StatusCode(StatusCodes.Status202Accepted, new
        {
            message = "Report successfully search",
            data = result
        });
    }





    [HttpGet("assigned")]
    [Authorize(Roles = "SectionResponsible")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetReportsbySectionResponsible(
        [FromQuery] PagedRequestDto pagedRequestDto
    )
    {
        if (pagedRequestDto == null)
            throw new ArgumentNullException(nameof(pagedRequestDto), "pagedRequestDto is required.");

        var result = await _reportQueryService.GetReportsBySectionResponsible(pagedRequestDto);


        return Ok(result);
    }

}