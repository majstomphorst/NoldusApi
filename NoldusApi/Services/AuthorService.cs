using System.Collections.Generic;
using System.Linq;
using NoldusApi.DataAccess;
using NoldusApi.Models;

namespace NoldusApi.Services
{
    public class AuthorService
    {
        private readonly IBookRepo _bookRepo;
        private readonly IAuthorRepo _authorRepo;

        public AuthorService(IBookRepo bookRepo, IAuthorRepo authorRepo)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
        }
        
        public void CreateAuthors(IEnumerable<Author> authors)
        {
            foreach (var author in authors)
            {
                _authorRepo.CreateAuthor(author);
            }
            _bookRepo.SaveChanges();
        }

        public bool validAuthors(IEnumerable<Author> authors)
        {
            return authors.Any(x => x.FirstName.ToLower() != "dark" && x.LastName.ToLower() != "father");
        }
    }
}