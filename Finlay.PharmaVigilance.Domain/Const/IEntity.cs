namespace Finlay.PharmaVigilance.Domain.Entities;

public interface IEntity<TId>
{
    TId Id { get; init; }
}
