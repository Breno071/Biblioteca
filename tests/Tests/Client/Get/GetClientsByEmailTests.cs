using API.Controllers;
using AutoMapper;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Client.Get
{
    public class GetClientsByEmailTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidEmail_WhenGettingClients_ThenReturnsOkResultWithClientDTOs()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);
            var email = "test@example.com";

            var clients = new List<Domain.Models.Entities.Client>
            {
                new() { Code = Guid.NewGuid(), Name = "Client 1", Email = email },
                new() { Code = Guid.NewGuid(), Name = "Client 2", Email = email }
            };

            DbContext.Clients.AddRange(clients);
            DbContext.SaveChanges();

            var clientDTOs = clients.Select(client => new ClientDTO { Code = client.Code, Name = client.Name, Email = client.Email }).ToList();
            mapperMock.Setup(x => x.Map<List<ClientDTO>>(It.IsAny<List<Domain.Models.Entities.Client>>())).Returns(clientDTOs);

            // Act
            var result = await controller.GetClientsByEmail(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClientDTOs = Assert.IsType<List<ClientDTO>>(okResult.Value);

            Assert.Equal(clientDTOs.Count, returnedClientDTOs.Count);
            for (int i = 0; i < clientDTOs.Count; i++)
            {
                Assert.Equal(clientDTOs[i].Code, returnedClientDTOs[i].Code);
                Assert.Equal(clientDTOs[i].Name, returnedClientDTOs[i].Name);
                Assert.Equal(clientDTOs[i].Email, returnedClientDTOs[i].Email);
            }
        }

        [Fact]
        public async Task GivenNullOrEmptyEmail_WhenGettingClients_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);
            var emptyEmail = string.Empty;

            // Act
            var result = await controller.GetClientsByEmail(emptyEmail);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

            // Act (with null email)
            result = await controller.GetClientsByEmail(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonexistentEmail_WhenGettingClients_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);
            var nonexistentEmail = "nonexistent@example.com";

            // Act
            var result = await controller.GetClientsByEmail(nonexistentEmail);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClientDTOs = Assert.IsType<List<ClientDTO>>(okResult.Value);

            Assert.Empty(returnedClientDTOs);
        }
    }
}
