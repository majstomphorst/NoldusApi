using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NoldusApi.Models
{
    [Table("Books")]
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string CoverImage { get; set; }
        
        [Required]
        public DateTime Release { get; set; }
        
        [Required]
        [DataType("decimal(10,2)")]
        public decimal Price { get; set; }
        
        public int AuthorId { get; set; }
    }
}