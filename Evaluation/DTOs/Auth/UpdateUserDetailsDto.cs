namespace Evaluation.DTOs.Auth
{
    public class UpdateUserDetailsDto
    {
        public string Email { get; set; } // Email to find the user
        public string NewUserName { get; set; }
    }
}
