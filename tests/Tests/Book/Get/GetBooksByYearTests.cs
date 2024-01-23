using API.Controllers;
using AutoMapper;
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
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var year = 2022;

            var books = new List<Domain.Models.Entities.Book>
            {
                new() { Code = Guid.NewGuid(), Title = "Book 1", Year = year },
                new() { Code = Guid.NewGuid(), Title = "Book 2", Year = year },
                new() { Code = Guid.NewGuid(), Title = "Book 3", Year = year }
            };

            DbContext.Books.AddRange(books);
            DbContext.SaveChanges();

            var bookDTOs = books.Select(book => new BookDTO { Code = book.Code, Title = book.Title, Year = book.Year }).ToList();
            mapperMock.Setup(x => x.Map<List<BookDTO>>(It.IsAny<List<Domain.Models.Entities.Book>>())).Returns(bookDTOs);

            // Act
            var result = await controller.GetBooksByYear(year);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBookDTOs = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Equal(bookDTOs.Count, returnedBookDTOs.Count);
            for (int i = 0; i < bookDTOs.Count; i++)
            {
                Assert.Equal(bookDTOs[i].Code, returnedBookDTOs[i].Code);
                Assert.Equal(bookDTOs[i].Title, returnedBookDTOs[i].Title);
                Assert.Equal(bookDTOs[i].Year, returnedBookDTOs[i].Year);
            }
        }

        [Fact]
        public async Task GivenNoMatchingBooks_WhenGettingBooks_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var year = 2022;

            // Act
            var result = await controller.GetBooksByYear(year);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBookDTOs = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Empty(returnedBookDTOs);
        }
    }
}
