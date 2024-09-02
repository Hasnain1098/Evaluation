using AutoFixture;
using Evaluation.Controllers;
using Evaluation.DTOs.Sales;
using Evaluation.Models;
using Evaluation.Services.IService;
using Evaluation.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace API.Test
{
    public class SalesControllerTests
    {
        private readonly Mock<ISaleService> _mockSaleService;
        private readonly SaleController _controller;
        private readonly Fixture _fixture;

        public SalesControllerTests()
        {
            _mockSaleService = new Mock<ISaleService>();
            _controller = new SaleController(_mockSaleService.Object);
            _fixture = new Fixture();

            // Customize AutoFixture to generate decimal values within a specific range for both CreateSaleDTO and UpdateSaleDTO
            _fixture.Customize<CreateSaleDto>(c => c
                .With(dto => dto.Amount, _fixture.Create<decimal>() % 1000m)); // Limit to a range
            _fixture.Customize<UpdateSaleDto>(c => c
                .With(dto => dto.Amount, _fixture.Create<decimal>() % 1000m));
            _fixture.Customize<GetSaleDto>(c => c
                .With(dto => dto.Amount, _fixture.Create<decimal>() % 1000m));
            _fixture.Customize<Sale>(c => c
                .With(dto => dto.Amount, _fixture.Create<decimal>() % 1000m)); // Limit to a range
        }


        //Create Sale
        [Fact]
        public async Task CreateSale_ServiceReturnsSuccess_ReturnsOk()
        {
            // Arrange
            var createSaleDto = _fixture.Create<CreateSaleDto>();
            var saleDto = _fixture.Create<GetSaleDto>();

            var response = new ResponseDto { IsSuccess = true, IsException = false, Result = saleDto, StatusCode = (int)HttpStatusCode.OK };
            _mockSaleService.Setup(s => s.CreateAsync(createSaleDto)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateSale(createSaleDto);

            // Assertiion
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task GetAsync_SaleExists_ReturnsOk()
        {
            // Arrange
            //var sale = _fixture.Create<Sale>();
            var saleDto = _fixture.Create<GetSaleDto>();

            var response = new ResponseDto { IsSuccess = true, IsException = false, Result = saleDto, StatusCode = (int)HttpStatusCode.OK };
            _mockSaleService.Setup(s => s.GetAsync(saleDto.Id)).ReturnsAsync(response);
            // Act
            var result = await _controller.GetSale(saleDto.Id);

            //Assert
            var okResult = Assert.IsType<ResponseDto>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(response, okResult);
        }

        //[Fact]
        //public async Task DeleteAsync_SaleExists_ReturnsOk()
        //{
        //    // Arrange
        //    //var sale = _fixture.Create<Sale>();
        //    var createSaleDto = _fixture.Create<CreateSaleDto>();

        //    var response = new ResponseDto { IsSuccess = true, IsException = false, Result = createSaleDto, StatusCode = (int)HttpStatusCode.OK };
        //    _mockSaleService.Setup(s => s.CreateAsync(createSaleDto)).ReturnsAsync(response);
        //    // Act
        //    var addedSale = await _controller.CreateSale(createSaleDto);

        //    var okResult = Assert.IsType<ResponseDto>(addedSale);

        //    var deletedSale = await _controller.DeleteSale((int)createSaleDto.Id);


        ////Assert
        //var okResult = Assert.IsType<ResponseDto>(result);
        //Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        //Assert.Equal(response, okResult);
    }
}
