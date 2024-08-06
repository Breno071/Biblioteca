using API.Features.Client.Endpoints.CreateClient;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Tests.Features.Client.Create
{
    public class CreateClientTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/client";

        [Fact]
        public async Task CreateClient_GivenNonExistentClientDTO_WhenCreatingClient_ThenReturnsOkResultWithCreatedClient()
        {
            // Arrange
            var req = new CreateClientRequest
            {
                Name = "Irineu",
                Email = "emailqualquer@gmail.com"
            };

            // Act
            var rsp = await AnonymousUser.PostAsJsonAsync(Path, req);

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.Created, await rsp.Content.ReadAsStringAsync());
            var res = await rsp.Content.ReadFromJsonAsync<CreateClientResponse>();

            res.Should().NotBeNull();

            var createdClientInDb = DbContext.Clients.Single(x => x.ClientId == res!.ClientId);

            createdClientInDb.Should().NotBeNull();

            res!.Email.Should().Be(createdClientInDb.Email);
            res!.Name.Should().Be(createdClientInDb.Name);
        }
    }
}
