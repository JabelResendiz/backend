using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        // UserManager is injected to interact with ASP.NET Identity users
        _userManager = userManager;
    }

    public async Task<User> GetByIdAsync(Guid elementId, CancellationToken cancellationToken = default)
    {
        // Query Identity's Users DbSet to find a user by Id
        var user = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.Id == elementId);

        // If no user is found, throw an exception
        if (user is null)
            throw new Exception($"Error searching user: {elementId}");

        return user;
    }

    public IQueryable<User> GetAll()
    {
        // Expose IQueryable to allow further filtering at service level
        return _userManager.Users;
    }

    public async Task DeleteByIdAsync(Guid elementId, CancellationToken cancellationToken = default)
    {
        // Search the user before attempting deletion
        var user = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.Id == elementId);

        if (user is null)
            throw new Exception($"Error deleting user: {elementId}");

        // Use Identity's DeleteAsync to properly remove the user
        await _userManager.DeleteAsync(user);
    }

    public async Task UpdateByIdAsync(Guid elementId, string email, CancellationToken cancellationToken = default)
    {
        // Reuse GetByIdAsync to ensure user exists
        User user = await GetByIdAsync(elementId, cancellationToken);

        // Update email only if it's provided and different from the current one
        if (!string.IsNullOrEmpty(email) && email != user.Email)
        {
            user.Email = email;

            // Use Identity's UpdateAsync to persist changes
            var result = await _userManager.UpdateAsync(user);

            // If update fails, throw detailed Identity errors
            if (!result.Succeeded)
                throw new Exception("Error updating user email: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}