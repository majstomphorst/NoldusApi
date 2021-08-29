using System.ComponentModel.DataAnnotations;

namespace NoldusApi.Dtos.AuthorDtos
{
    public class AuthorWriteDto
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        public string Pseudonym { get; set; }
    }
}