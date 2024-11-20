using Entities.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration
{
    public class TweetTrendConfiguration : IEntityTypeConfiguration<TweetTrend>
    {
        public void Configure(EntityTypeBuilder<TweetTrend> builder)
        {
            builder.HasKey(x => new { x.TweetId, x.TrendId });

            builder.HasIndex(x => new { x.TrendId ,x.TweetId}); 

            builder.HasOne(x => x.Trend).WithMany().HasForeignKey(x => x.TrendId);

            builder.HasOne(x => x.Tweet).WithMany().HasForeignKey(x => x.TweetId);
        }
    }
}
