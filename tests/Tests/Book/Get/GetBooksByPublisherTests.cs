using API.Features.Book.DTOs;
using Domain.Enums;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Tests.Book.Get
{
    public class GetBooksByPublisherTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/books/publisher/";

        [Fact]
        public async Task GivenValidPublisher_WhenGettingBooks_ThenReturnsOkResultWithBooks()
        {
            // Arrange
            var book = new Domain.Models.Entities.Book
            {
                BookId = Guid.NewGuid(),
                Author = "Irineu",
                Genre = Genre.Mystery,
                Active = true,
                Publisher = "Punisher",
                Title = "Titulo",
                Stock = 0,
                Year = 1990
            };

            DbContext.Books.Add(book);
            await DbContext.SaveChangesAsync();

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, book.Publisher));
            var res = await rsp.Content.ReadFromJsonAsync<List<BookDetailsDto>>();

            // Assert
            res.Should().NotBeNull();
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());

            foreach (var responseBook in res)
            {
                responseBook!.Title.Should().Be(book.Title);
                responseBook!.Author.Should().Be(book.Author);
                responseBook!.Publisher.Should().Be(book.Publisher);
                responseBook!.Genre.Should().Be(book.Genre);
            }
        }

        [Fact]
        public async Task GivenNoMatchingBooks_WhenGettingBooks_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var publisher = "asd123";

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, publisher));
            var res = await rsp.Content.ReadFromJsonAsync<List<BookDetailsDto>>();

            // Assert
            res.Should().NotBeNull();
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());

            res.Should().BeEmpty();
        }
    }
}
