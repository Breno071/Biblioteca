using API.Controllers;
using AutoMapper;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Book.Update
{
    public class UpdateBookTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidIdAndBookDTO_WhenUpdatingBook_ThenReturnsOkResultWithUpdatedBook()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var id = Guid.NewGuid();

            var existingBook = new Domain.Models.Entities.Book { Code = id, Title = "Existing Book", Author = "Author", Year = 2022 };
            DbContext.Books.Add(existingBook);
            DbContext.SaveChanges();

            var updatedBookDTO = new BookDTO { Code = id, Title = "Updated Book", Author = "New Author", Year = 2023 };
            mapperMock.Setup(x => x.Map<Domain.Models.Entities.Book>(It.IsAny<BookDTO>())).Returns(new Domain.Models.Entities.Book
            {
                Code = updatedBookDTO.Code,
                Title = updatedBookDTO.Title,
                Author = updatedBookDTO.Author,
                Year = updatedBookDTO.Year
            });

            // Act
            var result = await controller.UpdateBook(id, updatedBookDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBook = Assert.IsType<Domain.Models.Entities.Book>(okResult.Value);

            Assert.Equal(updatedBookDTO.Code, returnedBook.Code);
            Assert.Equal(updatedBookDTO.Title, returnedBook.Title);
            Assert.Equal(updatedBookDTO.Author, returnedBook.Author);
            Assert.Equal(updatedBookDTO.Year, returnedBook.Year);

            // Check if the book was actually updated in the database
            var updatedBookInDb = DbContext.Books.FirstOrDefault(x => x.Code == id);
            Assert.NotNull(updatedBookInDb);
            Assert.Equal(updatedBookDTO.Title, updatedBookInDb.Title);
            Assert.Equal(updatedBookDTO.Author, updatedBookInDb.Author);
            Assert.Equal(updatedBookDTO.Year, updatedBookInDb.Year);
        }

        [Fact]
        public async Task GivenInvalidId_WhenUpdatingBook_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var id = Guid.NewGuid();

            var updatedBookDTO = new BookDTO { Code = Guid.NewGuid(), Title = "Updated Book", Author = "New Author", Year = 2023 };

            // Act
            var result = await controller.UpdateBook(id, updatedBookDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNullBookDTO_WhenUpdatingBook_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var id = Guid.NewGuid();

            // Act
            var result = await controller.UpdateBook(id, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonexistentId_WhenUpdatingBook_ThenReturnsNotFound()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var id = Guid.NewGuid();

            var updatedBookDTO = new BookDTO { Code = id, Title = "Updated Book", Author = "New Author", Year = 2023 };

            // Act
            var result = await controller.UpdateBook(id, updatedBookDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
