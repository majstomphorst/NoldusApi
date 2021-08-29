using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NoldusApi.DataAccess;
using NoldusApi.Dtos;
using NoldusApi.Dtos.AuthorDtos;
using NoldusApi.Models;

namespace NoldusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepo _repo;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        //GET api/authors
        [HttpGet]
        public ActionResult<IEnumerable<AuthorReadDto>> GetAuthors()
        {
            var authors = _repo.GetAllAuthors();

            return Ok(_mapper.Map<IEnumerable<AuthorReadDto>>(authors));
        }
        
        //POST api/Authors
        [HttpPost]
        public ActionResult<IEnumerable<AuthorReadDto>> PostAuthors(IEnumerable<AuthorWriteDto> authorsDto)
        {
            var authors = _mapper.Map<IEnumerable<Author>>(authorsDto);

            foreach (var author in authors)
            {
                _repo.CreateAuthor(author);
            }
            _repo.SaveChanges();
            
            var authorsReadDto = _mapper.Map<IEnumerable<AuthorReadDto>>(authors);
            string location = $"{Request.Scheme}://{Request.Host.Value}/{Request.Path}";
            
            var response = new CreatedAtActionResult("","author", "1", authorsReadDto);
            return response;
            // return Created(location, authorsReadDto);
        }
    }
}