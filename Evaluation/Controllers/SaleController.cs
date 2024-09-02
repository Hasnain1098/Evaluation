using Evaluation.DTOs.Sales;
using Evaluation.Models;
using Evaluation.Services.IService;
using Evaluation.Web.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Evaluation.Controllers
{
    
    [ApiController]
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;

        
        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

     
        [HttpPost("/api/sales")]
        public async Task<ResponseDto> CreateSale(CreateSaleDto createSaleDto)
        {
            // Validate the model state
            if (!ModelState.IsValid)
                return new ResponseDto
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Model state is invalid."
                };

            // Call the service to create the sale
            var result = await _saleService.CreateAsync(createSaleDto);

            // Set the status code based on the presence of result in the response
            if (result.Result != null)
                result.StatusCode = (int)HttpStatusCode.OK; 
            else
                result.StatusCode = (int)HttpStatusCode.InternalServerError;

            
            return result;
        }

      
        [HttpGet("/api/sales/{saleId}")]
        public async Task<ResponseDto> GetSale(int saleId)
        {
            //  to get the sale by ID
            var sale = await this._saleService.GetAsync(saleId);

            // Check if the sale is null and throw an exception if not found
            if (sale == null)
            {
                throw new Exception($"SaleId {saleId} is not found");
            }

            // Set the status code based on the presence of result in the response
            if (sale.Result != null)
                sale.StatusCode = (int)HttpStatusCode.OK;
            else
                sale.StatusCode = (int)HttpStatusCode.InternalServerError; 

            
            return sale;
        }

        
        [HttpDelete("/api/sales/{id}")]
        public async Task<ResponseDto> DeleteSale(int id)
        {
            // Call the service to delete the sale by ID
            var sale = await this._saleService.DeleteAsync(id);

            // Check if the sale is null and throw an exception if not found
            if (sale == null)
            {
                throw new Exception($"SaleId {id} is not found");
            }

            // Set the status code based on the presence of result in the response
            if (sale.Result != null)
                sale.StatusCode = (int)HttpStatusCode.OK; 
            else
                sale.StatusCode = (int)HttpStatusCode.InternalServerError; 

            
            return sale;
        }

        
        [HttpPut("/api/sales/{id}")]
        public async Task<ResponseDto> UpdateSale(int id, [FromBody] UpdateSaleDto updateSaleDto)
        {
            // Call the service to update the sale
            var sale = await _saleService.UpdateAsync(id, updateSaleDto);

            // Check if the sale is null and throw an exception if not found
            if (sale == null)
            {
                throw new Exception($"SaleId {id} is not found");
            }

            // Set the status code based on the presence of result in the response
            if (sale.Result != null)
                sale.StatusCode = (int)HttpStatusCode.OK; 
            else
                sale.StatusCode = (int)HttpStatusCode.InternalServerError; 

            
            return sale;
        }


        [HttpGet("/api/sales")]
        public async Task<ResponseDto> GetFiltered(DateTime startDateRange, DateTime? endDateRange, int representativeId)
        {
            // Call the service to get sales records based on filters
            var sales = await this._saleService.GetRecordFromFilters(startDateRange,endDateRange,representativeId);

            // Check if the sales records are null and throw an exception if not found
            if (sales == null)
            {
                throw new Exception("Record not found");
            }

            // Set the status code based on the presence of result in the response
            if (sales.Result != null)
                sales.StatusCode = (int)HttpStatusCode.OK; 
            else
                sales.StatusCode = (int)HttpStatusCode.InternalServerError;

            return sales;
        }
    }
}