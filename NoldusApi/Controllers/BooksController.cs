using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NoldusApi.DataAccess;
using NoldusApi.Dtos;
using NoldusApi.Dtos.BookDtos;
using NoldusApi.Models;
using NoldusApi.Services;

namespace NoldusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepo _bookRepo;
        private readonly IAuthorRepo _authorRepo;
        private readonly IMapper _mapper;
        private readonly BookService _bookService;

        public BooksController(IBookRepo bookRepo, IAuthorRepo authorRepo, BookService service, IMapper mapper)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _bookService = service;
            _mapper = mapper;
        }
        
        //GET api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadDto>>> GetBooks()
        {
            var books =  await _bookService.GetAllBooks();
            
            // TODO: mapping in service?
            return Ok(_mapper.Map<IEnumerable<BookReadDto>>(books));
        }
        
        //POST api/books
        [HttpPost]
        public ActionResult<IEnumerable<BookReadDto>> PostBooks(IEnumerable<BookWriteDto> authorsDto)
        {
            var books = _mapper.Map<IEnumerable<Book>>(authorsDto);
            
            var allHaveAuthor = books.All(b => _authorRepo.GetAuthorById(b.AuthorId) != null);
            if (!allHaveAuthor)
            {
                return NotFound();
            }

            foreach (var book in books)
            {
                _bookRepo.CreateBook(book);
            }
            _bookRepo.SaveChanges();
            
            var booksReadDto = _mapper.Map<IEnumerable<BookReadDto>>(books);
            string location = $"{Request.Scheme}://{Request.Host.Value}"; //  "/{Request.Path}";
            var response = new CreatedAtActionResult(location,"/author",  "", booksReadDto);
            return response;
        }
    }
}