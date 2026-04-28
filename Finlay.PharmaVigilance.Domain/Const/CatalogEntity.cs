namespace Finlay.PharmaVigilance.Domain.Entities;

public abstract class CatalogEntity : BasicEntity, IEntity<int>
{
    public int Id { get; init; }
}