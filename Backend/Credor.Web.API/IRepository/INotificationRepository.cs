using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Data.Entities;

namespace Credor.Web.API
{
    public interface INotificationRepository
    {
        List<NotificationDto> GetUserNotifications(int userId);
        bool AddNotification(NotificationDto notification);
        bool DeleteNotification(int userId, int id);
        bool UpdateNotification(NotificationDto notificationDto);
        NotificationDto AddNotification(int userId, string title, string message, int adminUserId = 0);
        bool ClearAllNotifications(int userId);
        bool UpdateNotification(int userId, int Id);
    }
}
