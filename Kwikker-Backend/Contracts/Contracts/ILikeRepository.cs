using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts.Contracts
{
    public interface ILikeRepository
    {
      
        void CreateLike(int userId, int tweetId);
        void DeleteLike(Like like);
        Task<Like> GetLike(int userId, int tweetId,bool trackChanges);

        Task<List<Tweet>> GetLikedTweetsByUser(int UserId,  bool trackChanges);
    }
}
