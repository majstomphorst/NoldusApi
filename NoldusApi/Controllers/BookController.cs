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
        private readonly IMapper _mapper;
        private readonly BookService _bookService;

        public BookController(BookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        
        //GET api/book/{id}
        [HttpGet("{id}", Name="GetBookById")]
        public async Task<ActionResult<BookReadDto>> GetBookById(int id, bool includeBook=false)
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
        public ActionResult<AuthorReadDto> PostBook([FromForm]BookWriteDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);

            if (!_bookService.BookHasAuthor(book)) 
            {
                return BadRequest("no author found");
            }

            _bookService.CreateBook(book, bookDto.CoverImage); //// BAD!

            var bookReadDto = _mapper.Map<BookReadDto>(book);
            return CreatedAtAction(nameof(GetBookById), new {Id = bookReadDto.Id},bookReadDto);
        }
        
        //DELETE api/book
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (!await _bookService.BookExists(id))
            {
                return NotFound("Unknown book id.");
            }

            await _bookService.DeleteBook(id);
            return NoContent();
        }
    } 
}