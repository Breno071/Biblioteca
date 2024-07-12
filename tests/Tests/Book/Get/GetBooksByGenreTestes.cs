using API.Features.Book.DTOs;
using AutoFixture;
using Domain.Enums;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Tests.SharedUtils;

namespace Tests.Book.Get
{
    public class GetBooksByGenreTestes(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/books/genre/";
        private readonly Fixture _autoFixture = new Fixture();

        [Fact]
        public async Task GivenValidGenre_WhenGettingBooks_ThenReturnsOkResultWithBooks()
        {
            // Arrange
            var book = (await _autoFixture.AddBooksOnDb(DbContext, 1)).Single();

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, book.Genre));
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
            var genre = Genre.Adventure;

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, genre));

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());
        }
    }
}
