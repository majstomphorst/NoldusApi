using System.Collections.Generic;
using NoldusApi.Models;
using Xunit;

namespace TestNoldusApi.UnitTests
{
    public class AuthorServiceTest : BaseServicesTests
    {

        [Fact]
        public void validAuthors_bad_Test()
        {
            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "Dark", LastName = "Father", Pseudonym = "H.S", Books = null},
                new Author { Id = 2, FirstName = "Ingrid", LastName = "De Pen", Pseudonym = "I.P", Books = null},
            };

            var check = _authorService.validAuthors(authors);
            
            Assert.False(check);
        }

        [Fact]
        public void validAuthoers_good_Test()
        {
            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "pet", LastName = "piet", Pseudonym = "H.S", Books = null},
                new Author { Id = 2, FirstName = "Ingrid", LastName = "De Pen", Pseudonym = "I.P", Books = null},
            };

            var check = _authorService.validAuthors(authors);
            
            Assert.True(check);
        }
    }
}