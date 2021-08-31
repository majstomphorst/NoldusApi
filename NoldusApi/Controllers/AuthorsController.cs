using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepo _repo;
        private readonly IMapper _mapper;
        private readonly AuthorService _authorService;

        public AuthorsController(AuthorService authorService, IAuthorRepo repo, IMapper mapper)
        {
            _authorService = authorService;
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
            if (!authors.Any())
            {
                return BadRequest();
            }

            if (!_authorService.validAuthors(authors))
            {
                return BadRequest();
            }
            
            _authorService.CreateAuthors(authors);


            var authorsReadDto = _mapper.Map<IEnumerable<AuthorReadDto>>(authors);
            return Created("",authorsReadDto);
        }
    }
}