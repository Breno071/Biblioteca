using API.Controllers;
using AutoMapper;
using Domain.Enums;
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
            var controller = new BookController(DbContext, _mapper);
            var id = Guid.NewGuid();

            var existingBook = new BookDTO
            {
                Code = id,
                Title = "Existing Book",
                Author = "Author",
                Year = 2022,
                Genre = Genre.NonFiction,
                Publisher = "Publisher"
            };

            var updatedBookDTO = new BookDTO
            {
                Code = id,
                Title = "Updated Book",
                Author = "New Author",
                Year = 2023,
                Publisher = "Publisher",
                Genre = Genre.Mystery
            };

            // Act
            await controller.CreateBook(existingBook);
            DbContext.ChangeTracker.Clear();
            var result = await controller.UpdateBook(id, updatedBookDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBook = Assert.IsType<BookDTO>(okResult.Value);

            Assert.Equal(updatedBookDTO.Code, returnedBook.Code);
            Assert.Equal(updatedBookDTO.Title, returnedBook.Title);
            Assert.Equal(updatedBookDTO.Author, returnedBook.Author);
            Assert.Equal(updatedBookDTO.Year, returnedBook.Year);

            // Check if the book was actually updated in the database
            DbContext.ChangeTracker.Clear();
            var updatedBookInDb = DbContext.Books.FirstOrDefault(x => x.BookId == id);
            Assert.NotNull(updatedBookInDb);
            Assert.Equal(updatedBookDTO.Title, updatedBookInDb.Title);
            Assert.Equal(updatedBookDTO.Author, updatedBookInDb.Author);
            Assert.Equal(updatedBookDTO.Year, updatedBookInDb.Year);
        }

        [Fact]
        public async Task GivenInvalidId_WhenUpdatingBook_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var id = Guid.NewGuid();

            var updatedBookDTO = new BookDTO 
            { 
                Code = Guid.NewGuid(), 
                Title = "Updated Book", 
                Author = "New Author", 
                Year = 2023 
            };

            // Act
            var result = await controller.UpdateBook(id, updatedBookDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentId_WhenUpdatingBook_ThenReturnsNotFound()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var id = Guid.NewGuid();

            var updatedBookDTO = new BookDTO 
            { 
                Code = id, 
                Title = "Updated Book", 
                Author = "New Author", 
                Year = 2023 
            };

            // Act
            var result = await controller.UpdateBook(id, updatedBookDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
