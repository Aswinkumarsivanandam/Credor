using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Web.API.Common.UnitOfWork;
using Credor.Data.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NotificationRepository(IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = new UnitOfWork(configuration);
            _mapper = mapper;
        }
        public List<NotificationDto> GetUserNotifications(int userId)
        {
            List<NotificationDto> notifications = new List<NotificationDto>();
            var contextData = _unitOfWork.NotificationRepository.Context;
            try
            {
                notifications = (from notification in contextData.Notifications
                                 join user in contextData.UserAccount on Convert.ToInt32(notification.CreatedBy) equals user.Id
                                 where notification.UserId == userId && notification.Active == true
                                 select new NotificationDto
                                 {
                                     Id = notification.Id,
                                     UserId = notification.UserId,
                                     Title = notification.Title,
                                     Message = notification.Message,
                                     Status = notification.Status,
                                     CreatedOn = notification.CreatedOn,
                                     CreatedBy = user.FirstName + " " + user.LastName,
                                     CreatedByFirstName = user.FirstName,
                                     CreatedByLastName = user.LastName,
                                     CreatedByProfileImageURL = user.ProfileImageUrl,
                                     IsCreatedByAdmin = (user.RoleId == 3),
                                     DaysDifference = (DateTime.Now - notification.CreatedOn).Days, 
                                     MonthDifference = (DateTime.Now - notification.CreatedOn).Days/30,
                                     DisplayTime = notification.CreatedOn.ToString("hh:mm tt"),
                                 }).OrderByDescending(x=>x.CreatedOn).ToList();               
            }
            catch
            {
                notifications = null;
            } 
            finally
            {
                contextData = null;
            }
            return notifications;
        }
        public bool AddNotification(NotificationDto notificationDto)
        {
            try
            {
                using (var transaction = _unitOfWork.NotificationRepository.Context.Database.BeginTransaction())
                {
                    Notifications notification = new Notifications();
                    notification.UserId = notificationDto.UserId;
                    notification.Title = notificationDto.Title;
                    notification.Message = notificationDto.Message;
                    notification.Status = 1;
                    notification.Active = true;
                    notification.CreatedOn = DateTime.Now;
                    notification.CreatedBy = notificationDto.UserId.ToString();
                    _unitOfWork.NotificationRepository.Insert(notification);
                    _unitOfWork.Save();
                    transaction.Commit();
                    return true;
                }
            }
            catch(Exception e)
            {
                e.ToString();
                return false;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public bool UpdateNotification(NotificationDto notificationDto)
        {
            var notification = _unitOfWork.NotificationRepository.Get(x => x.Id == notificationDto.Id);
            if (notification != null)
            {
                try
                {
                    using (var transaction = _unitOfWork.NotificationRepository.Context.Database.BeginTransaction())
                    {
                        Notifications notificationData = notification;                       
                        notification.Status = notificationDto.Status;
                        notification.ModifiedBy = notificationDto.UserId.ToString();
                        _unitOfWork.NotificationRepository.Update(notification);
                        _unitOfWork.Save();
                        transaction.Commit();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }           
            return false;
        }
        public bool DeleteNotification(int userId, int id)
        {
            var notification = _unitOfWork.NotificationRepository.Get(x => x.Id == id);
            if (notification != null)
            {
                try
                {
                    using (var transaction = _unitOfWork.NotificationRepository.Context.Database.BeginTransaction())
                    {
                        Notifications notificationData = notification;
                        notification.Active = false;
                        notification.ModifiedBy = userId.ToString();
                        _unitOfWork.NotificationRepository.Update(notification);
                        _unitOfWork.Save();
                        transaction.Commit();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            return false;
        }

        public NotificationDto AddNotification(int userId, string title, string message,int adminUserId = 0)
        {
            try
            {
                using (var transaction = _unitOfWork.NotificationRepository.Context.Database.BeginTransaction())
                {
                    Notifications notification = new Notifications();
                    notification.UserId = userId;
                    notification.Title = title;
                    notification.Message = message;
                    notification.Status = 1;//Un-read
                    notification.Active = true;
                    if (adminUserId == 0)
                        notification.CreatedBy = userId.ToString();
                    else
                        notification.CreatedBy = adminUserId.ToString();
                    notification.CreatedOn = DateTime.Now;
                    _unitOfWork.NotificationRepository.Insert(notification);
                    _unitOfWork.Save();
                    transaction.Commit();
                    var notificationId = notification.Id;
                    UserAccount user;
                    NotificationDto notificationDto = new NotificationDto();
                    if (adminUserId == 0)
                        user = _unitOfWork.UserAccountRepository.GetByID(userId);
                    else
                    {
                        user = _unitOfWork.UserAccountRepository.GetByID(adminUserId);
                        notificationDto.IsCreatedByAdmin = true;
                    }
                    notificationDto.Id = notificationId;
                    notificationDto.UserId = userId;
                    notificationDto.Title = title;
                    notificationDto.Status = 1; //Un-read                    
                    notificationDto.Message = message;                   
                    notificationDto.CreatedByFirstName = user.FirstName;
                    notificationDto.CreatedByLastName = user.LastName;                    
                    notificationDto.CreatedByProfileImageURL = user.ProfileImageUrl;
                    notificationDto.CreatedOn = DateTime.Now;                   
                    return notificationDto;
                }
                
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public bool UpdateNotification(int userId, int Id)
        {
            var notification = _unitOfWork.NotificationRepository.Get(x => x.Id == Id);
            if (notification != null)
            {
                try
                {
                    using (var transaction = _unitOfWork.NotificationRepository.Context.Database.BeginTransaction())
                    {
                        Notifications notificationData = notification;
                        notification.Status = 2; //Read Status
                        notification.ModifiedBy = userId.ToString();
                        _unitOfWork.NotificationRepository.Update(notification);
                        _unitOfWork.Save();
                        transaction.Commit();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            return false; 
        }
        public bool ClearAllNotifications(int userId)
        {
            var notifications = _unitOfWork.NotificationRepository.GetMany(x => x.UserId == userId);
            try
            {
                List<Notifications> notificationList = new List<Notifications>();
                using (var transaction = _unitOfWork.NotificationRepository.Context.Database.BeginTransaction())
                {
                    foreach (Notifications notification in notifications)
                    {
                        Notifications notificationData = notification;
                        notification.Status = 2; //Read Status
                        notification.ModifiedBy = userId.ToString();
                        notificationList.Add(notificationData);
                    }
                    _unitOfWork.NotificationRepository.UpdateList(notificationList);
                    _unitOfWork.Save();
                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _unitOfWork.Dispose();
            }
            return false;
        }
    }
}
