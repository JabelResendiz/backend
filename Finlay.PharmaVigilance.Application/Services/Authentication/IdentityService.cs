
using AutoMapper;
using Finlay.PharmaVigilance.Application.Authentication;
using Finlay.PharmaVigilance.Application.Common.Authentication;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.Services.Authentication;

/// <summary>
/// Manages user authentication and registration operations.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly IIdentityManager _identityManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityService"/> class.
    /// </summary>
    /// <param name="jwtTokenGenerator">The JWT token generator.</param>
    /// <param name="mapper">The AutoMapper mapper.</param>
    /// <param name="identityManager">The identity manager.</param>
    /// <param name="unitOfWork">The unit of work.</param>

    public IdentityService(
                IJwtTokenGenerator jwtTokenGenerator,
                IMapper mapper,
                IIdentityManager identityManager,
                IUnitOfWork unitOfWork
                )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
        _identityManager = identityManager;
        _unitOfWork = unitOfWork;
    }
    /// <summary>
    /// Authenticates a user based on the provided credentials.
    /// </summary>
    /// <param name="loginDto">The login DTO containing username and password.</param>
    /// <returns>A JWT token if authentication is successful, otherwise null.</returns>
    public async Task<UserResponseDTO> LoginUserAsync(LoginUserDto loginDto)
    {

        // Validate the user's credentials.
        var savedUser = await _identityManager.CheckCredentialsAsync(
            loginDto.Email,
            loginDto.Password
        );

        // If the credentials are invalid, return null.
        if (savedUser == null)
            throw new Exception("invalid credentials");

        // generate token for the authenticate user
        var token = await _jwtTokenGenerator.GenerateToken(savedUser);

        // If the credentials are valid, generate a token for the authenticated user.
        return new UserResponseDTO
        {
            Id = savedUser.Id,
            UserName = savedUser.UserName!,
            UserRole = savedUser.UserRole!,
            Token = token
        };

    }


    public async Task<string> RegisterAdminAsync(RegisterUserDto registerAdminDto)
    {
        if (registerAdminDto == null)
            throw new ArgumentNullException(nameof(registerAdminDto), "Registration DTO cannot be null.");

        var user = _mapper.Map<User>(registerAdminDto);
        user.UserRole = UserRole.Admin.ToString();

        var createdUser = await _identityManager.CreateUserAsync(user, registerAdminDto.Password);
        if (createdUser == null)
            throw new InvalidOperationException("Failed to create user account.");

        // Assign Administrator role
        await _identityManager.AddRoles(createdUser.Id.ToString(), UserRole.Admin.ToString());

        var admin = new Admin
        {
            UserId = createdUser.Id,
            User = createdUser
        };

        await _unitOfWork.GetRepository<Admin>().CreateAsync(admin);
        await _unitOfWork.CompleteAsync();

        return "Administrator registered successfully.";
    }


}