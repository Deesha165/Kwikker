using AutoMapper;
using Contracts;

using Microsoft.IdentityModel.Tokens;
using Service.Contracts.Contracts;
using Shared.DTOs;
using StackExchange.Redis;  // Include Redis package

using System.Text.Json;  // Use for serialization/deserialization

namespace Service.ServiceModels
{
    public class TrendService : ITrendService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDatabase _redisCache;  // Use Redis cache

        private const string CacheKey = "TopTrends";  // Cache key
        private readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(10);  // Cache expiration time

        public TrendService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IConnectionMultiplexer redisConnection)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _redisCache = redisConnection.GetDatabase();  // Get the Redis database
        }

        public IEnumerable<TrendDTO> GetTrends()
        {
            // Get trends from Redis cache
            var cachedTrends = _redisCache.StringGet(CacheKey);
            if (!cachedTrends.HasValue)
                return Enumerable.Empty<TrendDTO>();

            // Deserialize the cached data
            var trends = JsonSerializer.Deserialize<List<TrendDTO>>(cachedTrends!);
            return trends ?? Enumerable.Empty<TrendDTO>();
        }

        public async Task<IEnumerable<TrendDTO>> GetTopTrends()
        {
            // Try to get trends from the Redis cache
            var cachedTrends = await _redisCache.StringGetAsync(CacheKey);
            if (!cachedTrends.HasValue)
            {
                // If cache is empty, fetch from the repository
                var trends = await _repository.TrendRepository.GetTopTrends();

                if (trends == null)
                {
                    _logger.LogWarn($"{nameof(GetTopTrends)}: No trends found in the database.");
                    return Enumerable.Empty<TrendDTO>();
                }

                // Map domain trends to DTOs
                List<TrendDTO> trendDTOs = _mapper.Map<List<TrendDTO>>(trends);

                if (trendDTOs == null || trendDTOs.Count == 0)
                {
                    _logger.LogWarn($"{nameof(GetTopTrends)}: Mapping resulted in null or empty list.");
                    return Enumerable.Empty<TrendDTO>();
                }

                // Serialize the trend DTOs to JSON and store in Redis with expiration
                var serializedTrends = JsonSerializer.Serialize(trendDTOs);
                await _redisCache.StringSetAsync(CacheKey, serializedTrends, CacheExpiration);

                // Return the fetched trends
                return trendDTOs;
            }

            // Deserialize the cached data
            var cachedTrendDTOs = JsonSerializer.Deserialize<List<TrendDTO>>(cachedTrends!);
            return cachedTrendDTOs ?? Enumerable.Empty<TrendDTO>();
        }

        public async Task<IEnumerable<TweetDTO>> GetTweetsByTrend(string hashtag)
        {
            var tweets = await _repository.TrendRepository.GetTweetsByTrend(hashtag);

            if (tweets.IsNullOrEmpty())
            {
                _logger.LogWarn($"{nameof(GetTweetsByTrend)}: No tweets found for hashtag '{hashtag}'.");
                return Enumerable.Empty<TweetDTO>();
            }

            var tweetDTOs = _mapper.Map<IEnumerable<TweetDTO>>(tweets);
            if (tweetDTOs == null || !tweetDTOs.Any())
            {
                _logger.LogWarn($"{nameof(GetTweetsByTrend)}: Mapping tweets resulted in null or empty list.");
                return Enumerable.Empty<TweetDTO>();
            }

            return tweetDTOs;
        }
    }
}
