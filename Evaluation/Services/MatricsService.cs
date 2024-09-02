using Evaluation.DB;
using Evaluation.Models;
using Evaluation.Services.IService;
using Evaluation.Utilities;
using Evaluation.Web.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
    public class MatricsService : IMatricsService
    {
        private readonly AppDbContext _appDbContext;

        public MatricsService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseDto> GetMatricsAsync()
        {
            try
            {
                var sales = await _appDbContext.Sales.ToListAsync();

                if (sales == null || !sales.Any())
                {
                    return ResponseHelper.CreateResponse(false, true, ResponseMessages.NotFound, null);
                }

                var totalSales = sales.Count();
                var averageSaleAmount = sales.Average(s => s.Amount);

                var metrics = new Matrics
                {
                    TotalSale = totalSales,
                    AverageSaleAmount = averageSaleAmount
                };

                return ResponseHelper.CreateResponse(true, false, ResponseMessages.Successful, metrics);
            }
            catch
            {
                return ResponseHelper.CreateResponse(false, true, ResponseMessages.ExceptionMessage, null);
            }
        }
    }
}