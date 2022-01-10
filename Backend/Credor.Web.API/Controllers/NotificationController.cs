using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API.Controllers
{
    [Route("Notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;        

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;           

        }
        [HttpGet]
        [Route("getusernotifications/{userid}")]
        public IActionResult GetUserNotifications(int userid)
        {
            var userAccount = _notificationService.GetUserNotifications(userid);
            return Ok(userAccount);
        }
        [HttpPost]
        [Route("addnotification")]
        public IActionResult AddNotification(NotificationDto notification)
        {
            var userAccount = _notificationService.AddNotification(notification);
            return Ok(userAccount);
        }
        [HttpPost]
        [Route("updatenotification")]
        public IActionResult UpdateNotification(NotificationDto notification)
        {
            var userAccount = _notificationService.UpdateNotification(notification);
            return Ok(userAccount);
        }
        [HttpDelete]
        [Route("deletenotification/{userid}/{id}")]
        public IActionResult DeleteNotification(int userid, int id)
        {
            var userAccount = _notificationService.DeleteNotification(userid,id);
            return Ok(userAccount);
        }
        [HttpDelete]
        [Route("clearallnotifications/{userid}")]
        public IActionResult ClearAllNotification(int userid)
        {
            var userAccount = _notificationService.ClearAllNotifications(userid);
            return Ok(userAccount);
        }
        [HttpPost]
        [Route("updatenotificationbyid/{userid}/{id}")]
        public IActionResult UpdateNotificationById(int userid,int id)
        {
            var userAccount = _notificationService.UpdateNotification(userid,id);
            return Ok(userAccount);
        }

    }
}
