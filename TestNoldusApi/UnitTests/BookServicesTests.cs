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
    [Collection("Sequential")]
    public class BookServicesTests : BaseServicesTests
    {
        [Fact]
        public async void GetAllBooksTest()
        {
            EmptyDatabase();
            SeedDatabase();
            
            var books = await _bookService.GetAllBooks();
            
            Assert.NotEmpty(books);
            Assert.Equal(5,books.Count());
            
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
                Price = 0, AuthorId = 3
            };
            var book2 = new Book
            {
                Id = 2, Title = "title 2", Description = "", CoverImage = null, Release = DateTime.Now.AddMonths(-23),
                Price = 0, AuthorId = 3
            };
            var book3 = new Book
            {
                Id = 2, Title = "title 3", Release = DateTime.Now.AddMonths(-24),
                AuthorId = 3
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
                Price = 0, AuthorId = 3
            };
            var book2 = new Book
            {
                Id = 2, Title = "title 2", Description = "", CoverImage = null, Release = DateTime.Now.AddMonths(-23),
                Price = 0, AuthorId = 3
            };
            var book3 = new Book
            {
                Id = 2, Title = "title 2", Description = "", CoverImage = null, Release = DateTime.Now.AddMonths(-23),
                Price = 0, AuthorId = 5
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
            Assert.Equal(5, books.Count());

        }
    }
}