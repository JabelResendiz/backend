namespace Finlay.PharmaVigilance.Domain.Entities;

public class RefreshToken : GuidEntity
{

    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool Revoked { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}