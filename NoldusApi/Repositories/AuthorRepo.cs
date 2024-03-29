using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NoldusApi.Models;

namespace NoldusApi.DataAccess
{
    public class AuthorRepo : IAuthorRepo
    {
        private readonly ApiContext _context;

        public AuthorRepo(ApiContext context)
        {
            _context = context;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            var authors = _context.Authors.ToList();

            return authors;
        }

        public Author GetAuthorById(int id)
        {
            var author = _context.Authors.FirstOrDefault(x => x.Id == id);
            return author;
        }

        public Author GetAuthorByIdWithFirstRelation(int id)
        {
            var author = _context.Authors
                .Include(x => x.Books)
                .FirstOrDefault(x => x.Id == id);
            return author;
        }

        public void CreateAuthor(Author author)
        {
            _context.Authors.Add(author);
        }

        public void UpdateAuthor(Author input)
        {
            throw new NotImplementedException();
        }

        public void DeleteAuthor(Author author)
        {
            _context.Authors.Remove(author);
        }
    }
}