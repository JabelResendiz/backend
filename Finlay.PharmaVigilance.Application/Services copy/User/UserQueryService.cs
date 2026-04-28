using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;

/// <summary>
/// Implementation of query services for User entity.
/// Provides read operations for retrieving user information.
/// </summary>
public class UserQueryService : IUserQueryServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserQueryService"/> class.
    /// </summary>
    /// <param name="unitOfWork">
    /// The Unit of Work instance used to access repositories.
    /// </param>
    /// <param name="mapper">
    /// AutoMapper instance used to map domain entities to DTOs.
    /// </param>
    public UserQueryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A <see cref="GetUserDto"/> if found; otherwise, null.
    /// </returns>
    public async Task<GetUserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var userRepository = _unitOfWork.UserRepository;
        var user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user == null)
            return null;

        return _mapper.Map<GetUserDto>(user);
    }

    /// <summary>
    /// Retrieves all users in the system.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A collection of <see cref="GetUserDto"/> representing all users.
    /// </returns>
    public async Task<IEnumerable<GetUserDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        var userRepository = _unitOfWork.UserRepository;
        var usersQuery = userRepository.GetAll();

        var userList = await usersQuery.ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<GetUserDto>>(userList);
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A <see cref="GetUserDto"/> if found; otherwise, null.
    /// </returns>
    public async Task<GetUserDto?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return null;

        var userRepository = _unitOfWork.UserRepository;
        var query = userRepository.GetAll();

        var user = await query.FirstOrDefaultAsync(
            u => u.UserName == userName,
            cancellationToken);

        if (user == null)
            return null;

        return _mapper.Map<GetUserDto>(user);
    }
}