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
            
            var controller = new ClientController(DbContext, _mapper);
            var id = Guid.NewGuid();

            var client = new ClientDTO
            { 
                Code = id, 
                Name = "Client 1",
                Email = "teste@email.com"
            };
            await controller.CreateClient(client);

            var clientDTO = new ClientDTO 
            { 
                Code = client.Code, 
                Name = client.Name,
                Email = client.Email    
            };

            // Act
            var result = await controller.GetClientById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClientDTO = Assert.IsType<ClientDTO>(okResult.Value);

            Assert.Equal(clientDTO.Code, returnedClientDTO.Code);
            Assert.Equal(clientDTO.Name, returnedClientDTO.Name);
            Assert.Equal(clientDTO.Email, returnedClientDTO.Email);
        }

        [Fact]
        public async Task GivenEmptyId_WhenGettingClient_ThenReturnsBadRequest()
        {
            // Arrange
            
            var controller = new ClientController(DbContext, _mapper);

            // Act
            var result = await controller.GetClientById(Guid.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenNonExistentId_WhenGettingClient_ThenReturnsNotFound()
        {
            // Arrange
            
            var controller = new ClientController(DbContext, _mapper);
            var id = Guid.NewGuid();

            // Act
            var result = await controller.GetClientById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
