using Shared.DTOs;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Contracts
{
    public interface IFollowService
    {
        Task CreateFollow(int followerId, int followeeId, bool trackChanges);
        Task DeleteFollow(int followerId,int followeeId, bool trackChanges);
        Task<(IEnumerable<GeneralUserDTO> followers, MetaData metaData)> GetUserFollowers(int followeeID, FollowingParameters followingParameters, bool trackChanges);
        Task<(IEnumerable<GeneralUserDTO> followees, MetaData metaData)> GetUserFollowees(int followerID, FollowingParameters followingParameters, bool trackChanges);

        Task<List<ExpandoObject>> GetSuggestedToFollow(int UserId, UserParameters userParameters);
        Task<IEnumerable<int>> GetRandomUsers(int UserId);
    }
}
