using API.Controllers;
using Domain.Enums;
using Domain.Models.DTO;
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
            var stockController = new StockController(DbContext, _reservationService);
            var reservationController = new ReservationController(DbContext, _reservationService);
            var clientController = new ClientController(DbContext, _mapper);
            var bookController = new BookController(DbContext, _mapper);

            var clientCode = Guid.NewGuid();
            var bookCodes = new List<Guid> 
            { 
                Guid.NewGuid(), 
                Guid.NewGuid() 
            };
            var returnDate = DateTime.Now.AddDays(14);

            ClientDTO clientDTO = new ()
            {
                Code = clientCode,
                Name = "Irineu",
                Email = Guid.NewGuid() + "irineu@email.com",
            };

            var books = bookCodes.Select(code => new BookDTO
            {
                Code = code,
                Title = "New Book",
                Author = "Author",
                Publisher = "Publisher",
                Genre = Genre.Comedy,
                Year = 2022
            }).ToList();

            // Act
            await clientController.CreateClient(clientDTO);
            await bookController.CreateBook(books[0]);
            await bookController.CreateBook(books[1]);

            DbContext.ChangeTracker.Clear();
            await stockController.SetBookStock(books[0].Code, 10);
            await stockController.SetBookStock(books[1].Code, 10);
            var result = await reservationController.MakeReservation(clientCode, bookCodes, returnDate);

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
            var controller = new ReservationController(DbContext, _reservationService);

            // Act
            var result = await controller.MakeReservation(Guid.Empty, [Guid.NewGuid()], DateTime.Now.AddDays(14));

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenEmptyBookCodes_WhenMakingReservation_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new ReservationController(DbContext, _reservationService);

            // Act
            var result = await controller.MakeReservation(Guid.NewGuid(), [], DateTime.Now.AddDays(14));

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentClient_WhenMakingReservation_ThenReturnsNotFoundResult()
        {
            // Arrange
            var controller = new ReservationController(DbContext, _reservationService);

            // Act
            var result = await controller.MakeReservation(Guid.NewGuid(), [Guid.NewGuid()], DateTime.Now.AddDays(14));

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentBook_WhenMakingReservation_ThenReturnsNotFoundResult()
        {
            // Arrange
            var controller = new ReservationController(DbContext, _reservationService);
            var clientCode = Guid.NewGuid();

            // Act
            DbContext.Clients.Add(new Domain.Models.Entities.Client 
            { 
                Code = clientCode ,
                Name = "Irineu",
                Email = "irineu@email.com",
            });
            DbContext.SaveChanges();
            var result = await controller.MakeReservation(clientCode, [Guid.NewGuid()], DateTime.Now.AddDays(14));

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task MakeReservation_GivenBookWithZeroStock_WhenMakingReservation_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new ReservationController(DbContext, _reservationService);
            var clientCode = Guid.NewGuid();
            var bookCode = Guid.NewGuid();

            // Act
            DbContext.Clients.Add(new Domain.Models.Entities.Client 
            { 
                Code = clientCode ,
                Name = "Irineu",
                Email = "irineu@email.com",
            });
            DbContext.Books.Add(new Domain.Models.Entities.Book 
            {
                Code = bookCode,
                Title = "New Book",
                Author = "Author",
                Publisher = "Publisher",
                Genre = Genre.Comedy,
                Year = 2022
            });
            DbContext.SaveChanges();
            var result = await controller.MakeReservation(clientCode, [bookCode], DateTime.Now.AddDays(14));

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
