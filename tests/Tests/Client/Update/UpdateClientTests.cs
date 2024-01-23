using API.Controllers;
using AutoMapper;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Client.Update
{
    public class UpdateClientTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GivenValidIdAndMatchingDTO_WhenUpdatingClient_ThenReturnsOkResultWithUpdatedClient()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);
            var id = Guid.NewGuid();

            var existingClient = new Domain.Models.Entities.Client { Code = id, Name = "Client 1", Email = "client@example.com" };
            DbContext.Clients.Add(existingClient);
            DbContext.SaveChanges();

            var updatedClientDTO = new ClientDTO { Code = id, Name = "Updated Client", Email = "updated@example.com" };
            mapperMock.Setup(x => x.Map<Domain.Models.Entities.Client>(It.IsAny<ClientDTO>())).Returns(new Domain.Models.Entities.Client
            {
                Code = updatedClientDTO.Code,
                Name = updatedClientDTO.Name,
                Email = updatedClientDTO.Email
            });

            // Act
            var result = await controller.UpdateClient(id, updatedClientDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUpdatedClient = Assert.IsType<Domain.Models.Entities.Client>(okResult.Value);

            Assert.Equal(updatedClientDTO.Code, returnedUpdatedClient.Code);
            Assert.Equal(updatedClientDTO.Name, returnedUpdatedClient.Name);
            Assert.Equal(updatedClientDTO.Email, returnedUpdatedClient.Email);

            // Check if the client was actually updated in the database
            var updatedClientInDb = DbContext.Clients.FirstOrDefault(x => x.Code == id);
            Assert.NotNull(updatedClientInDb);
            Assert.Equal(updatedClientDTO.Name, updatedClientInDb.Name);
            Assert.Equal(updatedClientDTO.Email, updatedClientInDb.Email);
        }

        [Fact]
        public async Task GivenMismatchedIdAndDTO_WhenUpdatingClient_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);
            var id = Guid.NewGuid();
            var mismatchedId = Guid.NewGuid();

            var existingClient = new Domain.Models.Entities.Client { Code = id, Name = "Client 1", Email = "client@example.com" };
            DbContext.Clients.Add(existingClient);
            DbContext.SaveChanges();

            var updatedClientDTO = new ClientDTO { Code = mismatchedId, Name = "Updated Client", Email = "updated@example.com" };

            // Act
            var result = await controller.UpdateClient(id, updatedClientDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenEmptyId_WhenUpdatingClient_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);

            // Act
            var result = await controller.UpdateClient(Guid.Empty, new ClientDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenClientWithExistingEmail_WhenUpdatingClient_ThenReturnsNotFound()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);
            var id = Guid.NewGuid();

            var existingClient1 = new Domain.Models.Entities.Client { Code = Guid.NewGuid(), Name = "Client 1", Email = "client1@example.com" };
            var existingClient2 = new Domain.Models.Entities.Client { Code = id, Name = "Client 2", Email = "client2@example.com" };
            DbContext.Clients.AddRange(existingClient1, existingClient2);
            DbContext.SaveChanges();

            var updatedClientDTO = new ClientDTO { Code = id, Name = "Updated Client", Email = "client1@example.com" };

            // Act
            var result = await controller.UpdateClient(id, updatedClientDTO);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
