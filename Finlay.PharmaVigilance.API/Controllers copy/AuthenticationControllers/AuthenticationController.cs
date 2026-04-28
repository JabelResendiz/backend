using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthenticationController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginUser(LoginUserDto loginDto)
    {

        var token = await _identityService.LoginUserAsync(loginDto);
        return Ok(token);

    }

    [HttpPost]
    [Route("register/admins")]
    public async Task<IActionResult> RegisterAdmin(RegisterUserDto registerAdminDto)
    {
        var result = await _identityService.RegisterAdminAsync(registerAdminDto);
        return Ok(new
        {
            message = result,
            success = true
        });
    }

}