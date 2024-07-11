using API.Features.Book.DTOs;
using AutoFixture;
using Domain.Enums;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Tests.SharedUtils;

namespace Tests.Book.Get
{
    public class GetBookByIdTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/book/";
        private readonly Fixture _autoFixture = new Fixture();

        [Fact]
        public async Task GivenValidBookId_WhenGettingBook_ThenReturnsOkResultWithBook()
        {
            // Arrange
            var book = (await _autoFixture.AddBooksOnDb(DbContext, 1)).Single();

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, book.BookId));
            var res = await rsp.Content.ReadFromJsonAsync<BookDetailsDto>();

            // Assert
            res.Should().NotBeNull();
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());

            res!.Title.Should().Be(book.Title);
            res!.Author.Should().Be(book.Author);
            res!.Publisher.Should().Be(book.Publisher);
            res!.Genre.Should().Be(book.Genre);
        }

        [Fact]
        public async Task GivenNonExistentBook_WhenGettingBook_ThenReturnsNotFoundResult()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, bookId));

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.NotFound, await rsp.Content.ReadAsStringAsync());
        }
    }
}
