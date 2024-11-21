using Service.Contracts.Contracts;
using Contracts;
using AutoMapper;
using Shared.DTOs;
using Shared.RequestFeatures;
using Entities.ExceptionModels;
using StackExchange.Redis;
using System.Text.Json;
namespace Service.ServiceModels
{
    public class BookmarkService:IBookmarkService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDatabase _redisCache;

        private string CacheKey = "Bookmarks";  // Cache key
        private readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(20);  // Cache expiration time
        public BookmarkService(IRepositoryManager repository, ILoggerManager
        logger,IMapper mapper,IConnectionMultiplexer redisConnection)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper; 
            _redisCache=redisConnection.GetDatabase();
        }

        public async Task CreateBookmark(int userId, int tweetId, bool trackChanges)
        {
            
            var user = await _repository.UserRepository.GetUser(userId, trackChanges);
            if (user is null) throw new NotFoundException(userId, "User");

            var tweet = await _repository.TweetRepository.GetTweet(tweetId, trackChanges);
            if (tweet is null) throw new NotFoundException(tweetId, "Tweets");



            _repository.BookmarkRepository.CreateBookmark(userId, tweetId);
            await _repository.SaveAsync();

        }

        public async Task DeleteBookmark(int userId, int tweetId, bool trackChanges)
        {
            var bookmark = await _repository.BookmarkRepository.GetBookmark(userId, tweetId, trackChanges);
            if (bookmark is null) throw new CompositeKeyNotFoundException(userId, tweetId, "Follows", "User", "User");
            _repository.BookmarkRepository.DeleteBookmark(bookmark);
            await _repository.SaveAsync();
        }

        public async Task <IEnumerable<int>> GetUserBookmarks (int userId,bool trackChanges)
        {
            CacheKey +=$"{userId}";

            var cachedBookmarks = await _redisCache.StringGetAsync(CacheKey);

            if(!cachedBookmarks.HasValue)
            {
                var bookmarks = await _repository.BookmarkRepository.GetBookmarksByUser(userId, trackChanges);
                if (!bookmarks.Any())
                {
                    return Enumerable.Empty<int>();
                }

               

                var serializedBookmarks = JsonSerializer.Serialize(bookmarks);

                await _redisCache.StringSetAsync(CacheKey, serializedBookmarks, CacheExpiration);
                return bookmarks;
            }

            var likedTweets = JsonSerializer.Deserialize<List<int>>(cachedBookmarks!);

            return likedTweets!;
        }
    }
}
