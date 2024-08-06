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
    public class GetClientsByEmailTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/client/email/";
        private readonly Fixture _autoFixture = new Fixture();

        [Fact]
        public async Task GivenValidEmail_WhenGettingClients_ThenReturnsOkResultWithClients()
        {
            // Arrange
            var client = (await _autoFixture.AddClientsToDb(DbContext, 1)).Single();

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, client.Email));
            var res = await rsp.Content.ReadFromJsonAsync<ClientDetailsDto>();

            // Assert
            res.Should().NotBeNull();
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());

            res!.Email.Should().Be(client.Email);
            res!.Name.Should().Be(client.Name);
            res!.ClientId.Should().Be(client.ClientId);
        }

        [Fact]
        public async Task GivenNonExistentEmail_WhenGettingClients_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var email = Guid.NewGuid();

            // Act
            var rsp = await AnonymousUser.GetAsync(string.Concat(Path, email));

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.NotFound, await rsp.Content.ReadAsStringAsync());
        }
    }
}
