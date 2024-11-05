using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Contracts
{
    public interface INotificationService
    {
        Task CreateNotification(int senderId, string type, int receiverId);
        Task<IEnumerable<NotificationDTO>> GetUserNotificationsAsync(int receiverId, bool trackChanges);
       
    }
}
