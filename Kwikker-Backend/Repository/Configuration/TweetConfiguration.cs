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
    public class TweetConfiguration : IEntityTypeConfiguration<Tweet>
    {
        public void Configure(EntityTypeBuilder<Tweet> builder)
        {
            

            builder.HasKey(t => t.ID); // Primary key

            // Configure properties
            builder.Property(t => t.Content);
            
            builder.Property(t => t.CreatedAt).IsRequired();

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserID).OnDelete(DeleteBehavior.Cascade);  
            builder.HasIndex(t => t.UserID); // Index on UserId for efficient lookups
        }
    }
}
