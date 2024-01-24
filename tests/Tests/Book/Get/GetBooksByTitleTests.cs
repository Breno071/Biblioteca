using API.Controllers;
using AutoMapper;
using Domain.Enums;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Book.Get
{
    public class GetBooksByTitleTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidTitle_WhenGettingBooks_ThenReturnsOkResultWithBookDTOs()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var title = "Test Title";
            var genre = Genre.Fiction;

            var books = new List<Domain.Models.Entities.Book>
            {
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Title = "Test Title",  
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = genre 
                },
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Title = "Test Title",  
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = genre 
                },
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Title = "Test Title",  
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = genre 
                }
            };


            // Act
            DbContext.Books.AddRange(books);
            DbContext.SaveChanges();

            var bookDTOs = books.Select(book => new BookDTO
            {
                Code = book.Code,
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                Year = book.Year,
                Genre = book.Genre
            }).ToList();
            var result = await controller.GetBooksByTitle(title);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Equal(bookDTOs.Count, returnedBooks.Count);
        }

        [Fact]
        public async Task GivenEmptyTitle_WhenGettingBooks_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);

            // Act
            var result = await controller.GetBooksByTitle(string.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNoMatchingBooks_WhenGettingBooks_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var title = "Any Title";

            // Act
            var result = await controller.GetBooksByTitle(title);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Empty(returnedBooks);
        }
    }
}
