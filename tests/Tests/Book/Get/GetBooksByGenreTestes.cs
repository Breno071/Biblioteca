using API.Controllers;
using AutoMapper;
using Domain.Enums;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Book.Get
{
    public class GetBooksByGenreTestes(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        //[Fact]
        public async Task GivenValidGenre_WhenGettingBooks_ThenReturnsOkResultWithBookDTOs()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var genre = Genre.Fiction;

            var books = new List<Domain.Models.Entities.Book>
            {
                new() { Code = Guid.NewGuid(), Title = "Book 1",  Author = "Author", Publisher = "Publisher", Year = 123, Genre = genre },
                new() { Code = Guid.NewGuid(), Title = "Book 2",  Author = "Author", Publisher = "Publisher", Year = 123, Genre = genre },
                new() { Code = Guid.NewGuid(), Title = "Book 3",  Author = "Author", Publisher = "Publisher", Year = 123, Genre = genre }
            };

            DbContext.Books.AddRange(books);
            DbContext.SaveChanges();

            var bookDTOs = books.Select(book => new BookDTO 
            { 
                Code = book.Code, 
                Title = book.Title, 
                Author =book.Author,
                Publisher = book.Publisher, 
                Year = book.Year, 
                Genre = book.Genre 
            }).ToList();
            mapperMock.Setup(x => x.Map<List<BookDTO>>(It.IsAny<List<Domain.Models.Entities.Book>>())).Returns(bookDTOs);

            // Act
            var result = await controller.GetBooksByGenre(genre);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<Domain.Models.Entities.Book>>(okResult.Value);

            Assert.Equal(bookDTOs.Count, returnedBooks.Count);
            for (int i = 0; i < bookDTOs.Count; i++)
            {
                Assert.Equal(bookDTOs[i].Code, returnedBooks[i].Code);
                Assert.Equal(bookDTOs[i].Title, returnedBooks[i].Title);
                Assert.Equal(bookDTOs[i].Genre, returnedBooks[i].Genre);
            }
        }

        //[Fact]
        public async Task GivenNoMatchingBooks_WhenGettingBooks_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var genre = Genre.NonFiction;

            // Act
            var result = await controller.GetBooksByGenre(genre);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<Domain.Models.Entities.Book>>(okResult.Value);

            Assert.Empty(returnedBooks);
        }
    }
}
