using AutoMapper;
using Evaluation.DB;
using Evaluation.DTOs.Auth;
using Evaluation.DTOs.Sales;
using Evaluation.Models;
using Evaluation.Services.IService;
using Evaluation.Utilities;
using Evaluation.Web.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
    public class SaleService : ISaleService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SaleService> _logger;

        //private readonly RoleManager<IdentityRole> _roleManager;

        public SaleService(AppDbContext dbContext, IMapper mapper, ILogger<SaleService> logger)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseDto> CreateAsync(CreateSaleDto createSaleDto)
        {

            try
            {
                // Map DTO to entity
                var sale = _mapper.Map<Sale>(createSaleDto);

                // Add entity to context
                await _dbContext.Sales.AddAsync(sale);
                await _dbContext.SaveChangesAsync();

                // Return a success response
                return ResponseHelper.CreateResponse(true, false, ResponseMessages.Successful, createSaleDto);
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update exceptions specifically
                // _logger.LogError(dbEx, "Database update error while creating sale.");
                return ResponseHelper.CreateResponse(false, true, ResponseMessages.DbExceptionMessage, null);
            }
            catch (Exception ex)
            {
                // Handle other general exceptions
                // _logger.LogError(ex, "An error occurred while creating the sale.");
                return ResponseHelper.CreateResponse(false, true, ResponseMessages.ExceptionMessage, null);
            }

        }


        public async Task<ResponseDto?> GetAsync(int? saleId)
        {
            if (saleId == null)
            {
                // Handle the case where saleId is null (optional)
                throw new ArgumentException("Sale ID cannot be null", nameof(saleId));
            }

            Sale? sale = await _dbContext.Sales
                                        .FirstOrDefaultAsync(c => c.Id == saleId);

            if (sale == null)
            {
                // Return a specific response indicating the sale was not found
                return ResponseHelper.CreateResponse(false, true, "Sale not found", null);
            }

            return ResponseHelper.CreateResponse(true, false, ResponseMessages.Successful, _mapper.Map<GetSaleDto>(sale));
        }


        public async Task<ResponseDto?> DeleteAsync(int? saleId)
        {

            Sale? sale = null;
            sale = await _dbContext.Sales.FindAsync(saleId);
            if (sale != null)
            {
                _dbContext.Sales.Remove(sale);
                await _dbContext.SaveChangesAsync(); 
                return ResponseHelper.CreateResponse(true, false, ResponseMessages.Successful, sale);
            }
            else
            {
                throw new NullReferenceException($"category not found");
                return ResponseHelper.CreateResponse(false, true, ResponseMessages.ExceptionMessage, sale);
            }
            
        }

        public async Task<ResponseDto?> UpdateAsync(int id,UpdateSaleDto updateSaleDto)
        {

            try
            {
                // Validate input
                if (updateSaleDto == null)
                {
                    return ResponseHelper.CreateResponse(false, true, ResponseMessages.InvalidData, null);
                }

                // Find the sale by ID
                Sale? sale = await _dbContext.Sales.FindAsync(id);

                // Check if the sale exists
                if (sale == null)
                {
                    return ResponseHelper.CreateResponse(false, true, ResponseMessages.NotFound, null);
                }

                // Map the updated properties from DTO to entity
                _mapper.Map(updateSaleDto, sale);

                // Update the sale entity
                _dbContext.Sales.Update(sale);
                await _dbContext.SaveChangesAsync();

                // Return success response with updated sale
                return ResponseHelper.CreateResponse(true, false, ResponseMessages.Successful, _mapper.Map<UpdateSaleDto>(sale));
            }
            catch (DbUpdateConcurrencyException ex)
            {
               
              
                return ResponseHelper.CreateResponse(false, true, ResponseMessages.DbExceptionMessage, ex.Message);
            }
            catch (DbUpdateException ex)
            {
               
                return ResponseHelper.CreateResponse(false, true, ResponseMessages.DbExceptionMessage, ex.Message);
            }
            catch (Exception ex)
            {
                
                return ResponseHelper.CreateResponse(false, true, ResponseMessages.ExceptionMessage, ex.Message);
            }
        }

        public async Task<ResponseDto?> GetRecordFromFilters(DateTime? startDateRange, DateTime? endDateRange, int? representativeId)
        {
            try
            {
            
                // Validate date ranges
                if (startDateRange.HasValue && endDateRange.HasValue)
                {
                    if (startDateRange.Value > endDateRange.Value)
                    {
                        return ResponseHelper.CreateResponse(false, true, "Start date cannot be after end date", null);
                    }
                }

                // Build the base query
                IQueryable<Sale> query = _dbContext.Sales.AsQueryable();

                // Apply date range filter if provided
                if (startDateRange.HasValue)
                {
                    var startDate = startDateRange.Value.Date; // Normalize to start of the day
                    query = query.Where(s => s.SaleDate >= startDate);
                }

                if (endDateRange.HasValue)
                {
                    var endDate = endDateRange.Value.Date.AddDays(1).AddTicks(-1); // Normalize to end of the day
                    query = query.Where(s => s.SaleDate <= endDate);
                }

                
                if (representativeId != 0)
                {
                   
                    var representativeExists = await _dbContext.Sales
                        .AnyAsync(r => r.RepresentativeId == representativeId);

                    if (!representativeExists)
                    {
                        return ResponseHelper.CreateResponse(false, true, "Representative not found", null);
                    }

                    query = query.Where(s => s.RepresentativeId == representativeId);
                }

               
                var salesRecords = await query.ToListAsync();

               
                var salesRecordsDto = _mapper.Map<List<Sale>>(salesRecords);

                
                return ResponseHelper.CreateResponse(true, false, ResponseMessages.Successful, salesRecordsDto);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error retrieving sales records");

               
                return ResponseHelper.CreateResponse(false, true, ResponseMessages.ExceptionMessage, ex.Message);
            }
        }
    }
}
