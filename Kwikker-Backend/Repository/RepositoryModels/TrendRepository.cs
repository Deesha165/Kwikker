using EFCore.BulkExtensions;
using Entities.DomainModels;
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
    public class TrendRepository : RepositoryBase<Trend>, ITrendRepository
    {
        public TrendRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        

        public async Task CreateNewTrendsAsync(List<Trend>trends)
        {
            await RepositoryContext.BulkInsertAsync(trends, options =>
            {
                options.SetOutputIdentity = true; // Ensures IDs are retrieved
            });
        }
        

        public async Task<IDictionary<string, Trend>> GetExistingTrends(List<string> trends)
        {
            var existingTrends=await FindByCondition(x => trends.Contains(x.hashtag), trackChanges: true).ToDictionaryAsync(x => x.hashtag,x=>x);
            return existingTrends;
        }

        public async Task<IEnumerable<Trend>> GetTopTrends()
         => await  RepositoryContext.Trends.OrderByDescending(x => x.DecayScore).Take(10).ToListAsync();
        


        public async Task<IEnumerable<Tweet>> GetTweetsByTrend(string hashtag)
        {
            var trend = await RepositoryContext.Trends.FirstOrDefaultAsync(x => x.hashtag.Equals(hashtag));
            if (trend == null) return Enumerable.Empty<Tweet>();    
            var tweets = await RepositoryContext.TweetTrends.Where(x => x.TrendId.Equals(trend.Id)).Include(x => x.Tweet).ThenInclude(y=>y.User).Select(x=>x.Tweet).ToListAsync();
            

            return tweets!;
        }
      
    }
}
