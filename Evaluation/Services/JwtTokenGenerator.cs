using Evaluation.Models;
using Evaluation.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Evaluation.Services
{
    /// <summary>
    /// Service class for generating JSON Web Tokens (JWTs) for user authentication.
    /// Implements the IJwtTokenGenerator interface.
    /// </summary>
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the JwtTokenGenerator class.
        /// </summary>
        /// <param name="configuration">Configuration object to access JWT settings.</param>
        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        public string GenerateToken(IdentityUser user, List<string> roles)
        {
            // Create claims for the user's email and roles
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Add a separate claim for each role
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Retrieve the security key from configuration
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value!));

            // Create signing credentials using the security key and HMAC SHA-256 algorithm
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create a JWT security token
            var securityToken = new JwtSecurityToken
            (
                claims: claims, 
                expires: DateTime.Now.AddMinutes(60), 
                issuer: _configuration.GetSection("JWT:Issuer").Value, 
                audience: _configuration.GetSection("JWT:Audience").Value, 
                signingCredentials: signingCredentials 
            );

            // Convert the JWT security token to a string
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            
            return tokenString;
        }
    }
}