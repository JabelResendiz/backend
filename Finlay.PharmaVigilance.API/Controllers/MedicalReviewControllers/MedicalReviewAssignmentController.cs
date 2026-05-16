using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GeneralQuery")]
public class MedicalReviewAssignmentController : ControllerBase
{
    private readonly IMedicalReviewAssignmentCommandService _medicalReviewAssignmentCommandService;

    public MedicalReviewAssignmentController(
        IMedicalReviewAssignmentCommandService medicalReviewAssignmentCommandService)
    {
        _medicalReviewAssignmentCommandService = medicalReviewAssignmentCommandService;
    }

    [HttpPost("create")]
    [Authorize(Roles = "SectionResponsible")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAssignment([FromBody] MedicalReviewAssignmentDTO reportDto)
    {
        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto), "Medical Review Assignment data is required.");

        var result = await _medicalReviewAssignmentCommandService.CreateAsync(reportDto);

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Medical Review Assignment successfully created",
            data = result
        });
    }


    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAssigment([FromQuery] Guid assignmentId)
    {
        await _medicalReviewAssignmentCommandService.DeleteAsync(assignmentId);

        return Ok(new
        {
            message = "the assignment successfully deleted"
        });
    }


}