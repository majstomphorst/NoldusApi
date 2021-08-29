using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NoldusApi.Models
{
    
    [Table("Authors")]
    public class Author
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        public string Pseudonym { get; set; }
        
        
        public ICollection<Book> Books { get; set; } 
    }
}