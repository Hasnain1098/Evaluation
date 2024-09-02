using Evaluation.Web.DTOs;

namespace Evaluation.Utilities
{
    public static class ResponseHelper
    {
         
        /// <summary>
        /// Creates the response for CRUD operations in the repository.
        /// </summary>
        /// <param name="isSuccess">Indicates whether the operation was successful or not.</param>
        /// <param name="isException">Indicates whether the exception was caught or not.</param>
        /// <param name="message">A message providing details about the result of the CRUD operation.</param>
        /// <param name="result">Optional result object, containing the data returned from the operation if successful, Null for failure.</param>
        /// <returns>Returns a <see cref="ResponseDTO"/> containing the success status, message, and result.</returns>
        public static ResponseDto CreateResponse(bool isSuccess, bool isException, string message, object? result = null)
        {
            return new ResponseDto
            {
                IsException = isException,
                IsSuccess = isSuccess,
                Message = message,
                Result = result
            };
        }
    }
}
