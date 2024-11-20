using Entities.DomainModels;

using Shared.DTOs;


namespace Repository.Contracts.Contracts
{
    public interface INotificationRepository
    {
        void  CreateNotification(int senderId, string type, int receiverId);
        Task<IEnumerable<NotificationDTO>> GetUserNotificationsAsync(int receiverId,bool trackChanges);
      

        Task<Notification> GetNotification(int id,bool trackChanges); 
    }
}
