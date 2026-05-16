using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GeneralQuery")]
public class DashboardController : ControllerBase
{
    private readonly IReportQueryService _reportQueryService;
    private readonly IVaccineQueryService _vaccineQueryService;

    public DashboardController(IReportQueryService reportQueryService,
                            IVaccineQueryService vaccineQueryService
                              )
    {
        _reportQueryService = reportQueryService;
        _vaccineQueryService = vaccineQueryService;

    }


    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDashboardAdmin()
    {
        var report = await _reportQueryService.GetReportDashboard();
        var vaccine = await _vaccineQueryService.GetVaccinesDashboard();

        return Ok(new DashboardAdminDto
        {
            ReportDashboard = report,
            VaccineDashboard = vaccine
        });

    }


}