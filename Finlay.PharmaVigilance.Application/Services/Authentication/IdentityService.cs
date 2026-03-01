
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
    /// <param name="userDto">The login DTO containing username and password.</param>
    /// <returns>A JWT token if authentication is successful, otherwise null.</returns>
    // public async Task<string> LoginUserAsync(LoginUserDto userDto)
    // {
    //     // Map the login DTO to the User model.
    //     var user = _mapper.Map<User>(userDto);


    //     // Check if the mapping or the user object is null.
    //     if (user == null)
    //         throw new Exception();

    //     //Console.WriteLine("User mapped successfully: " + user.Email + " " + userDto.Password);
    //     // Validate the user's credentials.
    //     var savedUser = await _identityManager.CheckCredentialsAsync(user.Email!, userDto.Password);

    //     //Console.WriteLine("candela");

    //     // If the credentials are invalid, return null.
    //     if (savedUser is null)
    //         throw new Exception();

    //     // If the credentials are valid, generate a token for the authenticated user.
    //     return await _jwtTokenGenerator.GenerateToken(savedUser);

    // }
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
            Email = savedUser.Email!,
            UserRole = savedUser.UserRole!,
            Token = token
        } ;

    }



    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="userDto">The register DTO containing user details.</param>
    /// <returns>A JWT token upon successful registration.</returns>
    
    public async Task<string> RegisterUserAsync(RegisterUserDto userDto)
    {
        //validate that the provided role is allowed
        if (!UserRoleHelper.IsValidRole(userDto.UserRole))
            throw new Exception("Invalid Role");

        // Map the DTO to the User entity
        var user = _mapper.Map<User>(userDto);

        // Create the user in ASP.NET Identity with the provided password
        var savedUser = await _identityManager.CreateUserAsync(user, userDto.Password);
    
        if (savedUser == null)
            throw new Exception("User creation failed");

        // Assign the specified role to the newly created user
        await _identityManager.AddRoles(savedUser.Id.ToString(), userDto.UserRole);

        // Commit all changes to the database
        await _unitOfWork.CompleteAsync();

        return "User created successfully";
    }

    /// <summary>
    /// Updates an existing user's details.
    /// </summary>
    /// <param name="updateDto">The update DTO containing user details.</param>

    // public async Task UpdateUserAsync(UpdateUserDto updateDto)
    // {
    //     try
    //     {

    //         var employee = _mapper.Map<Employee>(updateDto);

    //         _unitOfWork.GetRepository<Employee>().Update(employee);
            
    //         // Update user email if not ShippingSupervisor
            
    //         await _unitOfWork.UserRepository.UpdateByIdAsync(updateDto.Id, updateDto.Email);
    //         // Save changes to database
    //         await _unitOfWork.CompleteAsync();

    //     }
    //     catch (Exception ex)
    //     {
    //         // Log any errors
    //         throw new Exception(ex.Message);
    //     }
    // }


}