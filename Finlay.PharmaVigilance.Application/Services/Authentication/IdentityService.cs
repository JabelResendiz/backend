
using System.Security.Cryptography;
using AutoMapper;
using Finlay.PharmaVigilance.Application.Authentication;
using Finlay.PharmaVigilance.Application.Common.Authentication;
using Finlay.PharmaVigilance.Application.DTO;
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

    private string GenerateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[64];
        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
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
            throw new ArgumentNullException("invalid credentials");

        // generate token for the authenticate user
        var accessToken = await _jwtTokenGenerator.GenerateToken(savedUser);

        var refreshToken = GenerateRefreshToken();


        await _unitOfWork.GetRepository<RefreshToken>().CreateAsync(new RefreshToken
        {
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = savedUser.Id,
            User = savedUser
        });

        await _unitOfWork.CompleteAsync();

        // If the credentials are valid, generate a token for the authenticated user.
        return new UserResponseDTO
        {
            Id = savedUser.Id,
            UserName = savedUser.UserName!,
            UserRole = savedUser.UserRole!,
            Token = accessToken,
            RefreshToken = refreshToken
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


    public async Task<RefreshTokenResponseDto> RefreshTokenAsync(string refreshToken)
    {
        Console.WriteLine($"============================Received refresh token: {refreshToken}==================================="); // Debug log


        var storedToken = await _unitOfWork
            .GetRepository<RefreshToken>()
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedToken == null)
            throw new Exception("Invalid refresh token.");

        if (storedToken.Revoked)
            throw new Exception("Refresh token revoked.");

        if (storedToken.ExpiresAt < DateTime.UtcNow)
            throw new Exception("Refresh token expired.");

        // if (storedToken.User == null)
        //     throw new Exception("========================Associated user not found for the refresh token.========================");

        var user = await _unitOfWork.UserRepository.GetByIdAsync(storedToken.UserId);

        var newAccessToken = await _jwtTokenGenerator.GenerateToken(user);

        // ROTATE refresh token (MUY recomendado)
        storedToken.Revoked = true;

        var newRefreshToken = GenerateRefreshToken();

        await _unitOfWork.GetRepository<RefreshToken>().CreateAsync(
            new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

        await _unitOfWork.CompleteAsync();

        return new RefreshTokenResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }


    public async Task LogoutAsync(string refreshToken)
    {
        var storedToken = await _unitOfWork
                .GetRepository<RefreshToken>()
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedToken != null)
        {
            storedToken.Revoked = true;

            await _unitOfWork.CompleteAsync();
        }
    }


}