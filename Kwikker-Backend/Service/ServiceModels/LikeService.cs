using AutoMapper;
using Contracts;
using Service.Contracts.Contracts;

using Shared.DTOs;
using Entities.ExceptionModels;
using Microsoft.AspNetCore.SignalR;
using Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Text.Json;
namespace Service.ServiceModels
{
    internal sealed class LikeService : ILikeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly StackExchange.Redis.IDatabase _redisCache;
        private readonly INotificationService _notification;
        private readonly IHubContext<NotificationHub> _hubContext;

        private string CacheKey = "Likes";  // Cache key
        private readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);  // Cache expiration time
        public LikeService(IRepositoryManager repository, ILoggerManager
        logger, IMapper mapper, IConnectionMultiplexer redisConnection, IHubContext<NotificationHub> hubContext, INotificationService notification)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _redisCache = redisConnection.GetDatabase();
            _notification = notification;
            _hubContext = hubContext;   
        }

        public async Task CreateLike(int userId, int tweetid,bool trackChanges)
        {
            var user =await _repository.UserRepository.GetUser(userId,trackChanges);
            if (user is null) throw new ForeignKeyNotFoundException(userId, "Likes", "User");

            var tweet =await _repository.TweetRepository.GetTweet(tweetid, trackChanges);
            if (tweet is null) throw new ForeignKeyNotFoundException(tweetid, "Likes", "Tweet");

           _repository.LikeRepository.CreateLike(userId, tweetid);


            // Notify user
            string notificationMessage = $"{user.UserName} has like your tweet";
            await _notification.CreateNotification(userId, "Liked", tweet.UserID);

            // Send real-time notification via SignalR
          
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationMessage);

            await _repository.SaveAsync();
        }

        public async Task DeleteLike(int userId, int tweetid, bool trackChanges)
        {
            var like =await _repository.LikeRepository.GetLike(userId, tweetid, trackChanges: false);
            if (like is null) throw new CompositeKeyNotFoundException(userId, tweetid, "Likes", "User", "Tweet");

            _repository.LikeRepository.DeleteLike(like);
            await _repository.SaveAsync();
        }


        public async Task<IEnumerable<int>> GetUserLikedTweets(int userId, bool trackChanges)
        {
            CacheKey += $"{userId}";
            var cachedTweets = await _redisCache.StringGetAsync(CacheKey);
            if(!cachedTweets.HasValue)
            {
                var likedTweetsWithMetaData = await _repository.LikeRepository.GetLikedTweetsByUser(userId, trackChanges);


                if (!likedTweetsWithMetaData.Any())
                {
                    return Enumerable.Empty<int>();
                }

                var likedTweetsDTOs = _mapper.Map<IEnumerable<int>>(likedTweetsWithMetaData);

                var serializedLikedTweets = JsonSerializer.Serialize(likedTweetsDTOs);

                await _redisCache.StringSetAsync(CacheKey, serializedLikedTweets, CacheExpiration);

                return likedTweetsDTOs;
            }

            var likedTweets = JsonSerializer.Deserialize<List<int>>(cachedTweets!);
            return likedTweets!;
           
        }

    }
}
