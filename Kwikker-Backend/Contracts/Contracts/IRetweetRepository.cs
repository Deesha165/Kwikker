using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts.Contracts
{
    public interface IRetweetRepository
    {
      
        void CreateRetweet(int userId, int tweetId);
        void DeleteRetweet(Retweet retweet);
        Task<Retweet> GetRetweet(int userId, int tweetId, bool trackChanges);

        Task<List<Tweet>> GetRetweetsByUser(int UserId, bool trackChanges);
    }
}
