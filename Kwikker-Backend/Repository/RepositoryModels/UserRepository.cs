using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.RepositoryModels
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<User> GetUser( int id, bool trackChanges)
        {
            var result= await FindByCondition(x => x.Id.Equals(id), trackChanges: false).SingleOrDefaultAsync();
            return result!;
        }
        public async Task<int> GetUsersCount()
        => await GetCount();
    }
}
