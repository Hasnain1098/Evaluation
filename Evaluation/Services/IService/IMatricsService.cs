using Evaluation.Web.DTOs;

namespace Evaluation.Services.IService
{
    public interface IMatricsService
    {
        Task<ResponseDto> GetMatricsAsync();
    }
}
