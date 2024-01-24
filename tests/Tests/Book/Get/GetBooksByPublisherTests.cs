using API.Controllers;
using AutoMapper;
using Domain.Enums;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Book.Get
{
    public class GetBooksByPublisherTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidPublisher_WhenGettingBooks_ThenReturnsOkResultWithBookDTOs()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var publisher = "Publisher";
            var genre = Genre.Fiction;

            var books = new List<BookDTO>
            {
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Title = "Book 1",  
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = genre 
                },
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Title = "Book 2",  
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = genre 
                },
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Title = "Book 3",  
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Year = 123, 
                    Genre = genre 
                }
            };


            // Act
            foreach (var book in books)
            {
                await controller.CreateBook(book);
            }

            var bookDTOs = books.Select(book => new BookDTO
            {
                Code = book.Code,
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                Year = book.Year,
                Genre = book.Genre
            }).ToList();
            var result = await controller.GetBooksByPublisher(publisher);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Equal(bookDTOs.Count, returnedBooks.Count);
        }

        [Fact]
        public async Task GivenEmptyPublisher_WhenGettingBooks_ThenReturnsBadRequest()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);

            // Act
            var result = await controller.GetBooksByPublisher(string.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNoMatchingBooks_WhenGettingBooks_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var publisher = "NonExistent Publisher";

            // Act
            var result = await controller.GetBooksByPublisher(publisher);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Empty(returnedBooks);
        }
    }
}
