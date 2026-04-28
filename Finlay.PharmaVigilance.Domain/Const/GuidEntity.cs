namespace Finlay.PharmaVigilance.Domain.Entities;

public abstract class GuidEntity : BasicEntity, IEntity<Guid>
{
    public Guid Id { get; init; } = new Guid();
}