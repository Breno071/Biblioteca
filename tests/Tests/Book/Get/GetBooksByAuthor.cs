using API.Controllers;
using AutoMapper;
using Domain.Enums;
using Domain.Models.DTO;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var author = "Irineu";

            var books = new List<Domain.Models.Entities.Book>
            {
                new() { Code = Guid.NewGuid(), Title = "Book 1",  Author = "Irineu", Publisher = "Publisher", Year = 123, Genre = Genre.ScienceFiction },
                new() { Code = Guid.NewGuid(), Title = "Book 2",  Author = "Irineu", Publisher = "Publisher", Year = 123, Genre = Genre.ScienceFiction }
            };

            DbContext.Books.AddRange(books);
            DbContext.SaveChanges();

            // Act
            var result = await controller.GetBooksByAuthor(author);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<Domain.Models.Entities.Book>>(okResult.Value);

            Assert.Equal(books.Count, returnedBooks.Count);
            for (int i = 0; i < books.Count; i++)
            {
                Assert.Equal(books[i].Code, returnedBooks[i].Code);
                Assert.Equal(books[i].Title, returnedBooks[i].Title);
                Assert.Equal(books[i].Author, returnedBooks[i].Author);
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
            var returnedBooks = Assert.IsType<List<Domain.Models.Entities.Book>>(okResult.Value);

            Assert.Empty(returnedBooks);
        }
    }
}
