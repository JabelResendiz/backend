using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.AspNetCore.Mvc;


namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportDuplicateController : ControllerBase
{
    private readonly IReportDuplicateService _reportDuplicateService;

    public ReportDuplicateController(IReportDuplicateService reportDuplicateService)
    {
        _reportDuplicateService = reportDuplicateService;
    }

    [HttpPost("{id}/resolve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Resolve(Guid id, [FromBody] ResolveDuplicateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (dto.Verdict != EnumReportDuplicate.ConfirmedDuplicate &&
            dto.Verdict != EnumReportDuplicate.SeparateAsNew)
            throw new ArgumentException("Invalid verdict.");

        await _reportDuplicateService.ResolveAsync(id, dto);

        return Ok(new { message = "Duplicate resolved successfully." });
    }


    [HttpGet("pending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPending([FromQuery] PagedRequestDto request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        request.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var pendingDuplicates = await _reportDuplicateService.GetPendingAsync(request);

        return Ok(pendingDuplicates);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var duplicate = await _reportDuplicateService.GetByIdAsync(id);

        return Ok(duplicate);
    }
}