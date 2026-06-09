
using System.Linq.Expressions;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

// "Repository Pattern"
public class GenericRepository<T> : IGenericRepository<T> where T : BasicEntity
{
    protected readonly DbSet<T> _entity; // Represents the database set for the entity type T.
    protected readonly FinlayDbContext _context; // Holds the database context for interacting with the database.

    public GenericRepository(FinlayDbContext context)
    {
        if (context == null)
            throw new ArgumentException(nameof(context));

        _context = context;
        _entity = _context.Set<T>();// Initialize the DbSet for the entity type T.
    }

    public virtual async Task<T> CreateAsync(T element, CancellationToken cancellationToken = default)
    {
        // Asynchronously add the entity to the DbSet.
        await _entity.AddAsync(element, cancellationToken);

        // Return the added entity
        return element;
    }

    public virtual IQueryable<T> GetAll()
    {
        return _entity; // Return the entire DbSet as an IQueryable.
    }

    public virtual IQueryable<T> GetAllPaged(int skip, int take)
    {
        var query = GetAll();

        return query
                .Skip(skip)
                .Take(take);
    }


    public virtual IQueryable<T> GetAllPagedbyItem(int skip, int take,
                    Expression<Func<T, bool>> predicate,
                    params Expression<Func<T, object>>[] includes
                        )
    {

        var query = GetAllByItems(predicate);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query.Skip(skip).Take(take);
    }


    public virtual IQueryable<T> GetPaged(IQueryable<T> query, int skip, int take)
    {
        return query.Skip(skip).Take(take);
    }


    public async Task<T?> FirstOrDefaultAsync(
                    Expression<Func<T, bool>> predicate,
                    CancellationToken cancellationToken = default,
                    params Expression<Func<T, object>>[] includes
                   )
    {
        IQueryable<T> query = _entity;

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<T> GetByIdAsync<TId>(TId elementId,
                                                    CancellationToken cancellationToken = default,
                                                    params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _entity;

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        var result = await query.FirstOrDefaultAsync(e => EF.Property<TId>(e, "Id")!.Equals(elementId), cancellationToken);

        if (result == null) // Check if the entity was not found.
            throw new KeyNotFoundException($"No entity was found with the ID '{elementId}'.");

        return result;
    }

    public virtual async Task DeleteByIdAsync<TId>(TId elementId, CancellationToken cancellationToken = default)
    {
        // Retrieve the entity by its ID.
        var result = await GetByIdAsync(elementId, cancellationToken);

        // Remove the retrieved entity from the DbSet.
        _entity.Remove(result);

    }


    public virtual IQueryable<T> GetAllByItems(params Expression<Func<T, bool>>[] expressions)
    {
        IQueryable<T> query = _entity; // Initialize the query with the DbSet.

        if (expressions != null)
        {
            foreach (var exp in expressions) // Loop through each filter expression.
            {
                query = query.Where(exp); // Apply the filter expression to the query.
            }
        }

        // Return the filtered query.
        return query;
    }


    public async Task<T?> GetByItems(Expression<Func<T, bool>>[] expressions,
                                     Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _entity;

        if (expressions != null)
        {
            foreach (var exp in expressions) // Loop through each filter expression.
            {
                query = query.Where(exp); // Apply the filter expression to the query.
            }
        }

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include); // Apply each include to the query.
            }
        }

        var result = await query.FirstOrDefaultAsync();


        return result;
    }


}