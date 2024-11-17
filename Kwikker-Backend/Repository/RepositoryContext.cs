using Entities.DomainModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository
{
    public class RepositoryContext : IdentityDbContext<User,ApplicationRole,int>
    {
        public RepositoryContext(DbContextOptions options)
         : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

           
            modelBuilder.ApplyConfiguration(new LikeConfiguration());
            modelBuilder.ApplyConfiguration(new TweetConfiguration());
            modelBuilder.ApplyConfiguration(new RetweetConfiguration());
           modelBuilder.ApplyConfiguration(new FollowConfiguration());
            modelBuilder.ApplyConfiguration(new BookmarkConfiguration());
            modelBuilder.ApplyConfiguration(new TrendConfiguration());
            modelBuilder.ApplyConfiguration(new TweetTrendConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());

        }
        public DbSet<Follow>Follows { get; set; }
        public DbSet<Tweet> Tweets { get; set; }    
        public DbSet<Like> Likes { get; set; }
        public DbSet<Retweet> Retweets { get; set; }    
        public DbSet<Bookmark> Bookmarks { get; set; }  
   
        public DbSet<Trend> Trends { get; set; }
       
        public DbSet<TweetTrend> TweetTrends { get; set; }
        
        public DbSet<Notification> Notifications { get; set; }  
    }
}
