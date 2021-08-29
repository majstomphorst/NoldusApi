using NoldusApi.Dtos;
using NoldusApi.Dtos.AuthorDtos;
using NoldusApi.Models;

namespace NoldusApi.Profile
{
    public class AuthorProfile : AutoMapper.Profile
    {
        public AuthorProfile()
        {
            // internal -> external
            CreateMap<Author, AuthorReadDto>();
            // external -> internal
            CreateMap<AuthorWriteDto, Author>();
            // external -> internal
            CreateMap<AuthorUpdateDto, Author>();
            // internal -> external
            CreateMap<Author, AuthorUpdateDto>();
        }
    }
}