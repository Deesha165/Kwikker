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
    public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
    {
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder.HasKey(b => new { b.UserId, b.TweetId });


           

            builder.HasOne(b => b.User)
                  .WithMany()
                  .HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Tweet)
                  .WithMany(t => t.Bookmarks)
                  .HasForeignKey(b => b.TweetId).OnDelete(DeleteBehavior.NoAction); 


            // Index on UserId and TweetId for optimized querying
            builder.HasIndex(b => b.UserId);
            builder.HasIndex(b => b.TweetId);
        }
    }
}
