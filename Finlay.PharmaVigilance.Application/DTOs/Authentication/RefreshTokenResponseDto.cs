namespace Finlay.PharmaVigilance.Application.DTO;


public class RefreshTokenResponseDto
{
    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;
}

