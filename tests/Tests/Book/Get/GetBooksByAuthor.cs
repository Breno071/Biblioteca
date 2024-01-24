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
        //[Fact]
        public async Task GivenValidAuthor_WhenGettingBooksByAuthor_ThenReturnsOkResultWithBookDTOs()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var author = "Irineu";

            var books = new List<BookDTO>
            {
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Title = "Book 1",  
                    Author = "Irineu", 
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = Genre.ScienceFiction 
                },
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Title = "Book 2",  
                    Author = "Irineu", 
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = Genre.ScienceFiction 
                }
            };

            // Act
            foreach (var book in books)
            {
                await controller.CreateBook(book);
            }

            var result = await controller.GetBooksByAuthor(author);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<BookDTO?>>(okResult.Value);

            Assert.NotNull(returnedBooks);
            Assert.Equal(returnedBooks.Count, books.Count);
            Assert.Equal(books.Count, returnedBooks.Count);
        }

        //[Fact]
        public async Task GivenEmptyAuthor_WhenGettingBooksByAuthor_ThenReturnsBadRequest()
        {
            // Arrange
            
            var controller = new BookController(DbContext, _mapper);

            // Act
            var result = await controller.GetBooksByAuthor(string.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        //[Fact]
        public async Task GivenNoMatchingBooks_WhenGettingBooksByAuthor_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var author = "Nonexistent Author jose da silva";

            // Act
            var result = await controller.GetBooksByAuthor(author);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<BookDTO?>>(okResult.Value);

            Assert.Empty(returnedBooks);
        }
    }
}
