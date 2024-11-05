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
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(x=>x.Id);
         
            builder.Property(x => x.Type).HasMaxLength(10);
            builder.HasOne(x=>x.Receiver).WithMany(x=>x.notifications).HasForeignKey(x=>x.ReceiverId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Sender).WithMany().HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.ReceiverId);  
        }
    }
}
