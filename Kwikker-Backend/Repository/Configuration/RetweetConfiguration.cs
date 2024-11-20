using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration
{
    public class RetweetConfiguration : IEntityTypeConfiguration<Retweet>
    {
        public void Configure(EntityTypeBuilder<Retweet> builder)
        {
            builder.HasKey(r => new { r.UserId, r.TweetId });

     
            builder.HasOne(r => r.User)
                  .WithMany()
                  .HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Tweet)
                  .WithMany()
                  .HasForeignKey(r => r.TweetId).OnDelete(DeleteBehavior.NoAction);

            // Index on UserId and TweetId for optimized queries
            builder.HasIndex(r => r.UserId);
            builder.HasIndex(r => r.TweetId);
        }
    }
}
