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
        var result = await _municipalDashboardService.GetPerformanceAsync();
        return Ok(result);
    }




    [HttpGet("stats_municipal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStatsDashboard(
        [FromQuery] DashboardFilterDto filter
    )
    {

        var result = await _municipalDashboardService
        .GetDashboardAsync(filter);

        return Ok(result);
    }




}