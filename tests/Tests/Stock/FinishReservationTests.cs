using API.Controllers;
using Domain.Enums;
using Domain.Models.DTO;
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
            var stockController = new StockController(DbContext, _reservationService);
            var clientController = new ClientController(DbContext, _mapper);
            var bookController = new BookController(DbContext, _mapper);
            var reservationController = new ReservationController(DbContext, _reservationService);

            var client = new ClientDTO
            { 
                Code = Guid.NewGuid() ,
                Name = "Irineu",
                Email = Guid.NewGuid() + "irineu@email.com",
            };

            var books = new List<BookDTO>
            {
                new()
                {
                    Code = Guid.NewGuid(),
                    Title = "New Book",
                    Author = "Author",
                    Publisher = "Publisher",
                    Genre = Genre.Comedy,
                    Year = 2022
                }
            };

            // Act
            DbContext.ChangeTracker.Clear();
            await clientController.CreateClient(client);
            await bookController.CreateBook(books[0]);

            DbContext.ChangeTracker.Clear();
            await stockController.SetBookStock(books[0].Code, 10);

            var reservationResult = await reservationController.MakeReservation(client.Code, [books[0].Code], null);

            // Assert
            Assert.IsType<OkObjectResult>(reservationResult);
            var reservation = Assert.IsType<OkObjectResult>(reservationResult).Value as Reservation;
            Assert.NotNull(reservation);
            var reservationCode = reservation.Code;
            var result = await reservationController.FinishReservation(reservationCode);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GivenEmptyReservationCode_WhenFinishingReservation_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new ReservationController(DbContext, _reservationService);

            // Act
            var result = await controller.FinishReservation(Guid.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentReservation_WhenFinishingReservation_ThenReturnsNotFoundResult()
        {
            // Arrange
            var controller = new ReservationController(DbContext, _reservationService);

            // Act
            var result = await controller.FinishReservation(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
