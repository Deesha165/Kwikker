using AutoMapper;
using Entities.DomainModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs;
using System.Dynamic;

namespace Kwikker_Backend
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Source to Destination mappings
            CreateMap<UserForRegistrationDto, User>();


            

            CreateMap<User, GeneralUserDTO>();

            CreateMap<UserForUpdateDTO, User>()
      .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<TweetForCreationDTO, Tweet>();

            CreateMap<Tweet, TweetDTO>()
    .ForCtorParam("id", opt => opt.MapFrom(src => src.ID))
    .ForCtorParam("content", opt => opt.MapFrom(src => src.Content ?? string.Empty))
    .ForCtorParam("userId", opt => opt.MapFrom(src => src.UserID))
    .ForCtorParam("createdAt", opt => opt.MapFrom(src => src.CreatedAt))
    .ForCtorParam("userName", opt => opt.MapFrom(src => src.User.UserName))
    .ForCtorParam("likesNumber", opt => opt.MapFrom(src => src.LikesNumber))
    .ForCtorParam("retweetsNumber", opt => opt.MapFrom(src => src.RetweetsNumber))
    .ForCtorParam("bookmarksNumber", opt => opt.MapFrom(src => src.BookmarksNumber))
    .ForCtorParam("email", opt => opt.MapFrom(src => src.User.Email))
    .ForCtorParam("profilePicture", opt => opt.MapFrom(src => src.User.ProfilePicture ?? string.Empty))
    .ForCtorParam("mediaUrl", opt => opt.MapFrom(src => src.MediaURL ?? string.Empty));

            CreateMap<TweetForUpdateDTO, Tweet>();
            CreateMap<Trend, TrendDTO>();
            CreateMap<Notification, NotificationDTO>();
        
        }
    }
}
