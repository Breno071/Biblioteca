using API.Controllers;
using AutoMapper;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Client.Get
{
    public class GetClientByIdTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidId_WhenGettingClient_ThenReturnsOkResultWithClientDTO()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);
            var id = Guid.NewGuid();

            var client = new Domain.Models.Entities.Client { Code = id, Name = "Client 1" };
            DbContext.Clients.Add(client);
            DbContext.SaveChanges();

            var clientDTO = new ClientDTO { Code = client.Code, Name = client.Name };
            mapperMock.Setup(x => x.Map<ClientDTO>(It.IsAny<Domain.Models.Entities.Client>())).Returns(clientDTO);

            // Act
            var result = await controller.GetClient(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClientDTO = Assert.IsType<ClientDTO>(okResult.Value);

            Assert.Equal(clientDTO.Code, returnedClientDTO.Code);
            Assert.Equal(clientDTO.Name, returnedClientDTO.Name);
        }

        [Fact]
        public async Task GivenEmptyId_WhenGettingClient_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);

            // Act
            var result = await controller.GetClient(Guid.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentId_WhenGettingClient_ThenReturnsNotFound()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);
            var id = Guid.NewGuid();

            // Act
            var result = await controller.GetClient(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
