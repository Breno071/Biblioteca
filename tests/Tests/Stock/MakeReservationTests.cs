using API.Controllers;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Stock
{
    public class MakeReservationTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidParameters_WhenMakingReservation_ThenReturnsOkResultWithReservation()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);
            var clientCode = Guid.NewGuid();
            var bookCodes = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var returnDate = DateTime.Now.AddDays(14);

            // Act
            DbContext.Clients.Add(new Domain.Models.Entities.Client { Code = clientCode });
            DbContext.Books.AddRange(bookCodes.Select(code => new Domain.Models.Entities.Book { Code = code, Stock = 1 }));
            DbContext.SaveChanges();
            var result = await controller.MakeReservation(clientCode, bookCodes, returnDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var reservation = Assert.IsType<Reservation>(okResult.Value);

            Assert.Equal(clientCode, reservation.Client.Code);
            Assert.Equal(bookCodes.Count, reservation.Books.Count);
            Assert.Equal(returnDate, reservation.ReturnDate);
        }

        [Fact]
        public async Task GivenEmptyClientId_WhenMakingReservation_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);

            // Act
            var result = await controller.MakeReservation(Guid.Empty, [Guid.NewGuid()], DateTime.Now.AddDays(14));

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNullBookCodes_WhenMakingReservation_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);

            // Act
            var result = await controller.MakeReservation(Guid.NewGuid(), null, DateTime.Now.AddDays(14));

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenEmptyBookCodes_WhenMakingReservation_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);

            // Act
            var result = await controller.MakeReservation(Guid.NewGuid(), [], DateTime.Now.AddDays(14));

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentClient_WhenMakingReservation_ThenReturnsNotFoundResult()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);

            // Act
            var result = await controller.MakeReservation(Guid.NewGuid(), [Guid.NewGuid()], DateTime.Now.AddDays(14));

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentBook_WhenMakingReservation_ThenReturnsNotFoundResult()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);
            var clientCode = Guid.NewGuid();

            // Act
            DbContext.Clients.Add(new Domain.Models.Entities.Client { Code = clientCode });
            DbContext.SaveChanges();
            var result = await controller.MakeReservation(clientCode, [Guid.NewGuid()], DateTime.Now.AddDays(14));

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task MakeReservation_GivenBookWithZeroStock_WhenMakingReservation_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);
            var clientCode = Guid.NewGuid();
            var bookCode = Guid.NewGuid();

            // Act
            DbContext.Clients.Add(new Domain.Models.Entities.Client { Code = clientCode });
            DbContext.Books.Add(new Domain.Models.Entities.Book { Code = bookCode, Stock = 0 });
            DbContext.SaveChanges();
            var result = await controller.MakeReservation(clientCode, [bookCode], DateTime.Now.AddDays(14));

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
