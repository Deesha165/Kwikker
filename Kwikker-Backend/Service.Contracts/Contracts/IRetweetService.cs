using Shared.DTOs;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Contracts
{
    public interface IRetweetService
    {
        
     
        Task CreateRetweet(int userId, int tweetid, bool trackChanges);

        Task DeleteRetweet(int userId, int tweetid, bool trackChanges);

        Task<IEnumerable<TweetDTO>> GetUserRetweets(int userId, bool trackChanges);

    }
}
