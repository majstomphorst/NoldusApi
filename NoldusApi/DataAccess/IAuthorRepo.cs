using System.Collections.Generic;
using NoldusApi.Models;

namespace NoldusApi.DataAccess
{
    public interface IAuthorRepo
    {
        IEnumerable<Author> GetAllAuthors();
        Author GetAuthorById(int id);
        Author GetAuthorByIdWithFirstRelation(int id);
        
        void CreateAuthor(Author author);

        void UpdateAuthor(Author author);

        void DeleteAuthor(Author author);
        
        bool SaveChanges();
    }
}