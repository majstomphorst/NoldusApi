using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoldusApi.Models;

namespace NoldusApi.DataAccess
{
    public class BookRepo : IBookRepo
    {
        private readonly ApiContext _context;

        public BookRepo(ApiContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            var books = _context.Book.ToList();
            return books;
        }

        public IEnumerable<Book> GetAllBooksWithAuthor()
        {
            var books = _context.Book.ToList();
            foreach (var book in books)
            {
                var author = _context.Authors.First(x => x.Id == book.AuthorId);
                book.Author = author;
            }

            return books;
        }


        public Book GetBookById(int id)
        {
            var book = _context.Book.FirstOrDefault(x => x.Id == id);
            return book;
        }

        public Book GetBookByIdWithFirstRelation(int id)
        {
            var book = _context.Book.First(x => x.Id == id);
            var author = _context.Authors.First(x => x.Id == book.AuthorId);
            book.Author = author;
            return book;
        }

        public void CreateBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            _context.Book.Add(book);
        }

        public void UpdateBook(Book book)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteBook(Book book)
        {
            _context.Book.Remove(book);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}