using API.Controllers;
using AutoMapper;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Client.Get
{
    public class GetClientsTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        //[Fact]
        public async Task GivenValidSkipAndTake_WhenGettingClients_ThenReturnsOkResultWithClientDTOs()
        {
            // Arrange
            var controller = new ClientController(DbContext, _mapper);
            var skip = 0;
            var take = 5;

            var clients = new List<ClientDTO>
            {
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Name = "Client 1",
                    Email = "cliente1@email.com"
                },
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Name = "Client 2",
                    Email = "cliente2@email.com"
                },
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Name = "Client 3",
                    Email = "cliente3@email.com"
                }
            };


            // Act
            await controller.CreateClient(clients[0]);
            await controller.CreateClient(clients[1]);
            await controller.CreateClient(clients[2]);

            var clientDTOs = clients.Select(client => new ClientDTO 
            { 
                Code = client.Code, 
                Name = client.Name 
            }).ToList();
            var result = await controller.GetClients(skip, take);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClientDTOs = Assert.IsType<List<ClientDTO>>(okResult.Value);

            Assert.Equal(clientDTOs.Count, returnedClientDTOs.Count);
        }

        //[Fact]
        public async Task GivenNegativeSkipOrTake_WhenGettingClients_ThenReturnsBadRequest()
        {
            // Arrange
            
            var controller = new ClientController(DbContext, _mapper);
            var skip = -1;
            var take = 5;

            // Act
            var result = await controller.GetClients(skip, take);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        //[Fact]
        public async Task GivenTakeGreaterThanLimit_WhenGettingClients_ThenReturnsBadRequest()
        {
            // Arrange
            
            var controller = new ClientController(DbContext, _mapper);
            var skip = 0;
            var take = 1500;

            // Act
            var result = await controller.GetClients(skip, take);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
