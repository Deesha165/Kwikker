using Entities.Models;
using Shared.RequestFeatures;

namespace Repository.RepositoryModels
{
    public interface IBookmarkRepository
    {
        void CreateBookmark(int userId, int tweetId);
        void DeleteBookmark(Bookmark bookmark);
        Task<Bookmark> GetBookmark(int userId, int tweetId, bool trackChanges);

        Task<List<Tweet>> GetBookmarksByUser(int UserId,bool trackChanges);


    }
}