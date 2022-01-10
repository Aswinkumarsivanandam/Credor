using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Credor.Web.API.Common
{
    public class NotificationHub : Hub
    {
        public Task NotificationCenterMessage(int userId, string message)
        {
            return Clients.All.SendAsync("NewNotification", userId, message);
        }       
    }
}
