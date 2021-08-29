using System;
using System.ComponentModel.DataAnnotations;
using NoldusApi.Models;

namespace NoldusApi.Dtos.BookDtos
{
    public class BookReadDto
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
        public Author Author { get; set; }
    }
}