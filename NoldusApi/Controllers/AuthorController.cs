using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NoldusApi.DataAccess;
using NoldusApi.Dtos;
using NoldusApi.Dtos.AuthorDtos;
using NoldusApi.Models;

namespace NoldusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepo _repo;
        private readonly IMapper _mapper;

        public AuthorController(IAuthorRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        //GET api/author/{id}
        [HttpGet("{id}", Name="GetAuthorById")]
        public ActionResult <AuthorReadDto> GetAuthorById(int id, bool withBooks=false)
        {
            Author author = withBooks ? _repo.GetAuthorByIdWithFirstRelation(id) : _repo.GetAuthorById(id);
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
            // TODO: validation
            var author = _mapper.Map<Author>(authorDto);   
            
            _repo.CreateAuthor(author);
            _repo.SaveChanges();
            
            var authorReadDto = _mapper.Map<AuthorReadDto>(author);
            return CreatedAtAction(nameof(GetAuthorById), new {Id = authorReadDto.Id},authorReadDto);
        }
        
        //PUT api/author/{id}
        [HttpPut("{id}")]
        public ActionResult PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            var authorDb = _repo.GetAuthorById(id);
            if (authorDb == null)
            {
                return NotFound();
            }

            _mapper.Map(authorDto, authorDb);
            _repo.SaveChanges();

            return NoContent();
        }
        
        //PATCH api/author/{id}
        [HttpPatch("{id}")]
        public ActionResult PatchAuthor(int id, JsonPatchDocument<AuthorUpdateDto> patchDoc)
        {
            var authorDb = _repo.GetAuthorById(id);
            if (authorDb == null)
            {
                return NotFound();
            }

            var authorPatch = _mapper.Map<AuthorUpdateDto>(authorDb);
            patchDoc.ApplyTo(authorPatch, ModelState);
            if (TryValidateModel(authorPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(authorPatch, authorDb);
            _repo.SaveChanges();
            
            return NoContent();
        }
        
        //DELETE api/author/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var authorDb = _repo.GetAuthorByIdWithFirstRelation(id);
            if (authorDb == null)
            {
                return NotFound();
            }

            if (authorDb.Books.Count > 0)
            {
                return BadRequest();
            }

            _repo.DeleteAuthor(authorDb);
            _repo.SaveChanges();
            return NoContent();
        }   
    }
}