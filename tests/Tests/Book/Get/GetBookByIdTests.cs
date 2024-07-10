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
    public class GetBookByIdTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidBookId_WhenGettingBook_ThenReturnsOkResultWithBookDTO()
        {
            // Arrange            
            var controller = new BookController(DbContext, _mapper);
            var bookId = Guid.NewGuid();

            var book = new Domain.Models.Entities.Book
            {
                BookId = bookId,
                Title = "Test Book",
                Author = "Author",
                Publisher = "publisher",
                Year = 1234,
                Genre = Genre.Science
            };

            var bookDTO = new BookDTO()
            {
                Code = bookId,
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                Year = book.Year,
                Genre = book.Genre
            };


            // Act
            await controller.CreateBook(bookDTO);
            var result = await controller.GetBook(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<BookDTO>(okResult.Value);

            Assert.Equal(bookDTO.Code, returnedBooks.Code);
        }

        [Fact]
        public async Task GivenEmptyBookId_WhenGettingBook_ThenReturnsBadRequest()
        {
            // Arrange
            
            var controller = new BookController(DbContext, _mapper);

            // Act
            var result = await controller.GetBook(Guid.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentBook_WhenGettingBook_ThenReturnsNotFoundResult()
        {
            // Arrange
            
            var controller = new BookController(DbContext, _mapper);
            var bookId = Guid.NewGuid();

            // Act
            var result = await controller.GetBook(bookId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
