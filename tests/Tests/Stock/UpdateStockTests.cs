using API.Controllers;
using Domain.Interfaces;
using Domain.Models.Entities;
using Infraestructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Stock
{
    public class UpdateStockTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {

        /// <summary>
        /// Padrão de nome : Given_When_Then
        /// </summary>
        [Fact]
        public async Task GivenValidInput_WhenUpdatingStock_ThenShouldReturnOkResult()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);
            var bookId = Guid.NewGuid();
            var stock = 10;

            // Act
            var book = DbContext.Books.Add(new Domain.Models.Entities.Book()
            {
                Author = "Author",
                Title = "Title",
                Code = bookId
            });
            var result = await controller.SetBookStock(bookId, stock);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(stock, book.Entity.Stock);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task GivenNegativeStock_WhenUpdatingStock_ThenShouldReturnBadRequestResult(int stock)
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);
            var bookId = Guid.NewGuid();

            // Act
            var result = await controller.SetBookStock(bookId, stock);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNotInsertedBookCode_WhenUpdatingStock_ThenShouldReturnNotFoundResult()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);
            var bookId = Guid.NewGuid();
            var stock = 5;

            var book = await DbContext.Books.FirstOrDefaultAsync(x => x.Code == bookId);

            // Act
            var result = await controller.SetBookStock(bookId, stock);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Null(book);
        }

        [Fact]
        public async Task GivenEmptyGuid_WhenUpdatingStock_ThenShouldReturnNotFoundResult()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);
            var bookId = Guid.NewGuid();
            var stock = 5;

            var book = await DbContext.Books.FirstOrDefaultAsync(x => x.Code == bookId);

            // Act
            var result = await controller.SetBookStock(bookId, stock);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Null(book);
        }
    }
}
