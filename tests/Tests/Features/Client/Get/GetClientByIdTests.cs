using API.Features.Book.DTOs;
using API.Features.Client.DTOs;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using Tests.SharedUtils;

namespace Tests.Features.Client.Get
{
    public class GetClientByIdTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/client/";
        private readonly Fixture _autoFixture = new Fixture();

        [Fact]
        public async Task GivenValidId_WhenGettingClient_ThenReturnsOkResultWithClientDTO()
        {
            // Arrange
            var client = (await _autoFixture.AddClientsToDb(DbContext, 1)).Single();

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, client.ClientId));
            var res = await rsp.Content.ReadFromJsonAsync<ClientDetailsDto>();

            // Assert
            res.Should().NotBeNull();
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());

            res!.Email.Should().Be(client.Email);
            res!.Name.Should().Be(client.Name);
            res!.ClientId.Should().Be(client.ClientId);
        }

        [Fact]
        public async Task GivenNonExistentId_WhenGettingClient_ThenReturnsNotFound()
        {
            // Arrange
            var clientId = Guid.NewGuid();

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, clientId));

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.NotFound, await rsp.Content.ReadAsStringAsync());
        }
    }
}
