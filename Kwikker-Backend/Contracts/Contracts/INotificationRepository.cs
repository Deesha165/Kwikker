using Entities.DomainModels;
using Entities.Enums;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts.Contracts
{
    public interface INotificationRepository
    {
        void  CreateNotification(int senderId, string type, int receiverId);
        Task<IEnumerable<NotificationDTO>> GetUserNotificationsAsync(int receiverId,bool trackChanges);
      

        Task<Notification> GetNotification(int id,bool trackChanges); 
    }
}
