using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IEnumerable<Author> GetAllAuthors()
        {
            return _authorRepo.GetAllAuthors();
        }

        public void CreateAuthor(Author author)
        {
            _authorRepo.CreateAuthor(author);
            _authorRepo.SaveChanges();
        }

        public void CreateAuthors(IEnumerable<Author> authors)
        {
            foreach (var author in authors)
            {
                _authorRepo.CreateAuthor(author);
            }
            _bookRepo.SaveChanges();
        }

        public async Task<Author> GetAuthorById(int id, bool includeAuthor = false)
        {
            Author author = includeAuthor ? _authorRepo.GetAuthorByIdWithFirstRelation(id) : _authorRepo.GetAuthorById(id);
            return author;
        }

        public bool AuthorCanBeRemoved(Author author) => author.Books.Any();


        public void DeleteAuthor(Author author)
        { 
            _authorRepo.DeleteAuthor(author);
            _authorRepo.SaveChanges();
        }
        
        public bool validAuthors(IEnumerable<Author> authors)
        {
            return !authors.Any(x => x.FirstName.ToLower() == "dark" && x.LastName.ToLower() == "father");
        }
    }
}