using API.Controllers;
using AutoMapper;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Book.Get
{
    public class GetBookByIdTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidParameters_WhenGettingBooks_ThenReturnsOkResultWithBookDTOs()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);
            var skip = 0;
            var take = 5;

            var books = new List<Domain.Models.Entities.Book>
            {
                new() { Code = Guid.NewGuid(), Title = "Book 1" },
                new Domain.Models.Entities.Book{ Code = Guid.NewGuid(), Title = "Book 2" },
                new Domain.Models.Entities.Book{ Code = Guid.NewGuid(), Title = "Book 3" },
                new Domain.Models.Entities.Book{ Code = Guid.NewGuid(), Title = "Book 4" },
                new Domain.Models.Entities.Book{ Code = Guid.NewGuid(), Title = "Book 5" }
            };

            DbContext.Books.AddRange(books);
            DbContext.SaveChanges();

            var bookDTOs = new List<BookDTO>
            {
                new BookDTO { Code = books[0].Code, Title = "Book 1" },
                new BookDTO { Code = books[1].Code, Title = "Book 2" },
                new BookDTO { Code = books[2].Code, Title = "Book 3" },
                new BookDTO { Code = books[3].Code, Title = "Book 4" },
                new BookDTO { Code = books[4].Code, Title = "Book 5" }
            };

            mapperMock.Setup(x => x.Map<List<BookDTO>>(It.IsAny<List<Domain.Models.Entities.Book>>())).Returns(bookDTOs);

            // Act
            var result = await controller.GetBooks(skip, take);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBookDTOs = Assert.IsType<List<BookDTO>>(okResult.Value);

            Assert.Equal(bookDTOs.Count, returnedBookDTOs.Count);
            for (int i = 0; i < bookDTOs.Count; i++)
            {
                Assert.Equal(bookDTOs[i].Code, returnedBookDTOs[i].Code);
                Assert.Equal(bookDTOs[i].Title, returnedBookDTOs[i].Title);
            }
        }

        [Theory]
        [InlineData(-1, 5)]
        [InlineData(0, -5)]
        [InlineData(0, 1001)]
        public async Task GivenInvalidParameters_WhenGettingBooks_ThenReturnsBadRequest(int skip, int take)
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new BookController(DbContext, mapperMock.Object);

            // Act
            var result = await controller.GetBooks(skip, take);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
