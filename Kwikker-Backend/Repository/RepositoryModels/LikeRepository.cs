using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts.Contracts;
using Repository.Extensions;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.RepositoryModels
{
    public class LikeRepository : RepositoryBase<Like>, ILikeRepository
    {
        public LikeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public  void CreateLike(int userId, int tweetId)
        {
            Like like = new Like()
            {
                TweetId = tweetId,
                UserId = userId,
                LikedAt=DateTime.Now
            };
            var tweet=RepositoryContext.Set<Tweet>().FirstOrDefault(x=>x.ID.Equals(tweetId));
            if (tweet != null) tweet.LikesNumber++;

            Create(like);
            
        }

        public async Task<Like> GetLike(int userId,int tweetId,bool trackChanges)
        {
          var result=  await FindByCondition(x => x.UserId.Equals(userId) && x.TweetId.Equals(tweetId), trackChanges).SingleOrDefaultAsync();
            return result!;
         }
        public void DeleteLike(Like like) {

            var tweet = RepositoryContext.Set<Tweet>().FirstOrDefault(x => x.ID.Equals(like.TweetId));
            if (tweet != null) tweet.LikesNumber--;
            Delete(like);
        }
    
        public async Task<List<Tweet>> GetLikedTweetsByUser(int UserId, bool trackChanges)
        {
            // Build the query with a join and filtering logic
            var likes = FindByCondition(b => b.UserId == UserId, trackChanges)
             .OrderBy(x => x.LikedAt).AsSplitQuery().Include(x => x.Tweet).ThenInclude(x => x.User).Select(x=>x.Tweet).ToList();
            return likes;
        }
    }
}
