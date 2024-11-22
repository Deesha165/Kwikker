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
    public class RetweetRepository : RepositoryBase<Retweet>, IRetweetRepository
    {
        public RetweetRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public void CreateRetweet(int userId, int tweetId)
        {
            Retweet retweet = new Retweet()
            {
                TweetId = tweetId,
                UserId = userId,
                RetweetedAt = DateTime.Now
            };

            var tweet = RepositoryContext.Set<Tweet>().FirstOrDefault(x => x.ID.Equals(tweetId));
            if (tweet != null) tweet.RetweetsNumber++;

            Create(retweet);

        }

        public async Task< Retweet> GetRetweet(int userId, int tweetId, bool trackChanges)

        {
           var result= await FindByCondition(x => x.UserId.Equals(userId) && x.TweetId.Equals(tweetId), trackChanges).SingleOrDefaultAsync();
            
            return result!;
        }

        public void DeleteRetweet(Retweet retweet)
        {
            var tweet = RepositoryContext.Set<Tweet>().FirstOrDefault(x => x.ID.Equals(retweet.TweetId));
            if (tweet != null) tweet.RetweetsNumber--;
            Delete(retweet);
        }


        public async Task<List<Tweet>> GetRetweetsByUser(int UserId, bool trackChanges)
        {
            var retweets = FindByCondition(b => b.UserId == UserId, trackChanges)
           .OrderBy(x => x.RetweetedAt).AsSplitQuery().Include(x => x.Tweet).ThenInclude(x => x.User).Select(x => x.Tweet).ToList();
            return retweets;
        }

       
    }
}
