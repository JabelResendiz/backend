

using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalReviewController : ControllerBase
{
    private readonly IMedicalReviewQueryService _medicalReviewQueryService;
    private readonly IMedicalReviewCommandService _medicalReviewCommandService;

    public MedicalReviewController(IMedicalReviewQueryService medicalReviewQueryService,
                            IMedicalReviewCommandService medicalReviewCommandService)
    {
        _medicalReviewQueryService = medicalReviewQueryService;
        _medicalReviewCommandService = medicalReviewCommandService;
    }


    [HttpPost]
    [Authorize(Roles = "MedicalReviewer")]
    public async Task<IActionResult> CreateMedicalReview([FromBody] MedicalReviewDto medicalReviewDto)
    {
        if (medicalReviewDto == null)
            throw new ArgumentNullException(nameof(medicalReviewDto), "Medical Review data is required.");

        var result = await _medicalReviewCommandService.CreateAsync(medicalReviewDto);

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Medical Review successfully created",
            data = result
        });
    }

}




