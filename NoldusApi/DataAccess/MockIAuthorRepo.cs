using System.Collections.Generic;
using NoldusApi.Models;

namespace NoldusApi.DataAccess
{
    public class MockIAuthorRepo : IAuthorRepo
    {
        private IAuthorRepo _authorRepoImplementation;

        public bool SaveChanges()
        {
            //
            return true;
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            var authors = new List<Author>
            {
                new Author { Id = 0, FirstName = "Henk", LastName = "De Schrijver", Pseudonym = "H.S", Books = null},
                new Author { Id = 1, FirstName = "Ingrid", LastName = "De Pen", Pseudonym = "I.P", Books = null},
                new Author { Id = 2, FirstName = "Klaas", LastName = "De Typemachine", Pseudonym = "Computer", Books = null},
            };
            return authors;
        }

        public Author GetAuthorById(int id)
        {
            if (id != 1)
            {
                return null;
            }
            
            return new Author
                {Id = 1, FirstName = "Klaas", LastName = "De Typemachine", Pseudonym = "Computer", Books = null};
        }

        public Author GetAuthorByIdWithFirstRelation(int id)
        {
            throw new System.NotImplementedException();
        }

        public void CreateAuthor(Author input)
        {
            throw new System.NotImplementedException();
        }

        public void CreateMultiple(IEnumerable<Author> authors)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateAuthor(Author input)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAuthor(Author author)
        {
            throw new System.NotImplementedException();
        }
    }
}