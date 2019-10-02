using System.Linq;
using AutoMapper;
using BookReview.API.Dtos;
using BookReview.API.Models;

namespace DatingApp.API.Helpers
{
    ///<summary>
    ///Class that configures AutoMapper
    ///</summary>
    public class AutoMapperProfiles:Profile
    {
        ///<summary>
        ///Class Constructor, object-object maps can are configured here
        ///</summary>
        public AutoMapperProfiles()
        {
            CreateMap<Book, BookSummarizedViewDto>();
            CreateMap<Book, BookDetailedViewDto>();
            CreateMap<BookForCreationDto, Book>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserToReturnDto>();
            CreateMap<Chapter,ChapterDetailedViewDto>();
            CreateMap<Picture, PictureDto>();
            CreateMap<ChapterForCreationDto, Chapter>();
            CreateMap<Chapter, ChapterDetailedViewDto>();
        }
    }
}