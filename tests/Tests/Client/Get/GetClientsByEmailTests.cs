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
        //[Fact]
        public async Task GivenValidEmail_WhenGettingClients_ThenReturnsOkResultWithClientDTOs()
        {
            // Arrange
            
            var controller = new ClientController(DbContext, _mapper);
            var email = "test@example.com";

            var clients = new List<Domain.Models.Entities.Client>
            {
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Name = "Client 1", 
                    Email = email 
                },
                new() 
                { 
                    Code = Guid.NewGuid(), 
                    Name = "Client 2", 
                    Email = email 
                }
            };


            // Act
            DbContext.Clients.AddRange(clients);
            DbContext.SaveChanges();

            var clientDTOs = clients.Select(client => new ClientDTO 
            { 
                Code = client.Code, 
                Name = client.Name, 
                Email = client.Email 
            }).ToList();
            var result = await controller.GetClientsByEmail(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClientDTOs = Assert.IsType<List<ClientDTO>>(okResult.Value);

            Assert.Equal(clientDTOs.Count, returnedClientDTOs.Count);
        }

        //[Fact]
        public async Task GivenNonExistentEmail_WhenGettingClients_ThenReturnsOkResultWithEmptyList()
        {
            // Arrange
            var controller = new ClientController(DbContext, _mapper);
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
