using Entities.DomainModels;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts.Contracts;
using Repository.Extensions;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Repository.RepositoryModels
{
    public class TweetRepository : RepositoryBase<Tweet>, ITweetRepository
    {
        
        public TweetRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public void CreateTweet(int UserId, Tweet tweet)
        {
            tweet.UserID = UserId;
            tweet.CreatedAt = DateTime.Now;
            Create(tweet);
        }

        public void DeleteTweet(Tweet tweet)
        {
            var RepositoryContext = GetRepository();
            using (var transaction = RepositoryContext.Database.BeginTransaction())
            {
                try
                {
                    transaction.CreateSavepoint("beforeRemoval");

                    var bookmarks = RepositoryContext.Bookmarks.Where(x => x.TweetId.Equals(tweet.ID)).AsNoTracking();
                    if (bookmarks.Any()) RepositoryContext.Set<Bookmark>().RemoveRange(bookmarks);

                    var retweets = RepositoryContext.Retweets.Where(x => x.TweetId.Equals(tweet.ID)).AsNoTracking();
                    if (retweets.Any()) RepositoryContext.Set<Retweet>().RemoveRange(retweets);

                    var likes = RepositoryContext.Likes.Where(x => x.TweetId.Equals(tweet.ID)).AsNoTracking();
                    if (likes.Any()) RepositoryContext.Set<Like>().RemoveRange(likes);

                    var tweetTrends = RepositoryContext.TweetTrends.Where(x => x.TweetId.Equals(tweet.ID)).AsNoTracking();
                    if (tweetTrends.Any()) RepositoryContext.Set<TweetTrend>().RemoveRange(tweetTrends);

                    Delete(tweet);
                    transaction.Commit();
                }
                catch (Exception )
                {
                    // Log exception here if needed
                    transaction.RollbackToSavepoint("beforeRemoval");
                  
                }
            }
        }

    public async Task<Tweet> GetTweet(int id, bool trackChanges)
        {
            var result= await FindByCondition(x => x.ID.Equals(id), trackChanges).SingleOrDefaultAsync();
            return result!;
        }
           


        public async Task<PagedList<Tweet>> GetTweetsByUser(int UserId,TweetParameters tweetParameters, bool trackChanges)
        {
            var tweetsQuery = FindByCondition(tweet => tweet.UserID == UserId, trackChanges)
        .Sort<Tweet>(tweetParameters.OrderBy!)
        .Join(
            RepositoryContext.Set<User>(),        // Join with Users table
            tweet => tweet.UserID,                // Foreign key in Tweet
            user => user.Id,                      // Primary key in User
            (tweet, user) => new Tweet                          // Create a new Tweet instance
            {
                ID = tweet.ID,
                Content = tweet.Content,
                MediaURL = tweet.MediaURL,
                UserID = tweet.UserID,
                CreatedAt = tweet.CreatedAt,
                LikesNumber=tweet.LikesNumber,
                RetweetsNumber=tweet.RetweetsNumber,
                BookmarksNumber=tweet.BookmarksNumber,
                User = user                      // Include User if necessary
            }
        );


            return await PagedList<Tweet>
                   .ToPagedListAsync(tweetsQuery, tweetParameters.PageNumber,
                   tweetParameters.PageSize);
        }
      
    }
}
