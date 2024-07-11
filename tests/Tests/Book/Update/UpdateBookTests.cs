using API.Features.Book.DTOs;
using API.Features.Book.Endpoints.UpdateBook;
using AutoFixture;
using Domain.Enums;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Tests.SharedUtils;

namespace Tests.Book.Update
{
    public class UpdateBookTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/book/";
        private readonly Fixture _autoFixture = new Fixture();

        [Fact]
        public async Task GivenValidIdAndBookDTO_WhenUpdatingBook_ThenReturnsOkResultWithUpdatedBook()
        {
            // Arrange
            var book = (await _autoFixture.AddBooksOnDb(DbContext, 1)).Single();

            UpdateBookRequest request = new UpdateBookRequest
            {
                Author = "JR Token",
                Title = "O senhor dos aneis",
                Genre = Genre.Adventure,
                Publisher = "Token",
                Stock = 1,
                Year = 2000
            };

            // Act            
            var rsp = await AnonymousUser.PutAsJsonAsync(string.Concat(Path, book.BookId), request);

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());

            var res = await rsp.Content.ReadFromJsonAsync<BookDetailsDto>();
            res.Should().NotBeNull();

            res!.Title.Should().Be(request.Title);
            res!.Author.Should().Be(request.Author);
            res!.Publisher.Should().Be(request.Publisher);
            res!.Genre.Should().Be(request.Genre);
            res!.Year.Should().Be(request.Year);
        }

        [Fact]
        public async Task GivenNonExistentId_WhenUpdatingBook_ThenReturnsNotFound()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            // Act
            var rsp = await AnonymousUser.PutAsJsonAsync(string.Concat(Path, bookId), new UpdateBookRequest());
            var res = await rsp.Content.ReadFromJsonAsync<List<BookDetailsDto>>();

            // Assert
            res.Should().NotBeNull();
            rsp.StatusCode.Should().Be(HttpStatusCode.NotFound, await rsp.Content.ReadAsStringAsync());

            res.Should().BeEmpty();
        }
    }
}
