using NoldusApi.Dtos.BookDtos;
using NoldusApi.Models;

namespace NoldusApi.Profile
{
    public class BookProfile : AutoMapper.Profile
    {

        public BookProfile()
        {
            // internal -> external
            CreateMap<Book, BookReadDto>();
            // external -> internal
            CreateMap<BookWriteDto, Book>();
            // // external -> internal
            // CreateMap<AuthorUpdateDto, Book>();
            // // internal -> external
            // CreateMap<Author, AuthorUpdateDto>();
        }
        
    }
}