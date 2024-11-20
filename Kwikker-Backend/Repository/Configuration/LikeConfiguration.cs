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
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(l => new { l.UserId, l.TweetId });


        

            builder.HasOne(l => l.User)
                  .WithMany()
                  .HasForeignKey(l => l.UserId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(l => l.Tweet)
                  .WithMany(t => t.Likes)
                  .HasForeignKey(l => l.TweetId).OnDelete(DeleteBehavior.NoAction);


            // Index on UserId and TweetId for efficient lookups
            builder.HasIndex(l => l.UserId);
            builder.HasIndex(l => l.TweetId);
        }
    }
}
