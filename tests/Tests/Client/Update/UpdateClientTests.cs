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
        //[Fact]
        public async Task GivenValidIdAndMatchingDTO_WhenUpdatingClient_ThenReturnsOkResultWithUpdatedClient()
        {
            // Arrange
            var controller = new ClientController(DbContext, _mapper);
            var id = Guid.NewGuid();

            var existingClient = new ClientDTO
            { 
                Code = id, 
                Name = "Client 1", 
                Email = Guid.NewGuid() + "client1234@example.com" 
            };
            await controller.CreateClient(existingClient);

            var updatedClientDTO = new ClientDTO 
            { 
                Code = id,
                Name = "Updated Client", 
                Email = Guid.NewGuid() + "updated1234@example.com"
            };

            // Act
            DbContext.ChangeTracker.Clear();
            var updatedResult = await controller.UpdateClient(id, updatedClientDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(updatedResult);
            var returnedUpdatedClient = Assert.IsType<ClientDTO>(okResult.Value);

            Assert.Equal(updatedClientDTO.Code, returnedUpdatedClient.Code);
            Assert.Equal(updatedClientDTO.Name, returnedUpdatedClient.Name);
            Assert.Equal(updatedClientDTO.Email, returnedUpdatedClient.Email);

            DbContext.ChangeTracker.Clear();

            // Check if the client was actually updated in the database
            var updatedClientInDb = await controller.GetClientById(id);
            Assert.NotNull(updatedClientInDb);

            var okObjectResult = Assert.IsType<OkObjectResult>(updatedClientInDb);
            var client = Assert.IsType<ClientDTO>(okObjectResult.Value);
            Assert.Equal(updatedClientDTO.Name, client.Name);
            Assert.Equal(updatedClientDTO.Email, client.Email);
        }

        //[Fact]
        public async Task GivenMismatchedIdAndDTO_WhenUpdatingClient_ThenReturnsBadRequest()
        {
            // Arrange
            
            var controller = new ClientController(DbContext, _mapper);
            var id = Guid.NewGuid();
            var mismatchedId = Guid.NewGuid();

            var existingClient = new Domain.Models.Entities.Client 
            { 
                Code = id, 
                Name = "Client 1", 
                Email = "client@example.com" 
            };
            DbContext.Clients.Add(existingClient);
            DbContext.SaveChanges();

            var updatedClientDTO = new ClientDTO
            { 
                Code = mismatchedId,
                Name = "Updated Client", 
                Email = "updated@example.com"
            };

            // Act
            var result = await controller.UpdateClient(id, updatedClientDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        //[Fact]
        public async Task GivenEmptyId_WhenUpdatingClient_ThenReturnsBadRequest()
        {
            // Arrange
            
            var controller = new ClientController(DbContext, _mapper);

            // Act
            var result = await controller.UpdateClient(Guid.Empty, new ClientDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        //[Fact]
        public async Task GivenClientWithExistingEmail_WhenUpdatingClient_ThenReturnsBadRequest()
        {
            // Arrange
            
            var controller = new ClientController(DbContext, _mapper);
            var id = Guid.NewGuid();

            var existingClient1 = new ClientDTO
            { 
                Code = Guid.NewGuid(), 
                Name = "Client 1",
                Email = Guid.NewGuid() + "client1@example.com"
            };
            var existingClient2 = new ClientDTO
            { 
                Code = id,
                Name = "Client 2",
                Email = Guid.NewGuid() + "client2@example.com"
            };
            await controller.CreateClient(existingClient1);
            await controller.CreateClient(existingClient2);

            var updatedClientDTO = new ClientDTO 
            { 
                Code = id,
                Name = "Updated Client",
                Email = existingClient1.Email
            };

            // Act
            DbContext.ChangeTracker.Clear();
            var result = await controller.UpdateClient(id, updatedClientDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
