using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using NoldusApi.Models;

namespace NoldusApi.Dtos.BookDtos
{
    public class BookWriteDto
    {
        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        // public IFormFile CoverImage { get; set; }
        
        [Required]
        public DateTime Release { get; set; }
        
        [Required]
        [DataType("decimal(10,2)")]
        public decimal Price { get; set; }
        
        public int AuthorId { get; set; }
    }
}