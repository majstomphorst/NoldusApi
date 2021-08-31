using System.Linq;
using NoldusApi.Models;
using Xunit;

namespace TestNoldusApi.UnitTests
{
    public class AuthorRepoTests : BaseRepoTests
    {
        [Fact]
        public void GetAllAuthorsTest()
        {
            var authors = _authorRepo.GetAllAuthors();

            Assert.Equal(3, authors.Count());
        }

        [Fact]
        public void GetAuthorByIdTest()
        {
            var author = _authorRepo.GetAuthorById(1);
            
            Assert.NotNull(author);
            Assert.Equal("Henk", author.FirstName);
        }

        [Fact]
        public void GetAuthorByIdWithRelation_valid()
        {
            var author = _authorRepo.GetAuthorByIdWithFirstRelation(2);

            Assert.NotNull(author);
            Assert.NotNull(author.Books);
            Assert.Single(author.Books);
        }

        [Fact]
        public void GetAuthorByIdWithRelation_invalid()
        {
            var author = _authorRepo.GetAuthorByIdWithFirstRelation(1);
            
            Assert.NotNull(author);
            Assert.Empty(author.Books);
        }

        [Fact]
        public void CreateAuthor()
        {
            var newAuthor = new Author {FirstName = "Piet", LastName = "the last", Pseudonym = "", Books = null};
            
            _authorRepo.CreateAuthor(newAuthor);
            _authorRepo.SaveChanges();

            var piet = _context.Authors.Where(x => x.FirstName == "Piet");

            Assert.Single(piet);
            Assert.Equal("the last", piet.First().LastName);
        }

        [Fact]
        public void CreateAuthor_fail_DarkFather()
        {
            var newAuthor = new Author {FirstName = "Dark", LastName = "Father", Pseudonym = "", Books = null};
            
            _authorRepo.CreateAuthor(newAuthor);
            _authorRepo.SaveChanges();

            var vader = _context.Authors.Where(x => x.FirstName == "Dark" && x.LastName == "Father");

            Assert.Empty(vader);
        }

        [Fact]
        public void DeleteAuthorTest()
        {
            var firstname = "aabbcc";
            var lastName = "ccbbaa";
            var newAuthor = new Author {FirstName = firstname, LastName = lastName, Pseudonym = "", Books = null};

            _context.Authors.Add(newAuthor);
            _context.SaveChanges();

            var item = _context.Authors.Where(x => x.FirstName == firstname && x.LastName == lastName);
            Assert.Single(item);

            _authorRepo.DeleteAuthor(item.First());
            _authorRepo.SaveChanges();
            
            var noItem = _context.Authors.Where(x => x.FirstName == firstname && x.LastName == lastName);
            Assert.Empty(noItem);
        }

        [Fact]
        public void DeleteAuthor_WithBooks_Test()
        {
            //todo
        }
        
        // [Fact]
        // public void 

    }
}