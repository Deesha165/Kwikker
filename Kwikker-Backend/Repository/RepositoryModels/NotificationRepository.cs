using Entities.DomainModels;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts.Contracts;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.RepositoryModels
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

       

        public void CreateNotification(int senderId, string type, int receiverId)
        {
            Notification notification = new Notification()
            {
               
                SenderId=senderId,
                ReceiverId=receiverId,
     
                Type=type
            };
            Create(notification);
        }
           
        
        public async Task<Notification> GetNotification(int id, bool trackChanges)
        {
           var notification=await FindByCondition(x => x.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
            return notification!;
        }

        public async Task<IEnumerable<NotificationDTO>> GetUserNotificationsAsync(int receiverId, bool trackChanges)
        {
            var notifications = await FindByCondition(x => x.ReceiverId == receiverId, trackChanges)
                .Select(n => new NotificationDTO(
                    n.Id,
                    n.Type!,
                    n.CreatedAt,
                    n.Sender!.ID,
                    n.Sender.Username,
                    n.Sender.Email,
                    n.Sender.ProfilePicture!
                ))
                .ToListAsync();

            return notifications!;
        }


    }
}
