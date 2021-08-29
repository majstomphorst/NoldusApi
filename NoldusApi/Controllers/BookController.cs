using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoldusApi.DataAccess;
using NoldusApi.Dtos;
using NoldusApi.Dtos.BookDtos;
using NoldusApi.Models;

namespace NoldusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepo _bookRepo;
        private readonly IAuthorRepo _authorRepo;
        private readonly IMapper _mapper;

        public BookController(IBookRepo bookRepo, IAuthorRepo authorRepo, IMapper mapper)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _mapper = mapper;
        }
        
        //GET api/book/{id}
        [HttpGet("{id}", Name="GetBookById")]
        public ActionResult<BookReadDto> GetBookById(int id, bool withBooks=false)
        {
            Book author = withBooks ? _bookRepo.GetBookByIdWithFirstRelation(id) : _bookRepo.GetBookById(id);
            
            if(author != null)
            {
                return Ok(_mapper.Map<BookReadDto>(author));
            }
            return NotFound();
        }
        
        //POST api/book
        [HttpPost]
        public  ActionResult<AuthorReadDto> PostBook(BookWriteDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);

            var author = _authorRepo.GetAuthorById(book.AuthorId);
            if (author == null)
            {
                return NotFound();
            }
            
            _bookRepo.CreateBook(book);
            _bookRepo.SaveChanges();
            
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
        public IActionResult DeleteBook(int id)
        {
            var bookDb = _bookRepo.GetBookById(id);
            if (bookDb == null)
            {
                return NotFound();
            }
            _bookRepo.DeleteBook(bookDb);
            _bookRepo.SaveChanges();
            return NoContent();
        }
    } 
}