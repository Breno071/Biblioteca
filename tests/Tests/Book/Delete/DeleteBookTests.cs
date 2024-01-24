using API.Controllers;
using AutoMapper;
using Domain.Enums;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Book.Delete
{
    public class DeleteBookTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenExistingId_WhenDeletingBook_ThenReturnsNoContent()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var id = Guid.NewGuid();

            var existingBook = new BookDTO
            { 
                Code = id, 
                Title = "Existing Book", 
                Author = "Author", 
                Year = 2022, 
                Publisher = "Publisher", 
                Genre = Genre.Fantasy 
            };

            // Act
            await controller.CreateBook(existingBook);
            var result = await controller.DeleteBook(id);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Check if the book was actually deleted from the database
            var deletedBookInDb = DbContext.Books.FirstOrDefault(x => x.Code == id);
            Assert.NotNull(deletedBookInDb);
            Assert.False(deletedBookInDb?.Active);
        }

        [Fact]
        public async Task GivenNonExistentId_WhenDeletingBook_ThenReturnsNotFound()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var id = Guid.NewGuid();

            // Act
            var result = await controller.DeleteBook(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GivenEmptyId_WhenDeletingBook_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new BookController(DbContext, Mock.Of<IMapper>());

            // Act
            var result = await controller.DeleteBook(Guid.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
