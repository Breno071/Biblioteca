using API.Features.Book.DTOs;
using API.Features.Book.Endpoints.UpdateBook;
using AutoFixture;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net;
using Tests.SharedUtils;
using API.Features.Client.Endpoints.UpdateClient;
using FluentAssertions;
using API.Features.Client.DTOs;

namespace Tests.Client.Update
{
    public class UpdateClientTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/client/";
        private readonly Fixture _autoFixture = new Fixture();

        [Fact]
        public async Task GivenValidIdAndMatchingDTO_WhenUpdatingClient_ThenReturnsOkResultWithUpdatedClient()
        {
            // Arrange
            var client = (await _autoFixture.AddClientsToDb(DbContext, 1)).Single();

            UpdateClientRequest request = new UpdateClientRequest
            {
                Name = "JR Token",
                Email = "token@gmail.com",
            };

            // Act            
            var rsp = await AnonymousUser.PutAsJsonAsync(string.Concat(Path, client.ClientId), request);

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());

            var res = await rsp.Content.ReadFromJsonAsync<ClientDetailsDto>();
            res.Should().NotBeNull();

            res!.Email.Should().Be(request.Email);
            res!.Name.Should().Be(request.Name);
            res!.ClientId.Should().Be(client.ClientId);
        }

        [Fact]
        public async Task GivenNonExistentId_WhenUpdatingClient_ThenReturnsNotFound()
        {
            // Arrange
            var clientId = Guid.NewGuid();

            // Act
            var rsp = await AnonymousUser.PutAsJsonAsync(string.Concat(Path, clientId), new UpdateBookRequest());

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.NotFound, await rsp.Content.ReadAsStringAsync());
        }
    }
}
