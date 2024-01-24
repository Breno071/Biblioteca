using AutoMapper;
using Domain.Enums;
using Domain.Models.DTO;
using Domain.Models.Entities;
using Infraestructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(BaseDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly BaseDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Obtém uma lista paginada de livros.
        /// </summary>
        /// <param name="skip">Número de registros a serem ignorados.</param>
        /// <param name="take">Número máximo de registros a serem retornados.</param>
        /// <returns>Lista paginada de livros.</returns>
        [HttpGet("get-books/{skip:int}/{take:int}")]
        public async Task<IActionResult> GetBooks(int skip, int take)
        {
            int limit = 1000;

            if (skip < 0 || take < 0)
                return BadRequest("Parâmetros não podem ser menores que zero.");
            if (take > limit)
                return BadRequest($"Não é possível retornar mais de {limit} registros.");

            var books = await _context.Books
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .OrderBy(x => x.Title)
                .ToListAsync();

            var bookDTOs = _mapper.Map<List<BookDTO>>(books);

            return Ok(bookDTOs);
        }

        /// <summary>
        /// Obtém um livro por ID.
        /// </summary>
        /// <param name="id">ID do livro.</param>
        /// <returns>Informações do livro.</returns>
        [HttpGet("get-book/{id:Guid}")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Insira um valor para realizar a busca.");

            var book = await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == id);
            var bookDTO = _mapper.Map<BookDTO>(book);

            if (bookDTO is not null)
                return Ok(bookDTO);

            return NotFound();
        }

        /// <summary>
        /// Obtém uma lista de livros por autor.
        /// </summary>
        /// <param name="author">Nome do autor.</param>
        /// <returns>Lista de livros do autor.</returns>
        [HttpGet("get-books-by-author/{author}")]
        public async Task<IActionResult> GetBooksByAuthor(string author)
        {
            if (string.IsNullOrEmpty(author))
                return BadRequest("Insira um valor para realizar a busca.");

            var books = await _context.Books
                .Where(x => x.Author.Equals(author))
                .AsNoTracking()
                .OrderBy(x => x.Title)
                .ToListAsync();

            return Ok(books);
        }

        /// <summary>
        /// Obtém uma lista de livros por título.
        /// </summary>
        /// <param name="title">Título do livro.</param>
        /// <returns>Lista de livros com o título especificado.</returns>
        [HttpGet("get-books-by-title/{title}")]
        public async Task<IActionResult> GetBooksByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return BadRequest("Insira um valor para realizar a busca.");

            var books = await _context.Books
                .Where(x => x.Title.Equals(title))
                .AsNoTracking()
                .OrderBy(x => x.Code)
                .ToListAsync();

            return Ok(books);
        }

        /// <summary>
        /// Obtém uma lista de livros por gênero.
        /// </summary>
        /// <param name="genre">Gênero do livro.</param>
        /// <returns>Lista de livros do gênero especificado.</returns>
        [HttpGet("get-books-by-genre/{genre}")]
        public async Task<IActionResult> GetBooksByGenre(Genre genre)
        {
            var books = await _context.Books
                .Where(x => x.Genre == genre)
                .AsNoTracking()
                .OrderBy(x => x.Title)
                .ToListAsync();

            return Ok(books);
        }

        /// <summary>
        /// Obtém uma lista de livros por ano de publicação.
        /// </summary>
        /// <param name="year">Ano de publicação dos livros.</param>
        /// <returns>Lista de livros publicados no ano especificado.</returns>
        [HttpGet("get-books-by-year/{year:int}")]
        public async Task<IActionResult> GetBooksByYear(int year)
        {
            var books = await _context.Books
                .Where(x => x.Year == year)
                .AsNoTracking()
                .OrderBy(x => x.Title)
                .ToListAsync();

            var bookDTOs = _mapper.Map<List<BookDTO>>(books);

            return Ok(bookDTOs);
        }

        /// <summary>
        /// Obtém uma lista de livros por editora.
        /// </summary>
        /// <param name="publisher">Nome da editora.</param>
        /// <returns>Lista de livros da editora especificada.</returns>
        [HttpGet("get-books-by-publisher/{publisher}")]
        public async Task<IActionResult> GetBooksByPublisher(string publisher)
        {
            if (string.IsNullOrEmpty(publisher))
                return BadRequest("Insira um valor para realizar a busca.");

            var books = await _context.Books
                .Where(x => x.Publisher.Equals(publisher))
                .AsNoTracking()
                .OrderBy(x => x.Title)
                .ToListAsync();

            return Ok(books);
        }

        /// <summary>
        /// Atualiza as informações de um livro.
        /// </summary>
        /// <param name="id">ID do livro a ser atualizado.</param>
        /// <param name="bookDTO">Novas informações do livro.</param>
        /// <returns>Informações atualizadas do livro.</returns>
        [HttpPut("update-book/{id:Guid}")]
        public async Task<IActionResult> UpdateBook(Guid id, BookDTO bookDTO)
        {
            if (id == Guid.Empty)
                return BadRequest("id não pode ser vazio.");

            if (id != bookDTO.Code)
                return BadRequest("Id inválido.");

            if (bookDTO == null)
                return BadRequest("Objeto não pode ser null.");

            var exists = _context.Books.Any(x => x.Code == id);

            if (!exists)
                return NotFound();

            var book = _mapper.Map<Book>(bookDTO);

            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return Ok(book);
        }

        /// <summary>
        /// Cria um novo livro.
        /// </summary>
        /// <param name="bookDTO">Informações do novo livro a ser criado.</param>
        /// <returns>Informações do livro recém-criado.</returns>
        [HttpPost("create-book")]
        public async Task<IActionResult> CreateBook(BookDTO bookDTO)
        {
            var exists = _context.Books.Any(x => x.Code == bookDTO.Code);

            if (exists)
                return BadRequest("Livro já existente.");

            var book = _mapper.Map<Book>(bookDTO);

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return Ok(book);
        }

        /// <summary>
        /// Exclui um livro por ID.
        /// </summary>
        /// <param name="id">ID do livro a ser excluído.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete("delete-book/{id:Guid}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id não pode ser nulo");

            var book = await _context.Books.FirstOrDefaultAsync(x => x.Code == id);

            if (book is not null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            return NotFound();
        }
    }
}
