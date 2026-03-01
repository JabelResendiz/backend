

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Infrastructure.Repository;

namespace Finlay.PharmaVigilance.Infrastructure.UnitOfWorkPattern;

// Pattern Unit of Work
public class UnitOfWork : IUnitOfWork
{
    private readonly FinlayDbContext _context;
    private Dictionary<Type, object> _repositories;
    public IUserRepository UserRepository { get; }

    public UnitOfWork(FinlayDbContext context, IUserRepository userRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        _repositories = new Dictionary<Type, object>();

        UserRepository = userRepository;

    }

    public IGenericRepository<T> GetRepository<T>() where T : GenericEntity
    {
        var type = typeof(T);

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>).MakeGenericType(type);

            try
            {
                var repositoryInstance = Activator.CreateInstance(repositoryType, _context);


                if (repositoryInstance is GenericRepository<T> repository)
                {

                    _repositories[type] = repository;
                }
                else
                {
                    throw new InvalidCastException($"The created type '{repositoryType}' is not a valid IGenericRepository<{type}>.");
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error creating repository instance", ex);
            }

        }

        return (IGenericRepository<T>)_repositories[type];
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}