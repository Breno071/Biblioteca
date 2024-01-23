using API.Controllers;
using AutoMapper;
using Domain.Enums;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Book.Create
{
    public class CreateBookTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenNewBookDTO_WhenCreatingBook_ThenReturnsOkResultWithCreatedBook()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);

            var newBookDTO = new BookDTO { Code = Guid.NewGuid(), Title = "New Book", Author = "Author", Publisher = "Publisher", Genre = Genre.Comedy, Year = 2022 };
            mapperMock.Setup(x => x.Map<Domain.Models.Entities.Book>(It.IsAny<BookDTO>())).Returns(new Domain.Models.Entities.Book
            {
                Code = newBookDTO.Code,
                Title = newBookDTO.Title,
                Author = newBookDTO.Author,
                Year = newBookDTO.Year,
                Publisher = newBookDTO.Publisher,
                Genre = newBookDTO.Genre
            });

            // Act
            var result = await controller.CreateBook(newBookDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var createdBook = Assert.IsType<Domain.Models.Entities.Book>(okResult.Value);

            Assert.Equal(newBookDTO.Code, createdBook.Code);
            Assert.Equal(newBookDTO.Title, createdBook.Title);
            Assert.Equal(newBookDTO.Author, createdBook.Author);
            Assert.Equal(newBookDTO.Year, createdBook.Year);

            // Check if the book was actually created in the database
            var createdBookInDb = DbContext.Books.FirstOrDefault(x => x.Code == newBookDTO.Code);
            Assert.NotNull(createdBookInDb);
            Assert.Equal(newBookDTO.Title, createdBookInDb.Title);
            Assert.Equal(newBookDTO.Author, createdBookInDb.Author);
            Assert.Equal(newBookDTO.Year, createdBookInDb.Year);
        }

        [Fact]
        public async Task GivenExistingBookDTO_WhenCreatingBook_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);

            var existingBookDTO = new BookDTO { Code = Guid.NewGuid(), Title = "Existing Book", Author = "Author", Publisher = "Publisher", Genre = Genre.Comedy, Year = 2022 };
            var existingBook = new Domain.Models.Entities.Book
            {
                Code = existingBookDTO.Code,
                Title = existingBookDTO.Title,
                Author = existingBookDTO.Author,
                Year = existingBookDTO.Year,
                Publisher = existingBookDTO.Publisher,
                Genre = existingBookDTO.Genre
            };
            DbContext.Books.Add(existingBook);
            DbContext.SaveChanges();

            // Act
            var result = await controller.CreateBook(existingBookDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
