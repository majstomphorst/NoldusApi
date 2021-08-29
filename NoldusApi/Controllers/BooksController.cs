using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NoldusApi.DataAccess;
using NoldusApi.Dtos;
using NoldusApi.Dtos.BookDtos;
using NoldusApi.Models;

namespace NoldusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepo _bookRepo;
        private readonly IAuthorRepo _authorRepo;
        private readonly IMapper _mapper;

        public BooksController(IBookRepo bookRepo, IAuthorRepo authorRepo, IMapper mapper)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _mapper = mapper;
        }
        
        //GET api/books
        [HttpGet]
        public ActionResult<IEnumerable<BookReadDto>> GetBooks()
        {
            var books = _bookRepo.GetAllBooks();

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