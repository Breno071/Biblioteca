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

        /// <summary>
        /// Obtém uma lista paginada de clientes.
        /// </summary>
        /// <param name="skip">Número de registros a serem ignorados.</param>
        /// <param name="take">Número máximo de registros a serem retornados.</param>
        /// <returns>Lista paginada de clientes.</returns>
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
                .OrderBy(x => x.Name)
                .ToListAsync();

            var ClientDTOs = _mapper.Map<List<ClientDTO>>(clients);

            return Ok(ClientDTOs);
        }

        /// <summary>
        /// Obtém um cliente por ID.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        /// <returns>Informações do cliente.</returns>
        [HttpGet("get-client/{id:Guid}")]
        public async Task<IActionResult> GetClient(Guid id)
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

        /// <summary>
        /// Obtém uma lista de clientes por e-mail.
        /// </summary>
        /// <param name="email">Endereço de e-mail do cliente.</param>
        /// <returns>Lista de clientes com o e-mail especificado.</returns>
        [HttpGet("get-clients-by-email/{email}")]
        public async Task<IActionResult> GetClientsByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Insira um valor para realizar a busca.");

            var clients = await _context.Clients
                .Where(x => x.Email.Equals(email))
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();

            var ClientDTOs = _mapper.Map<List<ClientDTO>>(clients);

            return Ok(ClientDTOs);
        }

        /// <summary>
        /// Atualiza as informações de um cliente.
        /// </summary>
        /// <param name="id">ID do cliente a ser atualizado.</param>
        /// <param name="clientDTO">Novas informações do cliente.</param>
        /// <returns>Informações atualizadas do cliente.</returns>
        [HttpPut("update-client/{id:Guid}")]
        public async Task<IActionResult> UpdateClient(Guid id, ClientDTO ClientDTO)
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

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="clientDTO">Informações do novo cliente a ser criado.</param>
        /// <returns>Informações do cliente recém-criado.</returns>
        [HttpPost("create-client")]
        public async Task<IActionResult> CreateClient(ClientDTO ClientDTO)
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
