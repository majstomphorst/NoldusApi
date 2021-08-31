using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoldusApi.DataAccess;
using NoldusApi.Models;
using NoldusApi.Services;
using Xunit;

namespace TestNoldusApi.UnitTests
{
    public class BaseServicesTests
    {
        protected readonly ApiContext _context;
        protected readonly IAuthorRepo _authorRepo;
        protected readonly BookRepo _bookRepo;
        protected readonly BookService _service;

        public BaseServicesTests()
        {
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "memory")
                .Options;
            
            _context = new ApiContext(options);
            EmptyDatabase();
            SeedDatabase();

            _authorRepo = new AuthorRepo(_context);
            _bookRepo = new BookRepo(_context);
            _service = new BookService(_bookRepo);
        }

        #region Init

        protected void SeedDatabase()
        {
            var authors = GetSeedingAuthors();
            var books = GetSeedingBooks();
            
            foreach (var author in authors)
            {
                _context.Authors.Add(author);
            }
            foreach (var book in books)
            {
                _context.Book.Add(book);
            }
            _context.SaveChanges();
        }

        protected void EmptyDatabase()
        {
            _context.Book.RemoveRange(_context.Book);
            _context.SaveChanges();
            
            _context.Authors.RemoveRange(_context.Authors);
            _context.SaveChanges();
        }

        protected IEnumerable<Author> GetSeedingAuthors()
        {
            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "Henk", LastName = "De Schrijver", Pseudonym = "H.S", Books = null},
                new Author { Id = 2, FirstName = "Ingrid", LastName = "De Pen", Pseudonym = "I.P", Books = null},
                new Author { Id = 3, FirstName = "Klaas", LastName = "De Typemachine", Pseudonym = "drokkattta", Books = null},
                new Author { Id = 5, FirstName = "Piet", LastName = "Coder", Pseudonym = "grakchawwaa", Books = null},
            };
            return authors;
        }

        protected IEnumerable<Book> GetSeedingBooks()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "There is a star", Description = "A star is born.", CoverImage = null, Release = DateTime.Now, Price = 0, AuthorId = 2},
                new Book { Id = 2, Title = "title 2", Description = "A star is born.", CoverImage = null, Release = DateTime.Now, Price = 0, AuthorId = 2},
                new Book { Id = 3, Title = "title 3", Description = "", CoverImage = null, Release = DateTime.Now, Price = 0, AuthorId = 2},
                new Book { Id = 4, Title = "title 4", Description = "", CoverImage = null, Release = DateTime.Now, Price = 0, AuthorId = 2},
                new Book { Id = 5, Title = "title 5", Description = "", CoverImage = null, Release = DateTime.Now, Price = 0, AuthorId = 3},
                new Book { Id = 6, Title = "title 6", Description = "", CoverImage = null, Release = DateTime.Now.AddMonths(-25), Price = 0, AuthorId = 3},
                
                
                new Book { Id = 7, Title = "title 7", Description = "", CoverImage = null, Release = DateTime.Now.AddMonths(-25), Price = 0, AuthorId = 5},
                new Book { Id = 8, Title = "title 8", Description = "", CoverImage = null, Release = DateTime.Now.AddMonths(-26), Price = 0, AuthorId = 5},
                new Book { Id = 9, Title = "title 9", Description = "", CoverImage = null, Release = DateTime.Now.AddMonths(-24), Price = 0, AuthorId = 5},
            };
            return books;
        }
        
        #endregion
        
        
    }
    
    
    public class BookServicesTests : BaseServicesTests
    {
        [Fact]
        public async void GetAllBooksTest()
        {
            EmptyDatabase();
            SeedDatabase();
            
            var books = await _service.GetAllBooks();
            
            Assert.NotEmpty(books);
            Assert.Equal(GetSeedingBooks().Count(),books.Count());
            
        }


        [Fact]
        public void CheckForAuthorPseudonym_drokkattta_Test()
        {
            var author1 = new Author
            {
                Id = 3, FirstName = "Klaas", LastName = "De Typemachine", Pseudonym = "drokkattta",
            };
            
            var book1 = new Book
            {
                Id = 1, Title = "title 1", Description = "", CoverImage = null, Release = DateTime.Now.AddMonths(-25),
                Price = 0, AuthorId = 3, Author = author1
            };
            var book2 = new Book
            {
                Id = 2, Title = "title 2", Description = "", CoverImage = null, Release = DateTime.Now.AddMonths(-23),
                Price = 0, AuthorId = 3, Author = author1
            };
            var book3 = new Book
            {
                Id = 2, Title = "title 3", Release = DateTime.Now.AddMonths(-24),
                AuthorId = 3, Author = author1
            };
            
            var result = BookService.CheckForAuthorPseudonym(new List<Book>{book1,book2,book3});
            
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count());
            var dates = result.Where(x => x.Author.Pseudonym == "drokkattta").Select(x => x.Release);
            var all = dates.All(x => x > DateTime.Today.AddMonths(-24));
            Assert.True(all);
        }

        [Fact]
        public async void GetAllBooks_WithAuthor_Drokkatta_Grakchawwaa_test()
        {
            EmptyDatabase();
            SeedDatabase();
            
            var books = await _service.GetAllBooksWithAuthor();
            
            Assert.NotEmpty(books);
            Assert.Equal(6, books.Count());
            var bookReleaseDates = books.Where(x => x.Author.Pseudonym is "drokkattta" or "grakchawwaa")
                .Select(x => x.Release);
            
        }
    }
}