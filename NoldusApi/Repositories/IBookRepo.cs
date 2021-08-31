using System.Collections.Generic;
using NoldusApi.Models;

namespace NoldusApi.DataAccess
{
    public interface IBookRepo
    {
        
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> GetAllBooksWithAuthor();
        
        Book GetBookById(int id);
        
        Book GetBookByIdWithFirstRelation(int id);

        void CreateBook(Book book);

        void UpdateBook(Book book);

        void DeleteBook(Book book);
        
        bool SaveChanges();
    }
}