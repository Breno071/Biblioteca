using API.Features.Book.Endpoints.CreateBook;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net;
using API.Features.Book.DTOs;

namespace Tests.Book.Get
{
    public class GetBooksByAuthor(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/books/author/";

        [Fact]
        public async Task GivenValidAuthor_WhenGettingBooksByAuthor_ThenReturnsOkResultWithBooks()
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
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, book.Author));
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
        public async Task GivenNoMatchingBooks_WhenGettingBooksByAuthor_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var author = "asd123";

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, author));
            var res = await rsp.Content.ReadFromJsonAsync<List<BookDetailsDto>>();

            // Assert
            res.Should().NotBeNull();
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());

            res.Should().BeEmpty();
        }
    }
}
