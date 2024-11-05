using Entities.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration
{
    public class TrendConfiguration : IEntityTypeConfiguration<Trend>
    {
        public void Configure(EntityTypeBuilder<Trend> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(t => t.hashtag).IsUnique();
            builder.HasIndex(t => t.DecayScore);
        }
    }
}
