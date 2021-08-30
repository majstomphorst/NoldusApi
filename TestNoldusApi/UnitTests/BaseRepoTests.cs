using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NoldusApi.DataAccess;
using NoldusApi.Models;

namespace TestNoldusApi.UnitTests
{
    public class BaseRepoTests
    {
        protected readonly ApiContext _context;
        protected readonly IAuthorRepo _authorRepo;
        protected readonly BookRepo _bookRepo;
        

        public BaseRepoTests()
        {
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "memory")
                .Options;
            
            // Insert seed data into the database using one instance of the context
            var context = new ApiContext(options);
            
            context.Book.RemoveRange(context.Book);
            context.SaveChanges();
            
            context.Authors.RemoveRange(context.Authors);
            context.SaveChanges();
            
            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "Henk", LastName = "De Schrijver", Pseudonym = "H.S", Books = null},
                new Author { Id = 2, FirstName = "Ingrid", LastName = "De Pen", Pseudonym = "I.P", Books = null},
                new Author { Id = 3, FirstName = "Klaas", LastName = "De Typemachine", Pseudonym = "drokkattta", Books = null},
            };
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "There is a star", Description = "A star is born.", CoverImage = null, Release = DateTime.Now, Price = 0, AuthorId = 2},
                new Book { Id = 2, Title = "title 1", Description = "A star is born.", CoverImage = null, Release = DateTime.Now, Price = 0, AuthorId = 2},
                new Book { Id = 3, Title = "title 2", Description = "", CoverImage = null, Release = DateTime.Now, Price = 0, AuthorId = 2},
                new Book { Id = 4, Title = "title 3", Description = "", CoverImage = null, Release = DateTime.Now, Price = 0, AuthorId = 2},
                new Book { Id = 5, Title = "title 4", Description = "", CoverImage = null, Release = DateTime.Now, Price = 0, AuthorId = 3},
                new Book { Id = 6, Title = "title 5", Description = "", CoverImage = null, Release = DateTime.Now.AddMonths(-25), Price = 0, AuthorId = 3},
            };
            
            foreach (var author in authors)
            {
                context.Authors.Add(author);
            }
            foreach (var book in books)
            {
                context.Book.Add(book);
            }
            context.SaveChanges();
            
            _context = context;
            _authorRepo = new AuthorRepo(context);
            _bookRepo = new BookRepo(context);
        }
    }
}