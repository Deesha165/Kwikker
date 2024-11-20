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
    public class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.HasKey(f => new { f.FollowerID, f.FolloweeID });

            // Configure Follower -> User relationship
            builder.HasOne(f => f.Follower)
                   .WithMany()  // No need to configure the inverse navigation here
                   .HasForeignKey(f => f.FollowerID).OnDelete(DeleteBehavior.NoAction);

            // Configure Followee -> User relationship
            builder.HasOne(f => f.Followee)
                   .WithMany()  // No need to configure the inverse navigation here
                   .HasForeignKey(f => f.FolloweeID).OnDelete(DeleteBehavior.NoAction);
                   


            builder.HasIndex(x => x.FollowerID);
            builder.HasIndex(x => x.FolloweeID);
        }
    }
}
