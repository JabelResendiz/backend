using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GeneralQuery")]
[Authorize(Roles = "SectionResponsible")]
public class MunicipalDashboardController : ControllerBase
{
    private readonly IMunicipalDashboardService _municipalDashboardService;

    public MunicipalDashboardController(
        IMunicipalDashboardService municipalDashboardService
    )
    {
        _municipalDashboardService = municipalDashboardService;


    }



    [HttpGet("overview")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMunicipalDashboardOverview()
    {
        var result = await _municipalDashboardService.GetOverviewAsync();
        return Ok(result);
    }



    [HttpGet("doctors-performance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDoctorPerformance()
    {
        // una coleccion de doctor performance
        var result = await _municipalDashboardService.GetDoctorPerformanceAsync();
        return Ok(result);
    }


    // [HttpGet("report-status")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status409Conflict)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public async Task<IActionResult> GetReportStatus()
    // {
    //     // una coleccion de reports status
    // }



    // [HttpGet("bottlenecks")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status409Conflict)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public async Task<IActionResult> GetBottlenecks()
    // {
    //     // una coleccion de bottlenecks
    // }




}