using API.Features.Book.Endpoints.CreateBook;
using Domain.Enums;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Tests.Book.Create
{
    public class CreateBookTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/book";

        [Fact]
        public async Task GivenNewBook_WhenCreatingBook_ThenReturnsOkResultWithCreatedBook()
        {
            // Arrange
            var req = new CreateBookRequest 
            { 
                Title = "New Book", 
                Author = "Author", 
                Publisher = "Publisher", 
                Genre = Genre.Comedy, 
                Year = 2022 
            };

            // Act
            var rsp = await AnonymousUser.PostAsJsonAsync(Path, req);

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.Created, await rsp.Content.ReadAsStringAsync());
            var res = await rsp.Content.ReadFromJsonAsync<CreateBookResponse>();

            res.Should().NotBeNull();

            // Check if the book was actually created in the database
            var createdBookInDb = DbContext.Books.Single(x => x.BookId == res!.BookId);

            createdBookInDb.Should().NotBeNull();

            res!.Title.Should().Be(createdBookInDb.Title);
            res!.Author.Should().Be(createdBookInDb.Author);
            res!.Publisher.Should().Be(createdBookInDb.Publisher);
            res!.Genre.Should().Be(createdBookInDb.Genre);
            res!.Genre.Should().Be(createdBookInDb.Genre);

            createdBookInDb.Stock.Should().Be(0);
        }
    }
}
