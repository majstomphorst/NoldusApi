using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NoldusApi;
using NoldusApi.DataAccess;
using NoldusApi.Models;


namespace TestNoldusApi
{
    public class BaseIntergrationTest
    {
        protected readonly HttpClient _client;
        protected readonly IAuthorRepo _authorRepo;
        protected readonly IBookRepo _bookRepo;
        public BaseIntergrationTest()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "testing");
            var appFactory = new WebApplicationFactory<Startup>();
            var authorRepo = appFactory.Services.GetService<IAuthorRepo>();
            var bookRepo = appFactory.Services.GetService<IBookRepo>();
            var context = appFactory.Services.GetService<ApiContext>();
            if (authorRepo == null || bookRepo == null || context == null)
            {
                throw new NotImplementedException();
            }
            context.Book.RemoveRange(context.Book);
            context.SaveChanges();
            
            context.Authors.RemoveRange(context.Authors);
            context.SaveChanges();
            
            _authorRepo = authorRepo!;
            _bookRepo = bookRepo!;
            _client = appFactory.CreateClient();
            SeedDatabase();
        }

        protected void SeedDatabase()
        {
            var authors = new List<Author>
            {
                new Author { Id = 1,  FirstName = "Henk", LastName = "De Schrijver", Pseudonym = "H.S", Books = null},
                new Author { Id = 2, FirstName = "Ingrid", LastName = "De Pen", Pseudonym = "I.P", Books = null},
                new Author { Id = 3, FirstName = "Klaas", LastName = "De Typemachine", Pseudonym = "Computer", Books = null},
                new Author { Id = 4, FirstName = "Luke", LastName = "Skywalker", Pseudonym = "Drokkatta", Books = null},
                new Author { Id = 5, FirstName = "legolas", LastName = "green leaf", Pseudonym = "Grakchawwaa", Books = null},
            };
            var books = new List<Book>()
            {
                new Book
                { Id = 0, Title = "book 1", Description = "desc 1",  CoverImage = null, Release = DateTime.Now, Price = 9.3m, AuthorId = 1}
            };
            foreach (var author in authors)
            {
                _authorRepo.CreateAuthor(author);
            }
            _authorRepo.SaveChanges();

            foreach (var book in books)
            {
                _bookRepo.CreateBook(book);
            }
            _bookRepo.SaveChanges();

        }
        
    }
}