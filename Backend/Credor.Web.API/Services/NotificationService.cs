using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public class NotificationService :INotificationService
    {
        private readonly INotificationRepository _notificationRepository;        
        public NotificationService(INotificationRepository notificationRespository)
        {
            _notificationRepository = notificationRespository;            
        }
        public List<NotificationDto> GetUserNotifications(int userId)
        {
            return _notificationRepository.GetUserNotifications(userId);
        }
        public bool AddNotification(NotificationDto notification)
        {
            return _notificationRepository.AddNotification(notification);
        }
        public bool DeleteNotification(int userId, int id)
        {
            return _notificationRepository.DeleteNotification(userId,id);
        }
        public bool UpdateNotification(NotificationDto notificationDto)
        {
            return _notificationRepository.UpdateNotification(notificationDto);
        }
        public bool UpdateNotification(int userId,int Id)
        {
            return _notificationRepository.UpdateNotification(userId, Id);
        }
        public bool ClearAllNotifications(int userId)
        {
            return _notificationRepository.ClearAllNotifications(userId);
        }
    }
}
