using API.Controllers;
using AutoMapper;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Book.Get
{
    public class GetBooksTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidBookId_WhenGettingBook_ThenReturnsOkResultWithBookDTO()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var bookId = Guid.NewGuid();

            var book = new Domain.Models.Entities.Book { Code = bookId, Title = "Test Book" };
            DbContext.Books.Add(book);
            DbContext.SaveChanges();

            var bookDTO = new BookDTO { Code = bookId, Title = "Test Book" };
            mapperMock.Setup(x => x.Map<BookDTO>(It.IsAny<Domain.Models.Entities.Book>())).Returns(bookDTO);

            // Act
            var result = await controller.GetBook(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBookDTO = Assert.IsType<BookDTO>(okResult.Value);

            Assert.Equal(bookDTO.Code, returnedBookDTO.Code);
            Assert.Equal(bookDTO.Title, returnedBookDTO.Title);
        }

        [Fact]
        public async Task GivenEmptyBookId_WhenGettingBook_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);

            // Act
            var result = await controller.GetBook(Guid.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentBook_WhenGettingBook_ThenReturnsNotFoundResult()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var bookId = Guid.NewGuid();

            // Act
            var result = await controller.GetBook(bookId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
