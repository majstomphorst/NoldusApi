using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoldusApi.DataAccess;
using NoldusApi.Dtos.AuthorDtos;
using NoldusApi.Dtos.BookDtos;
using NoldusApi.Models;

namespace NoldusApi.Services
{
    public class BookService
    {
        private readonly IBookRepo _bookRepo;
        private readonly IAuthorRepo _authorRepo;

        public BookService(IBookRepo bookRepo, IAuthorRepo authorRepo)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            var books = _bookRepo.GetAllBooks();

            var filteredBooks = this.FilteredBooks(books).ToList();
            
            return filteredBooks;
        }

        public async Task<Book> GetBookById(int id, bool includeAuthor = false)
        {
            Book author = includeAuthor ? _bookRepo.GetBookByIdWithFirstRelation(id) : _bookRepo.GetBookById(id);
            return author;
        }

        public async Task<IEnumerable<Book>> GetAllBooksWithAuthor()
        {
            var books = _bookRepo.GetAllBooksWithAuthor();

            var validatedBooks = FilteredBooks(books);
            return validatedBooks;
        }

        public async Task<bool> BookExists(int id)
        {
            var book = _bookRepo.GetBookById(id);
            return book != null;
        }

        public async Task DeleteBook(int id)
        {
            var book = _bookRepo.GetBookById(id);
            
            if (book.CoverImage != null)
            {
                Helpers.FileStorage.RmFileIfExists(book.CoverImage);
            }
            
            
            _bookRepo.DeleteBook(book);
            _bookRepo.SaveChanges();
        }

        public bool AllBooksHaveAuthor(IEnumerable<Book> books)
        {
            return books.All(b => _authorRepo.GetAuthorById(b.AuthorId) != null);
        }
        
        public Author GetAuthorById(int id)
        {
            return _authorRepo.GetAuthorById(id);
        }

        public async void CreateBook(Book book, BookWriteDto book2) // BAD!!!!!!!
        {
            if (book.CoverImage != null)
            {
                var name = await Helpers.FileStorage.StoreImage(book2.CoverImage);
                book.CoverImage = name;
            }

            _bookRepo.CreateBook(book);
            _bookRepo.SaveChanges();
        }

        public void CreateBooks(IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                _bookRepo.CreateBook(book);
            }
            _bookRepo.SaveChanges();
        }

        private IEnumerable<Book>FilteredBooks (IEnumerable<Book> books)
        {
            var pseudonymChecked = CheckForAuthorPseudonym(books);
            
            return pseudonymChecked;
        }

        public IEnumerable<Book> CheckForAuthorPseudonym(IEnumerable<Book> books)
        {
            return books.Where(book =>
            {
                var author = _authorRepo.GetAuthorById(book.AuthorId);
                
                if (author.Pseudonym != null && 
                    author.Pseudonym.ToLower() is "drokkattta" or "grakchawwaa" &&
                    book.Release < DateTime.Now.AddMonths(-24))
                {
                    return false;
                }
                return true;
            });
        }

        public bool BookHasAuthor(Book book)
        {
            var author = _authorRepo.GetAuthorById(book.AuthorId);
            return author != null;
        }
    }
}