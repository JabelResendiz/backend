namespace Finlay.PharmaVigilance.Application.IServices;


public interface ICaptchaService
{
    Task<bool> VerifyToken(string token);
}