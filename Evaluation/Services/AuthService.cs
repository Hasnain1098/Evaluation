using Evaluation.DB;
using Evaluation.DTOs.Auth;
using Evaluation.Services.IService;
using Microsoft.AspNetCore.Identity;

namespace Evaluation.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext? _appDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        // Constructor to initialize dependencies
        public AuthService(AppDbContext appDbContext, UserManager<IdentityUser> userManager, IJwtTokenGenerator jwtTokenGenerator, RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleManager = roleManager;
        }

 
        public async Task<string> Register(RegisterationRequestDto registerationRequestDto)
        {
            // Ensure the registration request data is valid
            if (registerationRequestDto == null || string.IsNullOrEmpty(registerationRequestDto.Name) ||
                string.IsNullOrEmpty(registerationRequestDto.Email) || string.IsNullOrEmpty(registerationRequestDto.Password) ||
                string.IsNullOrEmpty(registerationRequestDto.Role))
            {
                return "Invalid registration details.";
            }

            // Create a new user object
            var user = new IdentityUser
            {
                UserName = registerationRequestDto.Name,
                Email = registerationRequestDto.Email,
                NormalizedEmail = registerationRequestDto.Email.ToUpper()
            };

            try
            {
                // Attempt to create the user
                var result = await _userManager.CreateAsync(user, registerationRequestDto.Password);

                if (result.Succeeded)
                {
                    // Check if the role exists
                    if (!await _roleManager.RoleExistsAsync(registerationRequestDto.Role))
                    {
                        // Create the role if it does not exist
                        var roleResult = await _roleManager.CreateAsync(new IdentityRole(registerationRequestDto.Role));
                        if (!roleResult.Succeeded)
                        {
                            // Return error if role creation failed
                            return roleResult.Errors.FirstOrDefault()?.Description ?? "Unknown error while creating role.";
                        }
                    }

                    // Assign the role to the user
                    var roleAssignmentResult = await _userManager.AddToRoleAsync(user, registerationRequestDto.Role);
                    if (roleAssignmentResult.Succeeded)
                    {
                        // Successful registration and role assignment
                        return "Registration and role assignment successful.";
                    }
                    else
                    {
                        // Return the first error description from role assignment
                        return roleAssignmentResult.Errors.FirstOrDefault()?.Description ?? "Unknown error while assigning role.";
                    }
                }
                else
                {
                    // Return the first error description from user creation
                    return result.Errors.FirstOrDefault()?.Description ?? "Unknown error.";
                }
            }
            catch (Exception ex)
            {
                return "An error occurred during registration.";
            }
        }

        
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            // Validate the login request data
            if (loginRequestDto == null || string.IsNullOrEmpty(loginRequestDto.UserName) || string.IsNullOrEmpty(loginRequestDto.Password))
            {
                return new LoginResponseDto { User = null, Token = "" };
            }

            try
            {
                // Find the user by username
                var user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
                if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
                {
                    return new LoginResponseDto { User = null, Token = "" };
                }
                List<string> rolesList = (await _userManager.GetRolesAsync(user)).ToList();
                // Generate a token for the user
                var token = _jwtTokenGenerator.GenerateToken(user, rolesList);

                var userDto = new UserDto
                {
                    Email = user.Email,
                    ID = user.Id,
                    Name = user.UserName
                };

                // Return the login response with user details and token
                return new LoginResponseDto
                {
                    User = userDto,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                // Log.Error(ex, "An error occurred during login.");
                return new LoginResponseDto { User = null, Token = "" };
            }
        }

       
        public async Task<IdentityUser?> GetAsync(string? id)
        {
            // Ensure the ID is valid
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            try
            {
                // Find and return the user by ID
                return await _userManager.FindByIdAsync(id);
            }
            catch (Exception ex)
            {
               
                return null;
            }
        }

        
        public async Task<bool> UpdateUserNameByEmailAsync(UpdateUserDetailsDto updateUserDetailsDto)
        {
            // Validate the update details
            if (updateUserDetailsDto == null || string.IsNullOrEmpty(updateUserDetailsDto.Email) || string.IsNullOrEmpty(updateUserDetailsDto.NewUserName))
            {
                return false;
            }

            try
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(updateUserDetailsDto.Email);
                if (user == null)
                {
                    return false;
                }

                // Update the user's username
                user.UserName = updateUserDetailsDto.NewUserName;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                // Log the exception
                // Log.Error(ex, "An error occurred while updating the username.");
                return false;
            }
        }

        
        public async Task<bool> DeleteUserByIdAsync(string userId)
        {
            // Ensure the user ID is valid
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            try
            {
                // Find the user by ID
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return false;
                }

                // Attempt to delete the user
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
    }
}