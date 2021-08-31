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
        private readonly IMapper _mapper;
        private readonly BookService _bookService;

        public BooksController(BookService service, IMapper mapper)
        {
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
            if (!books.Any())
            {
                return BadRequest();
            }

            var allHaveAuthor = _bookService.AllBooksHaveAuthor(books);
            if (!allHaveAuthor)
            {
                return NotFound("One or more books do not have a valid author id");
            }
            
            _bookService.CreateBooks(books);

            var booksReadDto = _mapper.Map<IEnumerable<BookReadDto>>(books);
            string location = $"{Request.Scheme}://{Request.Host.Value}";
            return Created(location  + "api/books",booksReadDto);
            // var response = new CreatedAtActionResult(location,"/author",  "", booksReadDto);
            // return response;
        }
    }
}