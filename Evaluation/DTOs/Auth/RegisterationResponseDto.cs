namespace Evaluation.DTOs.Auth
{
    public class RegisterationResponseDto
    {
        public object? Result { get; set; }
        public bool? IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        
    }
}
