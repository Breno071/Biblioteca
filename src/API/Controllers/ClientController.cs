using AutoMapper;
using Domain.Models.DTO;
using Domain.Models.Entities;
using Infraestructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController(BaseDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly BaseDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        //GetClients
        [HttpGet("get-clients/{skip:int}/{take:int}")]
        public async Task<IActionResult> GetClients(int skip, int take)
        {
            int limit = 1000;

            if (skip < 0 || take < 0)
                return BadRequest("Parâmetros não podem ser menores que zero.");
            if (take > limit)
                return BadRequest($"Não é possível retornar mais de {limit} registros.");

            var clients = await _context.Clients
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var ClientDTOs = _mapper.Map<List<ClientDTO>>(clients);

            return Ok(ClientDTOs);
        }

        //GetClient
        [HttpGet("get-client/{id:Guid}")]
        public async Task<IActionResult> Getclient(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Insira um valor para realizar a busca.");

            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == id);
            var ClientDTO = _mapper.Map<ClientDTO>(client);

            if (ClientDTO is not null)
                return Ok(ClientDTO);

            return NotFound();
        }

        //GetClientsByEmail
        [HttpGet("get-clients-by-email/{email}")]
        public async Task<IActionResult> GetClientsByAuthor(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Insira um valor para realizar a busca.");

            var clients = await _context.Clients
                .Where(x => x.Email.Equals(email))
                .AsNoTracking()
                .ToListAsync();

            var ClientDTOs = _mapper.Map<List<ClientDTO>>(clients);

            return Ok(ClientDTOs);
        }

        //UpdateClient
        [HttpPut("update-client/{id:Guid}")]
        public async Task<IActionResult> Updateclient(Guid id, ClientDTO ClientDTO)
        {
            if (id == Guid.Empty || id != ClientDTO.Code)
                return BadRequest("Objeto ou parametro inválido.");

            //Check if there is another client with this email
            var exists = _context.Clients.Any(x => x.Code != ClientDTO.Code && x.Email.Equals(ClientDTO.Email));

            if (!exists)
                return NotFound("Já existe um cliente com este e-mail.");

            var client = _mapper.Map<Client>(ClientDTO);

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return Ok(client);
        }

        //CreateClient
        [HttpPost("create-client")]
        public async Task<IActionResult> Createclient(ClientDTO ClientDTO)
        {
            var exists = _context.Clients.Any(x => x.Code == ClientDTO.Code || x.Email.Equals(ClientDTO.Email));

            if (exists)
                return BadRequest("Cliente já existente.");

            var client = _mapper.Map<Client>(ClientDTO);

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return Ok(client);
        }
    }
}
