

using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IRepository;


public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid elementId, CancellationToken cancellationToken = default);
    IQueryable<User> GetAll();
    Task DeleteByIdAsync(Guid elementId, CancellationToken cancellationToken = default);
    Task UpdateByIdAsync(Guid elementId, string email, CancellationToken cancellationToken = default);
}