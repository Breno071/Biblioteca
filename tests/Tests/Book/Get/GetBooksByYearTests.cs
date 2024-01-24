using API.Controllers;
using AutoMapper;
using Domain.Enums;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Book.Get
{
    public class GetBooksByYearTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidYear_WhenGettingBooks_ThenReturnsOkResultWithBookDTOs()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var year = 2022;

            var books = new List<Domain.Models.Entities.Book>
            {
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Title = "Book 1", 
                    Year = year, 
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Genre = 
                    Genre.Adventure 
                },
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Title = "Book 2", 
                    Year = year, 
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Genre = Genre.Adventure 
                },
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Title = "Book 3", 
                    Year = year, 
                    Author = "Author", 
                    Publisher = "Publisher", 
                    Genre = Genre.Adventure 
                }
            };


            // Act
            DbContext.Books.AddRange(books);
            DbContext.SaveChanges();

            var bookDTOs = books.Select(book => new BookDTO 
            { 
                Code = book.Code, 
                Title = book.Title, 
                Year = book.Year,
                Author = book.Author,
                Publisher = book.Publisher,
                Genre = book.Genre
            }).ToList();
            var result = await controller.GetBooksByYear(year);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Equal(bookDTOs.Count, returnedBooks.Count);
        }

        [Fact]
        public async Task GivenNoMatchingBooks_WhenGettingBooks_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var controller = new BookController(DbContext, _mapper);
            var year = 1;

            // Act
            var result = await controller.GetBooksByYear(year);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Empty(returnedBooks);
        }
    }
}
