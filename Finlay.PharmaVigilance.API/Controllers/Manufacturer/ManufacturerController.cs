
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
public class ManufacturerController : ControllerBase
{
    private readonly IManufacturerQueryService _manufacturerQueryService;

    public ManufacturerController(
        IManufacturerQueryService manufacturerQueryService)
    {
        _manufacturerQueryService = manufacturerQueryService;
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetManufacturers()
    {

        var result = await _manufacturerQueryService.GetManufacturers();

        return Ok(result);

    }



}