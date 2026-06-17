using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("Auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ICaptchaService _captchaService;

    public AuthenticationController(
        IIdentityService identityService,
        ICaptchaService captchaService)
    {
        _identityService = identityService;
        _captchaService = captchaService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginUser(LoginUserDto loginDto)
    {

        if (loginDto == null)
            throw new ArgumentNullException(nameof(loginDto), "Login data is required.");

        // var isValid = await _captchaService.VerifyToken(loginDto.Token);

        // if (!isValid)
        //     return BadRequest(new { success = false });

        var authResult = await _identityService.LoginUserAsync(loginDto);

        // Response.Cookies.Append("refreshToken", authResult.RefreshToken,
        //     new CookieOptions
        //     {
        //         HttpOnly = true,
        //         Secure = false,
        //         SameSite = SameSiteMode.Lax,
        //         Expires = DateTime.UtcNow.AddDays(7)
        //     });

        Response.Cookies.Append("refreshToken", authResult.RefreshToken,
           new CookieOptions
           {
               HttpOnly = true,
               Secure = true,
               SameSite = SameSiteMode.None,
               Expires = DateTime.UtcNow.AddDays(7)
           });

        return Ok(new
        {
            authResult.Id,
            authResult.UserName,
            authResult.UserRole,
            accessToken = authResult.Token
        });

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

    [HttpPost("complete-registration")]
    public async Task<IActionResult> Create(
        CompleteRegistrationDto dto
    )
    {
        await _identityService.CompleteRegistrationAsync(dto);

        return Ok();
    }



    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            // return Unauthorized(new
            // {
            //     success = false,
            //     message = "Refresh token missing."
            // });
            return Ok(new
            {
                success = false,
                message = "Refresh token missing"
            });
        }


        var result = await _identityService.RefreshTokenAsync(refreshToken);

        // Response.Cookies.Append("refreshToken", result.RefreshToken,
        //     new CookieOptions
        //     {
        //         HttpOnly = true,
        //         Secure = false,
        //         SameSite = SameSiteMode.Lax,
        //         Expires = DateTime.UtcNow.AddDays(7)
        //     });


        Response.Cookies.Append("refreshToken", result.RefreshToken,
        new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok(new
        {
            accessToken = result.AccessToken
        });
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _identityService.LogoutAsync(refreshToken);
        }

        // Response.Cookies.Delete("refreshToken", new CookieOptions
        // {
        //     HttpOnly = true,
        //     Secure = false,
        //     SameSite = SameSiteMode.Lax
        // });


        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        return Ok(new
        {
            message = "Logged out successfully."
        });
    }


}