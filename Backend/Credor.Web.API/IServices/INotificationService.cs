using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public interface INotificationService
    {
        List<NotificationDto> GetUserNotifications(int userId);
        bool AddNotification(NotificationDto notification);
        bool DeleteNotification(int userId, int id);
        bool UpdateNotification(NotificationDto notificationDto);
        bool ClearAllNotifications(int userId);
        bool UpdateNotification(int userId, int Id);
    }
}
