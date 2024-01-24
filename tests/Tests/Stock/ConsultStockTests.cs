using API.Controllers;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Stock
{
    public class ConsultStockTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        public async Task GivenValidBookId_WhenConsultingStock_ThenReturnsOkResultWithStock(int stock)
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);
            var bookId = Guid.NewGuid();
            DbContext.Books.Add(new Domain.Models.Entities.Book 
            { 
                Code = bookId, 
                Author = "Teste", 
                Genre = Genre.Adventure,
                Publisher = "Publisher", 
                Title = "Title",
                Year = 123, Stock = stock 
            });
            DbContext.SaveChanges();

            // Act
            var result = await controller.ConsultBookStock(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(stock, okResult.Value);
        }

        [Fact]
        public async Task GivenEmptyId_WhenConsultingStock_ThenReturnsBadRequest()
        {
            // Arrange

            var controller = new StockController(DbContext, _reservationService);

            // Act
            var result = await controller.ConsultBookStock(Guid.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentBook_WhenConsultingBookStock_ThenReturnsNotFoundResult()
        {
            // Arrange

            var controller = new StockController(DbContext, _reservationService);
            var bookId = Guid.NewGuid();

            // Act
            var result = await controller.ConsultBookStock(bookId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
