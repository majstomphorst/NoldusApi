using System.Collections.Generic;
using NoldusApi.Models;

namespace NoldusApi.Dtos
{
    public class AuthorReadDto
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string FullName => FirstName + " " + LastName;
        
        public string Pseudonym { get; set; }
        
        public ICollection<Book> Books { get; set; }
    }
}