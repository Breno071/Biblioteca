using Domain.Interfaces;
using Domain.Models.Entities;
using Infraestructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController(BaseDbContext dbContext, IReservationService reservationService) : ControllerBase
    {
        private readonly BaseDbContext _context = dbContext;
        private readonly IReservationService _reservationService = reservationService;

        /// <summary>
        /// Realiza uma reserva de livros para um cliente.
        /// </summary>
        /// <param name="clientCode">ID do cliente.</param>
        /// <param name="bookCodes">Lista de IDs de livros a serem reservados.</param>
        /// <param name="returnDate">Data de retorno desejada para os livros.</param>
        /// <returns>Dados da reserva realizada.</returns>
        [HttpPost("make-reservation")]
        public async Task<IActionResult> MakeReservation(Guid clientCode, List<Guid> bookCodes, DateTime? returnDate)
        {
            if (clientCode == Guid.Empty)
                return BadRequest("Id não pode ser nulo ou vazio.");

            if (bookCodes is null || bookCodes.Count == 0)
                return BadRequest("Os livros não foram informados.");

            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Code == clientCode);

            if (client is null)
                return NotFound("Cliente não encontrado.");

            List<Book> books = [];
            foreach (var bookCode in bookCodes)
            {
                var book = await _context.Books.FirstOrDefaultAsync(x => x.Code == bookCode);

                if (book is null)
                    return NotFound($"Livro com o código {bookCode} não encontrado na base.");

                if (book.Stock == 0)
                    return BadRequest($"Livro com o código {bookCode} não possui estoque.");
                books.Add(book);

                book.Stock -= 1;
                await _context.SaveChangesAsync();
            }

            Reservation reservation = new()
            {
                Client = client,
                Books = books,
                ReservationDate = DateTime.Now,
                ReturnDate = returnDate ?? DateTime.Now.AddDays(7)
            };
            await _reservationService.AddReservation(reservation);
            return Ok(reservation);
        }

        /// <summary>
        /// Finaliza uma reserva, atualizando o estoque e o status da reserva.
        /// </summary>
        /// <param name="reservationCode">ID da reserva.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("finish-reservation/{reservationCode:Guid}")]
        public async Task<IActionResult> FinishReservation(Guid reservationCode)
        {
            if (reservationCode == Guid.Empty)
                return BadRequest("Id não pode ser nulo ou vazio.");

            var reservation = await _context.Reservations
                            .Include(x => x.Books)
                            .Include(x => x.Client)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Code == reservationCode);

            if (reservation is null)
                return NotFound();

            foreach (var book in reservation.Books)
                book.Stock += 1;

            await _context.SaveChangesAsync();
            await _reservationService.FinishReservation(reservation);

            return Ok();
        }
    }
}
