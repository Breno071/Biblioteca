using API.Controllers;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tests.Stock
{
    public class FinishReservationTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidReservationCode_WhenFinishingReservation_ThenReturnsOkResult()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);
            var reservationCode = Guid.NewGuid();

            var client = new Domain.Models.Entities.Client { Code = Guid.NewGuid() };
            var books = new List<Domain.Models.Entities.Book> { new() { Code = Guid.NewGuid(), Stock = 1 } };

            var reservation = new Reservation
            {
                Code = reservationCode,
                Client = client,
                Books = books
            };


            // Act
            DbContext.Reservations.Add(reservation);
            DbContext.SaveChanges();
            var result = await controller.FinishReservation(reservationCode);

            // Assert
            Assert.IsType<OkResult>(result);

            var updatedBooks = DbContext.Books.ToList();
            foreach (var book in updatedBooks)
            {
                Assert.Equal(2, book.Stock);
            }
        }

        [Fact]
        public async Task GivenEmptyReservationCode_WhenFinishingReservation_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);

            // Act
            var result = await controller.FinishReservation(Guid.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentReservation_WhenFinishingReservation_ThenReturnsNotFoundResult()
        {
            // Arrange
            var controller = new StockController(DbContext, _reservationService);

            // Act
            var result = await controller.FinishReservation(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
