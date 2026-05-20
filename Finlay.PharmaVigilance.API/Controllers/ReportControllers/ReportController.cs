using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportQueryService _reportQueryService;
    private readonly IReportCommandService _reportCommandService;
    private readonly ICaptchaService _captchaService;


    public ReportController(IReportQueryService reportQueryService,
                            IReportCommandService reportCommandService,
                            ICaptchaService captchaService)
    {
        _reportQueryService = reportQueryService;
        _reportCommandService = reportCommandService;
        _captchaService = captchaService;
    }

    [HttpPost("createPublic")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("PharmaCritical")]
    public async Task<IActionResult> CreatePublicReport([FromBody] PublicAefiReportDto reportDto)
    {
        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto), "Report data is required.");

        // var isValid = await _captchaService.VerifyToken(reportDto.Token);

        // if (!isValid)
        //     return BadRequest(new { success = false });


        var result = await _reportCommandService.CreatePublicReportAsync(reportDto);

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
    [EnableRateLimiting("GeneralQuery")]
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
    [EnableRateLimiting("GeneralQuery")]
    public async Task<ActionResult> GetReportAssigment(
        [FromQuery] PagedRequestDto pagedRequestDto,
        [FromQuery] ReportMedicalReviewerFilter filter
    )
    {
        if (pagedRequestDto == null)
            throw new ArgumentNullException(nameof(pagedRequestDto), "pagedRequestDto is required.");

        var result = await _reportQueryService.GetReportAssigment(pagedRequestDto, filter);

        return StatusCode(StatusCodes.Status202Accepted, new
        {
            message = "Report successfully search",
            data = result
        });
    }





    [HttpGet("sectionResponsible/assigned")]
    [Authorize(Roles = "SectionResponsible")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("GeneralQuery")]
    public async Task<ActionResult> GetReportsbySectionResponsible(
        [FromQuery] PagedRequestDto pagedRequestDto,
        [FromQuery] ReportSectionResponsibleFilter filter
    )
    {
        if (pagedRequestDto == null)
            throw new ArgumentNullException(nameof(pagedRequestDto), "pagedRequestDto is required.");

        var result = await _reportQueryService.GetReportsBySectionResponsible(
            pagedRequestDto,
            filter);


        return Ok(result);
    }






    //     [HttpGet("{notificationNumber}/pdf")]
    //     [ProducesResponseType(StatusCodes.Status200OK)]
    //     [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //     [ProducesResponseType(StatusCodes.Status409Conflict)]
    //     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //     public async Task<IActionResult> GetPdf(
    //        string notificationNumber
    //    )
    //     {
    //         if (notificationNumber == null)
    //             throw new ArgumentNullException(nameof(notificationNumber), "notificationNumber is required.");

    //         var pdf = await _reportQueryService.GetReportPdfAsync(notificationNumber);

    //         return File(pdf, "application/pdf", $"report_{notificationNumber}.pdf");

    //     }


    //     [HttpGet("{notificationNumber}/pdf")]
    //     [ProducesResponseType(StatusCodes.Status200OK)]
    //     [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //     [ProducesResponseType(StatusCodes.Status409Conflict)]
    //     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //     public async Task<IActionResult> GetReportDetailsPdf(
    //        string notificationNumber
    //    )
    //     {
    //         if (notificationNumber == null)
    //             throw new ArgumentNullException(nameof(notificationNumber), "notificationNumber is required.");

    //         var pdf = await _reportQueryService.GetReportDetailsPdfAsync(notificationNumber);

    //         return File(pdf, "application/pdf", $"report_{notificationNumber}.pdf");

    //     }



    [HttpGet("admin/summary")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("GeneralQuery")]
    public async Task<ActionResult> GetReportsSummaryAdmin(
       [FromQuery] PagedRequestDto paged,
       [FromQuery] string? vaccineName,
       [FromQuery] string? provinceName,
       [FromQuery] string? severity,
       [FromQuery] string? reportStatus
   )
    {
        if (paged == null)
            throw new ArgumentNullException(nameof(paged), "Paged is required.");

        var result = await _reportQueryService.GetFilter(
            paged,
            vaccineName,
            provinceName,
            severity,
            reportStatus);

        return Ok(result);

    }


    [HttpGet("admin/detail")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableRateLimiting("GeneralQuery")]
    public async Task<ActionResult> GetReportDetailAdmin(Guid reportId)
    {

        var result = await _reportQueryService.GetReportDetailAdmin(reportId);

        return Ok(result);

    }

}