using API.Controllers;
using AutoMapper;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Book.Get
{
    public class GetBooksByAuthor(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidAuthor_WhenGettingBooksByAuthor_ThenReturnsOkResultWithBookDTOs()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var author = "Test Author";

            var books = new List<Domain.Models.Entities.Book>
            {
                new() { Code = Guid.NewGuid(), Title = "Book 1", Author = author },
                new() { Code = Guid.NewGuid(), Title = "Book 2", Author = author },
                new() { Code = Guid.NewGuid(), Title = "Book 3", Author = author }
            };
            DbContext.Books.AddRange(books);
            DbContext.SaveChanges();
            var bookDTOs = books.Select(book => new BookDTO { Code = book.Code, Title = book.Title, Author = book.Author }).ToList();
            mapperMock.Setup(x => x.Map<List<BookDTO>>(It.IsAny<List<Domain.Models.Entities.Book>>())).Returns(bookDTOs);

            // Act
            var result = await controller.GetBooksByAuthor(author);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBookDTOs = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Equal(bookDTOs.Count, returnedBookDTOs.Count);
            for (int i = 0; i < bookDTOs.Count; i++)
            {
                Assert.Equal(bookDTOs[i].Code, returnedBookDTOs[i].Code);
                Assert.Equal(bookDTOs[i].Title, returnedBookDTOs[i].Title);
                Assert.Equal(bookDTOs[i].Author, returnedBookDTOs[i].Author);
            }
        }

        [Fact]
        public async Task GivenEmptyAuthor_WhenGettingBooksByAuthor_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);

            // Act
            var result = await controller.GetBooksByAuthor(string.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNullAuthor_WhenGettingBooksByAuthor_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);

            // Act
            var result = await controller.GetBooksByAuthor(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNoMatchingBooks_WhenGettingBooksByAuthor_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var author = "Nonexistent Author";

            // Act
            var result = await controller.GetBooksByAuthor(author);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBookDTOs = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Empty(returnedBookDTOs);
        }
    }
}
