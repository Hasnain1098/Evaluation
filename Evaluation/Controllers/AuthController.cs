using Evaluation.DTOs.Auth;
using Evaluation.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
 
    public class AuthController : Controller
    {
        private readonly IAuthService _authService; 
        protected RegisterationResponseDto _responseDto = new(); 

       
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

     
        [HttpPost("api/users/register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto model)
        {
            //registering user
            var errorMessage = await _authService.Register(model);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                
                _responseDto.IsSuccess = false;
                _responseDto.Message = errorMessage;
                return BadRequest(_responseDto);
            }

           
            return Ok(_responseDto);
        }

       
        [HttpPost("api/users/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {

            //Login
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
              
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or password is incorrect";
                return BadRequest(_responseDto);
            }

           
            _responseDto.Result = loginResponse;
            return Ok(_responseDto);
        }

       
        [HttpGet("/api/users/{id}")]
        public async Task<ActionResult> GetUser(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");

            //finding by id
            var user = await this._authService.GetAsync(id);
            if (user == null)
            {
                
                throw new Exception($"UserId{id} is not Found");
            }

            
            return Ok(user);
        }

       
        [HttpPut("/api/users/updateDetails")]
        public async Task<IActionResult> UpdateUserNameByEmail([FromBody] UpdateUserDetailsDto updateUserDetails)
        {
            if (updateUserDetails == null || string.IsNullOrEmpty(updateUserDetails.Email) || string.IsNullOrEmpty(updateUserDetails.NewUserName))
            {
                
                return BadRequest("Invalid input");
            }

            //updating
            var success = await _authService.UpdateUserNameByEmailAsync(updateUserDetails);
            if (success)
            {
                
                return Ok("Username updated successfully");
            }
            else
            {
                
                return NotFound("User not found or update failed");
            }
        }

        [HttpDelete("/api/users/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
               
                return BadRequest("User ID is required");
            }

            //deleting by id
            var success = await _authService.DeleteUserByIdAsync(userId);
            if (success)
            {
                
                return Ok("User deleted successfully");
            }
            else
            {
                
                return NotFound("User not found or delete failed");
            }
        }
    }
}