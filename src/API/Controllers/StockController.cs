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

        //UpdateBookStock
        [HttpPut("update-book-stock/{id:Guid}")]
        public async Task<IActionResult> UpdateBookStock(Guid id, int stock)
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

        //ConsultBookStock
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

        //MakeReservation
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

        // FinishReservation
        [HttpPut("finish-reservation/{reservationCode:Guid}")]
        public async Task<IActionResult> FinishReservation(Guid reservationCode)
        {
            if (reservationCode == Guid.Empty)
                return BadRequest("Id não pode ser nulo ou vazio.");

            var reservation = await _context.Reservations
                            .FirstOrDefaultAsync(x => x.Code == reservationCode);

            if (reservation is null)
                return NotFound();

            reservation.IsReturned = true;
            reservation.ReturnDate = DateTime.Now;

            foreach (var book in reservation.Books)
                book.Stock += 1;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
