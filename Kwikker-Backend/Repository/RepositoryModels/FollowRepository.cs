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
    public class FollowRepository : RepositoryBase<Follow>, IFollowRepository
    {
        public FollowRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateFollow(int followerId, int followeeId)
        {
            Follow follow = new Follow()
            {
                FollowerID = followerId,
                FolloweeID = followeeId,
                CreatedAt=DateTime.Now
            };
            Create(follow);

        }

        public void DeleteFollow(Follow follow)
       => Delete(follow);

        public async Task<Follow> GetFollow(int followerId, int followeeId, bool trackChanges)
        {
           var follow= await FindByCondition(x => x.FollowerID.Equals(followerId) && x.FolloweeID.Equals(followeeId),trackChanges).SingleOrDefaultAsync();
            return follow!;
        }

        public async Task<PagedList<User>> GetUserFollowees(int followerId, FollowingParameters followingParameters, bool trackChanges)
        {
            var followees =
               FindByCondition(x => x.FollowerID == followerId, trackChanges)
               .Sort<Follow>(followingParameters.OrderBy!)
               .Join(
                    RepositoryContext.Set<User>(), // Join with Users table
                    f => f.FolloweeID,                 // Foreign key in Follow
                    u => u.Id,                      // Primary key in User
                    (f, u) => u                  // Select entire User entity
                );




            return await PagedList<User>
                   .ToPagedListAsync(followees, followingParameters.PageNumber,
                   followingParameters.PageSize);
        }

       

        public async Task<PagedList<User>> GetUserFollowers(int followeeId, FollowingParameters followingParameters, bool trackChanges)
        {
           var followers= FindByCondition(x => x.FolloweeID == followeeId, trackChanges)
                .Sort<Follow>(followingParameters.OrderBy!)
                .Join(
                    RepositoryContext.Set<User>(), // Join with Users table
                    f => f.FollowerID,                 // Foreign key in Follow
                    u => u.Id,                      // Primary key in User
                    (f, u) => u                  // Select entire User entity
                );

            return await PagedList<User>
                   .ToPagedListAsync(followers, followingParameters.PageNumber,
                   followingParameters.PageSize);
        }

        
    }
}
