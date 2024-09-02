using Evaluation.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace Evaluation.Services.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegisterationRequestDto registerationRequestDto);

        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        Task<IdentityUser?> GetAsync(string? userId);

        Task<bool> UpdateUserNameByEmailAsync(UpdateUserDetailsDto updateUserDetailsDto);
        Task<bool> DeleteUserByIdAsync(string userId);

    }
}
