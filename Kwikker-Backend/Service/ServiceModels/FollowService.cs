﻿using AutoMapper;
using Contracts;
using Service.Contracts.Contracts;
using Entities.ExceptionModels;
using Shared.DTOs;
using Shared.RequestFeatures;
using Entities.Models;
using System.Dynamic;
namespace Service.ServiceModels
{
    internal sealed class FollowService : IFollowService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly INotificationService _notification;

        private readonly IUserService _userService;
        public FollowService(IRepositoryManager repository, ILoggerManager
        logger, IMapper mapper, INotificationService notification, IUserService userService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _notification = notification;
            _userService = userService; 
        }

        public async Task CreateFollow(int followerId, int followeeId, bool trackChanges)
        {
            if (followeeId == followerId) throw new Entities.ExceptionModels.ArgumentException("FollowerId and FolloweeId must be distinct");
            var follower =await _repository.UserRepository.GetUser(followerId, trackChanges);
            if (follower is null) throw new NotFoundException(followerId, "User");

            var followee =await  _repository.UserRepository.GetUser(followeeId, trackChanges);
            if (followee is null) throw new NotFoundException(followeeId, "User");

             
            
             _repository.FollowRepository.CreateFollow(followerId, followeeId);

            await _notification.CreateNotification(followerId, "Follow", followee.ID, $"{follower.Username} has  Followed you");

            await _repository.SaveAsync();
        }

        public async Task DeleteFollow(int followerId, int followeeId, bool trackChanges)
        {
            var follow=await _repository.FollowRepository.GetFollow(followerId, followeeId, trackChanges);
            if (follow is null) throw new CompositeKeyNotFoundException(followerId,followeeId,"Follows","User","User");
            _repository.FollowRepository.DeleteFollow(follow);
           await _repository.SaveAsync();
        }

        public async Task<(IEnumerable<FollowDTO> followees, MetaData metaData)> GetUserFollowees(int followerID, FollowingParameters followingParameters, bool trackChanges)
        {
            var followeesWithMetaData = await _repository.FollowRepository.GetUserFollowees(followerID, followingParameters, trackChanges);

            if (!followeesWithMetaData.Any())
            {
                return (Enumerable.Empty<FollowDTO>(), new MetaData());
            }

            var followeesDTOs = _mapper.Map<IEnumerable<FollowDTO>>(followeesWithMetaData);

            return (followees: followeesDTOs, metaData: followeesWithMetaData.MetaData);
        }

        public async Task<(IEnumerable<FollowDTO> followers, MetaData metaData)> GetUserFollowers(int followeeID, FollowingParameters followingParameters, bool trackChanges)
        {
            var followersWithMetaData=await _repository.FollowRepository.GetUserFollowers(followeeID, followingParameters, trackChanges);

            if (!followersWithMetaData.Any())
            {
                return (Enumerable.Empty<FollowDTO>(), new MetaData());
            }

            var followersDTOs=_mapper.Map<IEnumerable<FollowDTO>>(followersWithMetaData);
           
            return (followers:followersDTOs,metaData:followersWithMetaData.MetaData);
        }
        
        public async Task<List<GeneralUserDTO>>GetSuggestedToFollow(int UserId)
        {
            IEnumerable<int> availableUserIds = new List<int>();
            List<GeneralUserDTO> availableUsers = new List<GeneralUserDTO>();
            
            availableUserIds = await GetRandomUsers(UserId);

            foreach(var userid in availableUserIds)
            {
                var user =await _userService.GetUser(userid,trackChanges:false);
                var GeneralUser = _mapper.Map<GeneralUserDTO>(user);
                availableUsers.Add(GeneralUser);
            }
            return availableUsers;
        }
        public async Task<IEnumerable<int>>GetRandomUsers(int UserId)
        {
            FollowingParameters followingParameters = new FollowingParameters();
            var userFollowees = GetUserFollowees(UserId, followingParameters, trackChanges: false).Result.followees.Select(x => x.FolloweeID).ToHashSet();
            int userCounts = await _userService.GetUserCount();

            // List to store the selected available numbers
            List<int> availableUsers = new List<int>();


            // Continue randomly picking numbers from the range until we have 3 available numbers
            Random random = new Random();
            int maxWanted = Math.Min(3,userCounts-userFollowees.Count-1);
            while (maxWanted>0 && availableUsers.Count() < maxWanted)
            {
                // Generate a random number in the range
                int randomNumber = random.Next(1, userCounts + 1); // Include rangeEnd

                // Check if the number is not in the exclusion list
                if (randomNumber != UserId && !userFollowees.Contains(randomNumber) && !availableUsers.Contains(randomNumber))
                {
                    availableUsers.Add(randomNumber);
                }
            }
            return availableUsers;
        }
    }
}
