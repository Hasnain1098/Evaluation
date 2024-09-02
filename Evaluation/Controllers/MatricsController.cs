using Evaluation.Models;
using Evaluation.Services.IService;
using Evaluation.Web.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Evaluation.Controllers
{
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class MatricsController : Controller
    {
        private readonly IMatricsService _matricsService;

        public MatricsController(IMatricsService matricsService)
        {
            _matricsService = matricsService;
        }

        [HttpGet("/api/metrics/sales-summary")]
        public async Task<ResponseDto> GetSummary()
        {
            var summary = await _matricsService.GetMatricsAsync();

            if (summary == null)
            {
                throw new Exception("Summary is not found");
            }

            summary.StatusCode = summary.Result != null
                ? (int)HttpStatusCode.OK
                : (int)HttpStatusCode.InternalServerError;

            return summary;
        }
    }
}