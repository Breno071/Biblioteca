using Domain.Interfaces;
using Domain.Models.Entities;
using Infraestructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController(BaseDbContext dbContext, IReservationService reservationService) : ControllerBase
    {
        private readonly BaseDbContext _context = dbContext;
        private readonly IReservationService _reservationService = reservationService;

        /// <summary>
        /// Atualiza o estoque de um livro específico.
        /// </summary>
        /// <param name="id">ID do livro.</param>
        /// <param name="stock">Novo valor de estoque.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("update-book-stock/{id:Guid}")]
        public async Task<IActionResult> SetBookStock(Guid id, int stock)
        {
            if (stock < 0)
                return BadRequest("Estoque não pode ser negativo.");

            if (id == Guid.Empty)
                return BadRequest("Id não pode ser nulo ou vazio.");

            var book = await _context.Books
                .FirstOrDefaultAsync(x => x.Code == id);

            if (book is null)
                return NotFound("Livro não encontrado.");

            book.Stock = stock;
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Consulta o estoque de um livro específico.
        /// </summary>
        /// <param name="id">ID do livro.</param>
        /// <returns>Quantidade disponível em estoque do livro.</returns>
        [HttpGet("consult-book-stock/{id:Guid}")]
        public async Task<IActionResult> ConsultBookStock(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id não pode ser nulo ou vazio.");

            var book = await _context.Books
                            .FirstOrDefaultAsync(x => x.Code == id);

            if (book is null)
                return NotFound();

            return Ok(book.Stock);
        }
    }


}
