using API.Features.Book.DTOs;
using AutoFixture;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Tests.SharedUtils;

namespace Tests.Book.Get
{
    public class GetBookPaginatedTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/books/";
        private readonly Fixture _autoFixture = new Fixture();

        [Fact]
        public async Task GivenValidParameters_WhenGettingBooks_ThenReturnsOkResultWithBooks()
        {
            // Arrange
            var books = (await _autoFixture.AddBooksOnDb(DbContext, 6));

            int pageSize = 3;            

            for (int page = 1; page < 3; page++)
            {
                // Act
                var rsp = await AnonymousUser.GetAsync($"{Path}?Page={page}&PageSize={pageSize}");
                var res = await rsp.Content.ReadFromJsonAsync<List<BookDetailsDto>>();

                // Assert
                res.Should().NotBeNull();
                rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());

                res.Count.Should().Be(3);

                foreach (var book in res)
                {
                    books.Find(x => x.BookId == book.BookId).Should().NotBeNull();
                    books.Find(x => x.Title == book.Title).Should().NotBeNull();
                    books.Find(x => x.Author == book.Author).Should().NotBeNull();
                    books.Find(x => x.Publisher == book.Publisher).Should().NotBeNull();
                    books.Find(x => x.Genre == book.Genre).Should().NotBeNull();
                    books.Find(x => x.Year == book.Year).Should().NotBeNull();
                }
            }
        }
    }
}
