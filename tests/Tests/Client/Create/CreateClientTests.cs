using API.Controllers;
using AutoMapper;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Client.Create
{
    public class CreateClientTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task CreateClient_GivenNonexistentClientDTO_WhenCreatingClient_ThenReturnsOkResultWithCreatedClient()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);

            var clientDTO = new ClientDTO { Code = Guid.NewGuid(), Name = "New Client", Email = "newclient@example.com" };
            mapperMock.Setup(x => x.Map<Domain.Models.Entities.Client>(It.IsAny<ClientDTO>())).Returns(new Domain.Models.Entities.Client
            {
                Code = clientDTO.Code,
                Name = clientDTO.Name,
                Email = clientDTO.Email
            });

            // Act
            var result = await controller.CreateClient(clientDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var createdClient = Assert.IsType<Domain.Models.Entities.Client>(okResult.Value);

            Assert.Equal(clientDTO.Code, createdClient.Code);
            Assert.Equal(clientDTO.Name, createdClient.Name);
            Assert.Equal(clientDTO.Email, createdClient.Email);

            // Check if the client was actually created in the database
            var createdClientInDb = DbContext.Clients.FirstOrDefault(x => x.Code == clientDTO.Code);
            Assert.NotNull(createdClientInDb);
            Assert.Equal(clientDTO.Name, createdClientInDb.Name);
            Assert.Equal(clientDTO.Email, createdClientInDb.Email);
        }

        [Fact]
        public async Task CreateClient_GivenExistingClientDTO_WhenCreatingClient_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);

            var existingClientDTO = new ClientDTO { Code = Guid.NewGuid(), Name = "Existing Client", Email = "existingclient@example.com" };
            var existingClient = new Domain.Models.Entities.Client
            {
                Code = existingClientDTO.Code,
                Name = existingClientDTO.Name,
                Email = existingClientDTO.Email
            };

            DbContext.Clients.Add(existingClient);
            DbContext.SaveChanges();

            // Act
            var result = await controller.CreateClient(existingClientDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateClient_GivenClientDTOWithExistingEmail_WhenCreatingClient_ThenReturnsBadRequest()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var controller = new ClientController(DbContext, mapperMock.Object);

            var existingClientEmail = "existingclient@example.com";
            var existingClient = new Domain.Models.Entities.Client { Code = Guid.NewGuid(), Name = "Existing Client", Email = existingClientEmail };
            DbContext.Clients.Add(existingClient);
            DbContext.SaveChanges();

            var newClientDTO = new ClientDTO { Code = Guid.NewGuid(), Name = "New Client", Email = existingClientEmail };

            // Act
            var result = await controller.CreateClient(newClientDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
