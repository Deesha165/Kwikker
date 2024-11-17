using AutoMapper;
using Entities.DomainModels;
using Entities.Models;
using Shared.DTOs;

namespace Kwikker_Backend
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Source to Destination mappings
            CreateMap<UserForRegistrationDto, User>();



            CreateMap<User, GeneralUserDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<TweetForCreationDTO, Tweet>();

            // Mapping from Tweet to TweetDTO
            CreateMap<Tweet, TweetDTO>()
                .ForCtorParam("profilePicture", opt => opt.MapFrom(src => src.User.ProfilePicture )) // Default to empty string if null
                .ForCtorParam("userName", opt => opt.MapFrom(src => src.User.UserName)); // Maps User's Username to TweetDTO's userName

            CreateMap<TweetForUpdateDTO, Tweet>();
            CreateMap<Trend, TrendDTO>();
            CreateMap<Notification, NotificationDTO>();
            CreateMap<UserDTO, GeneralUserDTO>();
        }
    }
}
