using Domain.Enums;
using FluentAssertions;
using System.Net;

namespace Tests.Book.Delete
{
    public class DeleteBookTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/book/";

        [Fact]
        public async Task GivenExistingId_WhenDeletingBook_ThenReturnsNoContent()
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
            var rsp = await AnonymousUser.DeleteAsync(string.Concat(Path, book.BookId));

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.NoContent, await rsp.Content.ReadAsStringAsync());

            DbContext.Books.Single(x => x.BookId == book.BookId && !x.Active).Should().NotBeNull();
        }

        [Fact]
        public async Task GivenNonExistentId_WhenDeletingBook_ThenReturnsNotFound()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            // Act
            var rsp = await AnonymousUser.DeleteAsync(string.Concat(Path, bookId));

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.NotFound, await rsp.Content.ReadAsStringAsync());
        }
    }
}
