using Microsoft.AspNetCore.Identity;

namespace Evaluation.Services.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(IdentityUser user , List<string> roles);
    }
}
