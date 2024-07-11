using API.Features.Book.DTOs;
using AutoFixture;
using Domain.Enums;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Tests.SharedUtils;

namespace Tests.Book.Get
{
    public class GetBooksByPublisherTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/books/publisher/";
        private readonly Fixture _autoFixture = new Fixture();

        [Fact]
        public async Task GivenValidPublisher_WhenGettingBooks_ThenReturnsOkResultWithBooks()
        {
            // Arrange
            var book = (await _autoFixture.AddBooksOnDb(DbContext, 1)).Single();

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

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());
        }
    }
}
