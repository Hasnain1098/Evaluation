using System.Net;
namespace Evaluation.Web.DTOs
{
    public class ResponseDto
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
        public bool IsSuccess { get; set; } = true;
        public bool IsException { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public object? Result { get; set; }
    }
}