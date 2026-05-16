
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GeneralQuery")]
public class LotController : ControllerBase
{
    private readonly ILotCommandService _lotCommandService;
    private readonly ILotQueryService _lotQueryService;

    public LotController(
        ILotCommandService lotCommandService,
        ILotQueryService lotQueryService)
    {
        _lotCommandService = lotCommandService;
        _lotQueryService = lotQueryService;
    }

    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(LotDto dto)
    {

        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "Request body cannot be null");

        await _lotCommandService.Create(dto);

        return Ok(new
        {
            message = "Lot successfully created"
        });

    }


    [HttpGet("getByVaccine")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByVaccine(
        [FromQuery] Guid vaccineId
    )
    {
        var result = await _lotQueryService.GetByVaccine(vaccineId);

        return Ok(result);

    }

}