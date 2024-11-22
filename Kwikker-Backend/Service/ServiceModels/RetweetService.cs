using AutoMapper;
using Contracts;
using Entities.ExceptionModels;
using Entities.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;
using Service.Contracts.Contracts;
using Service.DataShaping;
using Shared.DTOs;
using Shared.RequestFeatures;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.ServiceModels
{
    internal sealed class RetweetService : IRetweetService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly INotificationService _notification;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly StackExchange.Redis.IDatabase _redisCache;

        private string CacheKey = "Retweets";  // Cache key
        private readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(20);  // Cache expiration time
        public RetweetService(IRepositoryManager repository, ILoggerManager
        logger, IMapper mapper, IConnectionMultiplexer redisConnection, IHubContext<NotificationHub> hubContext, INotificationService notification)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _notification = notification;
            _redisCache = redisConnection.GetDatabase();
            _hubContext = hubContext;
        }
        public async Task CreateRetweet(int userId, int tweetid, bool trackChanges)
        {
            var user =await _repository.UserRepository.GetUser(userId, trackChanges);
            if (user is null) throw new ForeignKeyNotFoundException(userId, "Likes", "User");

            var tweet = await _repository.TweetRepository.GetTweet(tweetid, trackChanges);
            if (tweet is null) throw new ForeignKeyNotFoundException(tweetid, "Likes", "Tweet");

            _repository.RetweetRepository.CreateRetweet(userId, tweetid);

            //notify user
            string notificationMessage = $"{user.UserName} has retweeted your tweet";
            await _notification.CreateNotification(userId, "Retweet", tweet.UserID);

            if (userId != tweet.UserID)
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationMessage);


            await _repository.SaveAsync();

            var cacheKey = CacheKey + userId;
            await _redisCache.KeyDeleteAsync(cacheKey);
        }

        public async Task DeleteRetweet(int userId, int tweetid, bool trackChanges)
        {
            var retweet =await _repository.RetweetRepository.GetRetweet(userId, tweetid, trackChanges: false);
            if (retweet is null) throw new CompositeKeyNotFoundException(userId, tweetid, "Likes", "User", "Tweet");

            _repository.RetweetRepository.DeleteRetweet(retweet);
            await _repository.SaveAsync();

            var cacheKey = CacheKey + userId;
            await _redisCache.KeyDeleteAsync(cacheKey);
        }

        public async Task<IEnumerable<TweetDTO>> GetUserRetweets(int userId,bool trackChanges)
        {
            CacheKey += $"{userId}";

            var cachedRetweets = await _redisCache.StringGetAsync(CacheKey);

            if (!cachedRetweets.HasValue)
            {
                var Retweets = await _repository.RetweetRepository.GetRetweetsByUser(userId,  trackChanges);
                if (!Retweets.Any())
                {
                    return Enumerable.Empty<TweetDTO>();
                }

                var RetweetDTOs = _mapper.Map<IEnumerable<TweetDTO>>(Retweets);

                var serializedRetweets = JsonSerializer.Serialize(RetweetDTOs);

                await _redisCache.StringSetAsync(CacheKey, serializedRetweets, CacheExpiration);
                return RetweetDTOs;
            }

            var retweets = JsonSerializer.Deserialize<List<TweetDTO>>(cachedRetweets!);

            return retweets!;
        }
    }
}
