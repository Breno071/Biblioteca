using API.Features.Book.DTOs;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net;
using AutoFixture;
using Tests.SharedUtils;

namespace Tests.Book.Get
{
    public class GetBooksByTitleTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/books/title/";
        private readonly Fixture _autoFixture = new Fixture();

        [Fact]
        public async Task GivenValidTitle_WhenGettingBooks_ThenReturnsOkResultWithBooks()
        {
            // Arrange
            var book = (await _autoFixture.AddBooksOnDb(DbContext, 1)).Single();

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, book.Title));
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
            var title = "asd123";

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, title));

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());
        }
    }
}
