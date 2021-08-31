using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoldusApi.DataAccess;
using NoldusApi.Models;

namespace NoldusApi.Services
{
    public class BookService
    {
        private readonly IBookRepo _repo;

        public BookService(IBookRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            var books = _repo.GetAllBooks();
            return books;
        }

        public async Task<IEnumerable<Book>> GetAllBooksWithAuthor()
        {
            var books = _repo.GetAllBooksWithAuthor();

            var validatedBooks = ValidateBooks(books);
            return validatedBooks;
        }

        private IEnumerable<Book> ValidateBooks(IEnumerable<Book> books)
        {
            var pseudonymChecked = CheckForAuthorPseudonym(books);


            return pseudonymChecked;
        }

        private IEnumerable<Book> CheckForAuthorPseudonym(IEnumerable<Book> books)
        {
            return books.Where(book =>
            {
                if (book.Author.Pseudonym.ToLower() is "drokkattta" or "grakchawwaa" && 
                    book.Release < DateTime.Now.AddMonths(-24))
                {
                    return false;
                }
                return true;
            });
        }
        
    }
}