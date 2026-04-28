namespace Finlay.PharmaVigilance.Domain.Entities;

public abstract class BasicEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}