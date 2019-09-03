using System.Linq;
using AutoMapper;
using BookReview.API.Dtos;
using BookReview.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Book, BookForUserListDto>();
            CreateMap<Book, BookForDetailedDto>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserToReturnDto>();
        }
    }
}