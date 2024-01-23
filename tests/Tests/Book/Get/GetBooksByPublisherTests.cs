using API.Controllers;
using AutoMapper;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Book.Get
{
    public class GetBooksByPublisherTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidPublisher_WhenGettingBooks_ThenReturnsOkResultWithBookDTOs()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var publisher = "Test Publisher";

            var books = new List<Domain.Models.Entities.Book>
            {
                new() { Code = Guid.NewGuid(), Title = "Book 1", Publisher = publisher },
                new() { Code = Guid.NewGuid(), Title = "Book 2", Publisher = publisher },
                new() { Code = Guid.NewGuid(), Title = "Book 3", Publisher = publisher }
            };

            DbContext.Books.AddRange(books);
            DbContext.SaveChanges();

            var bookDTOs = books.Select(book => new BookDTO { Code = book.Code, Title = book.Title, Publisher = book.Publisher }).ToList();
            mapperMock.Setup(x => x.Map<List<BookDTO>>(It.IsAny<List<Domain.Models.Entities.Book>>())).Returns(bookDTOs);

            // Act
            var result = await controller.GetBooksByPublisher(publisher);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBookDTOs = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Equal(bookDTOs.Count, returnedBookDTOs.Count);
            for (int i = 0; i < bookDTOs.Count; i++)
            {
                Assert.Equal(bookDTOs[i].Code, returnedBookDTOs[i].Code);
                Assert.Equal(bookDTOs[i].Title, returnedBookDTOs[i].Title);
                Assert.Equal(bookDTOs[i].Publisher, returnedBookDTOs[i].Publisher);
            }
        }

        [Fact]
        public async Task GivenEmptyPublisher_WhenGettingBooks_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);

            // Act
            var result = await controller.GetBooksByPublisher(string.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNullPublisher_WhenGettingBooks_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);

            // Act
            var result = await controller.GetBooksByPublisher(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNoMatchingBooks_WhenGettingBooks_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var publisher = "Nonexistent Publisher";

            // Act
            var result = await controller.GetBooksByPublisher(publisher);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBookDTOs = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Empty(returnedBookDTOs);
        }
    }
}
