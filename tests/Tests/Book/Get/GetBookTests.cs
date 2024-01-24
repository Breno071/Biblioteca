using API.Controllers;
using AutoMapper;
using Domain.Enums;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Book.Get
{
    public class GetBookTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidParameters_WhenGettingBooks_ThenReturnsOkResultWithBookDTOs()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var skip = 0;
            var take = 5;

            var books = new List<BookDTO>
            {
                new()
                {
                    Code = Guid.NewGuid(),
                    Title = "Book 1",
                    Author = "Author",
                    Publisher = "Publisher",
                    Year = 123,
                    Genre = Genre.ScienceFiction
                },
                new()
                {
                    Code = Guid.NewGuid(),
                    Title = "Book 2",
                    Author = "Author",
                    Publisher = "Publisher",
                    Year = 123,
                    Genre = Genre.ScienceFiction
                },
                new()
                {
                    Code = Guid.NewGuid(),
                    Title = "Book 3",
                    Author = "Author",
                    Publisher = "Publisher",
                    Year = 123,
                    Genre = Genre.Science
                },
                new()
                {
                    Code = Guid.NewGuid(),
                    Title = "Book 4",
                    Author = "Author",
                    Publisher = "Publisher",
                    Year = 123,
                    Genre = Genre.Romance
                },
                new()
                {
                    Code = Guid.NewGuid(),
                    Title = "Book 5",
                    Author = "Author",
                    Publisher = "Publisher",
                    Year = 123,
                    Genre = Genre.Mystery
                }
            };

            foreach (var book in books)
            {
                await controller.CreateBook(book);
            }

            var bookDTOs = new List<BookDTO>
            {
                new ()
                { Code = books[0].Code,
                    Title = "Book 1",
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = Genre.ScienceFiction
                },
                new () 
                { 
                    Code = books[1].Code, 
                    Title = "Book 2",  
                    Author = "Author", 
                    Publisher = "Publisher",
                    Year = 123, 
                    Genre = Genre.ScienceFiction
                },
                new () 
                { 
                    Code = books[2].Code, 
                    Title = "Book 3",  
                    Author = "Author",
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = Genre.ScienceFiction
                },
                new () 
                { 
                    Code = books[3].Code, 
                    Title = "Book 4",  
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = Genre.ScienceFiction
                },
                new () 
                { 
                    Code = books[4].Code, 
                    Title = "Book 5",  
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = Genre.ScienceFiction
                }
            };

            // Act
            var result = await controller.GetBooks(skip, take);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<BookDTO?>>(okResult.Value);

            Assert.Equal(bookDTOs.Count, returnedBooks.Count);
        }

        [Theory]
        [InlineData(-1, 5)]
        [InlineData(0, -5)]
        [InlineData(0, 1001)]
        public async Task GivenInvalidParameters_WhenGettingBooks_ThenReturnsBadRequest(int skip, int take)
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);

            // Act
            var result = await controller.GetBooks(skip, take);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
