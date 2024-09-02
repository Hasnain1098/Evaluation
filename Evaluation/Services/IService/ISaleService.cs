using Evaluation.DTOs.Sales;
using Evaluation.Web.DTOs;

namespace Evaluation.Services.IService
{
    public interface ISaleService
    {
        Task<ResponseDto> CreateAsync(CreateSaleDto createSaleDto);

        Task<ResponseDto?> GetAsync(int? saleId);

        Task<ResponseDto?> DeleteAsync(int? saleId);

        Task<ResponseDto?> UpdateAsync(int id,UpdateSaleDto updateSaleDto);

        Task<ResponseDto?> GetRecordFromFilters(DateTime? startDateRange,DateTime? endDateRange, int? representativeId);
    }
}
