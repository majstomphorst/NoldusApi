using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    public class BookController : ControllerBase
    {
        private readonly IBookRepo _bookRepo;
        private readonly IAuthorRepo _authorRepo;
        private readonly IMapper _mapper;
        private readonly BookService _bookService;

        public BookController(BookService bookService, IBookRepo bookRepo, IAuthorRepo authorRepo, IMapper mapper)
        {
            _bookService = bookService;
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _mapper = mapper;
        }
        
        //GET api/book/{id}
        [HttpGet("{id}", Name="GetBookById")]
        public async  Task<ActionResult<BookReadDto>> GetBookById(int id, bool includeBook=false)
        {
            var book = await _bookService.GetBookById(id, includeBook);
            
            if(book == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<BookReadDto>(book));
        }
        
        //POST api/book
        [HttpPost]
        public ActionResult<AuthorReadDto> PostBook(BookWriteDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            
            if (!_bookService.BookHasAuthor(book)) 
            {
                return BadRequest("no author found");
            }
            
            _bookService.CreateBook(book);

            var bookReadDto = _mapper.Map<BookReadDto>(book);
            return CreatedAtAction(nameof(GetBookById), new {Id = bookReadDto.Id},bookReadDto);
        }
        
        //POST api/book/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> SetBookImage(int id, IFormFile file)
        {
            var book = _bookRepo.GetBookById(id);
            if (book == null || file == null)
            {
                return NotFound();
            }

            var oldFile = book.CoverImage;
            if (oldFile != null)
            {
                Helpers.FileStorage.RmFileIfExists(oldFile);
            }

            var name = await Helpers.FileStorage.StoreImage(file);

            book.CoverImage = name;
            _bookRepo.SaveChanges();

            return Ok();
        }
        
        //DELETE api/book
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (await _bookService.BookExists(id))
            {
                return NotFound("Unknown book id.");
            }

            await _bookService.DeleteBook(id);
            
            return NoContent();
        }
    } 
}