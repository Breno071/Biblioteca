using API.Controllers;
using AutoMapper;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Client.Get
{
    public class GetClientsTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidSkipAndTake_WhenGettingClients_ThenReturnsOkResultWithClientDTOs()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);
            var skip = 0;
            var take = 5;

            var clients = new List<Domain.Models.Entities.Client>
            {
                new() { Code = Guid.NewGuid(), Name = "Client 1" },
                new() { Code = Guid.NewGuid(), Name = "Client 2" },
                new() { Code = Guid.NewGuid(), Name = "Client 3" }
            };

            DbContext.Clients.AddRange(clients);
            DbContext.SaveChanges();

            var clientDTOs = clients.Select(client => new ClientDTO { Code = client.Code, Name = client.Name }).ToList();
            mapperMock.Setup(x => x.Map<List<ClientDTO>>(It.IsAny<List<Domain.Models.Entities.Client>>())).Returns(clientDTOs);

            // Act
            var result = await controller.GetClients(skip, take);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClientDTOs = Assert.IsType<List<ClientDTO>>(okResult.Value);

            Assert.Equal(clientDTOs.Count, returnedClientDTOs.Count);
            for (int i = 0; i < clientDTOs.Count; i++)
            {
                Assert.Equal(clientDTOs[i].Code, returnedClientDTOs[i].Code);
                Assert.Equal(clientDTOs[i].Name, returnedClientDTOs[i].Name);
            }
        }

        [Fact]
        public async Task GivenNegativeSkipOrTake_WhenGettingClients_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);
            var skip = -1;
            var take = 5;

            // Act
            var result = await controller.GetClients(skip, take);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenTakeGreaterThanLimit_WhenGettingClients_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);
            var skip = 0;
            var take = 1500;

            // Act
            var result = await controller.GetClients(skip, take);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
