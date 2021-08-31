using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NoldusApi.DataAccess;
using NoldusApi.Dtos;
using NoldusApi.Dtos.AuthorDtos;
using NoldusApi.Models;
using NoldusApi.Services;

namespace NoldusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AuthorService _authorService;

        public AuthorController(AuthorService authorService, IMapper mapper)
        {
            _mapper = mapper;
            _authorService = authorService;
        }

        //GET api/author/{id}
        [HttpGet("{id}", Name="GetAuthorById")]
        public async Task<ActionResult<AuthorReadDto>> GetAuthorById(int id, bool withBooks=false)
        {
            var author = await _authorService.GetAuthorById(id, withBooks);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorReadDto>(author));
        }
        
        //POST api/author
        [HttpPost]
        public ActionResult<AuthorReadDto> PostAuthor(AuthorWriteDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            
            _authorService.CreateAuthor(author);

            var authorReadDto = _mapper.Map<AuthorReadDto>(author);
            return CreatedAtAction(nameof(GetAuthorById), new {Id = authorReadDto.Id},authorReadDto);
        }
        
        // //PUT api/author/{id}
        // [HttpPut("{id}")]
        // public ActionResult PutAuthor(int id, AuthorUpdateDto authorDto)
        // {
        //     var authorDb = _repo.GetAuthorById(id);
        //     if (authorDb == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _mapper.Map(authorDto, authorDb);
        //     _repo.SaveChanges();
        //
        //     return NoContent();
        // }
        //
        // //PATCH api/author/{id}
        // [HttpPatch("{id}")]
        // public ActionResult PatchAuthor(int id, JsonPatchDocument<AuthorUpdateDto> patchDoc)
        // {
        //     var authorDb = _repo.GetAuthorById(id);
        //     if (authorDb == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var authorPatch = _mapper.Map<AuthorUpdateDto>(authorDb);
        //     patchDoc.ApplyTo(authorPatch, ModelState);
        //     if (TryValidateModel(authorPatch))
        //     {
        //         return ValidationProblem(ModelState);
        //     }
        //
        //     _mapper.Map(authorPatch, authorDb);
        //     _repo.SaveChanges();
        //     
        //     return NoContent();
        // }
        
        
        //DELETE api/author/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorService.GetAuthorById(id, true);
            if (author == null)
            {
                return NotFound();
            }

            if (!_authorService.AuthorCanBeRemoved(author))
            {
                return BadRequest();
            }

            _authorService.DeleteAuthor(author);
            
            return NoContent();
        }   
    }
}