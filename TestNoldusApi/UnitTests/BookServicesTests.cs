using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoldusApi.Models;
using NoldusApi.Services;
using Xunit;

namespace TestNoldusApi.UnitTests
{
    public class BookServicesTests : BaseServicesTests
    {
        [Fact]
        public async void GetAllBooksTest()
        {
            EmptyDatabase();
            SeedDatabase();
            
            var books = await _bookService.GetAllBooks();
            
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
        public async void AllBooksHaveAuthor_True_Test()
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
            
            Assert.True(_bookService.AllBooksHaveAuthor(new List<Book>(){book1,book2,book3}));
        }
        
        [Fact]
        public async void AllBooksHaveAuthor_False_Test()
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
                Id = 2, Title = "title 2", Description = "", CoverImage = null, Release = DateTime.Now.AddMonths(-23),
                Price = 0, AuthorId = 5, Author = author1
            };
            var book4 = new Book
            {
                Id = 2, Title = "title 3", Release = DateTime.Now.AddMonths(-24)
            };
            Assert.False(_bookService.AllBooksHaveAuthor(new List<Book>(){book1,book2,book3,book4}));
        }

        [Fact]
        public async void GetAllBooks_WithAuthor_Drokkatta_Grakchawwaa_test()
        {
            EmptyDatabase();
            SeedDatabase();
            
            var books = await _bookService.GetAllBooksWithAuthor();
            
            Assert.NotEmpty(books);
            Assert.Equal(6, books.Count());
            var bookReleaseDates = books.Where(x => x.Author.Pseudonym is "drokkattta" or "grakchawwaa")
                .Select(x => x.Release);
            
        }
    }
}