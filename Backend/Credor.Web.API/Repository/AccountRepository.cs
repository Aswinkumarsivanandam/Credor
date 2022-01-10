using AutoMapper;
using Credor.Web.API.Common.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Data.Entities;
using Credor.Web.API.Shared;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.SignalR;
using Credor.Web.API.Common;
using System.Resources;
using System.Reflection;

namespace Credor.Web.API
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UnitOfWork _unitOfWork;
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        public IConfiguration _configuration { get; }
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationRepository _notificationRepository;
        //ResourceManager rm = new ResourceManager("Credor.Web.API.Resource", Assembly.GetExecutingAssembly());
        public AccountRepository(INotificationRepository notificationRepository,
                                IHubContext<NotificationHub> hubContext,
                                IMapper mapper, 
                                IConfiguration configuration)
        {
            _unitOfWork = new UnitOfWork(configuration);
            _mapper = mapper;
            _configuration = configuration;
            _hubContext = hubContext;
            _notificationRepository = notificationRepository;
        }
        public UserAccountDto GetUserAccount(int userId)
        {
            UserAccountDto userAccount = new UserAccountDto();
            var contextData = _unitOfWork.UserAccountRepository.Context;
            try
            {
                userAccount = (from user in contextData.UserAccount
                               where user.Id == userId && user.Active == true
                               select new UserAccountDto
                               {
                                   Id = user.Id,
                                   RoleId = user.RoleId,
                                   FirstName = user.FirstName,
                                   LastName = user.LastName,
                                   NickName = user.NickName,
                                   EmailId = user.EmailId,
                                   SecondaryEmail = user.SecondaryEmail,
                                   ReceiveEmailNotifications = user.ReceiveEmailNotifications,
                                   DateOfBirth = user.DateOfBirth,
                                   ProfileImageUrl = user.ProfileImageUrl,
                                   Capacity = user.Capacity,
                                   HeardFrom = user.HeardFrom,
                                   PhoneNumber = user.PhoneNumber,
                                   Status = user.Status,
                                   Residency = user.Residency,
                                   Country = user.Country,
                                   IsAccreditedInvestor = user.IsAccreditedInvestor,
                                   IsEmailVerified = user.IsEmailVerified,
                                   IsPhoneVerified = user.IsPhoneVerified,
                                   IsTOCApproved = user.IsTOCApproved,
                                   IsTwoFactorAuthEnabled = user.IsTwoFactorAuthEnabled,
                                   CreatedBy = user.CreatedBy,
                                   CreatedOn = user.CreatedOn,
                                   PasswordChangedOn = user.PasswordChangedOn,
                                   CompanyNewsLetterUpdates = user.CompanyNewsLetterUpdates,
                                   NewInvestmentAnnouncements = user.NewInvestmentAnnouncements                                  
                                }).FirstOrDefault();                
            }
            catch
            {
                userAccount = null;
            }
            finally
            {
                contextData = null;
            }
            return userAccount;
        }
        public async Task<int> UpdateUserAccount(UpdateUserAccountDto userAccountDto)
        {
            var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == userAccountDto.Id);
            if (userAccount.Id != 0)
            {
                try
                {
                    string blobFilePath = userAccount.ProfileImageUrl;
                    if (userAccount.ProfileImageUrl != userAccountDto.ProfileImageUrl)
                    {
                        Helper _helper = new Helper(_configuration);
                        blobFilePath = await _helper.DocumentSaveAndUpload(userAccountDto.profileImage, userAccountDto.Id, 7);
                    }
                
                    using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                    {
                        UserAccount userAccountData = userAccount;
                        userAccountData.FirstName = userAccountDto.FirstName;
                        userAccountData.LastName = userAccountDto.LastName;
                        userAccountData.NickName = userAccountDto.NickName;
                        userAccountData.PhoneNumber = userAccountDto.PhoneNumber;
                        userAccountData.EmailId = userAccountDto.EmailId;
                        userAccountData.IsEmailVerified = userAccountDto.IsEmailVerified;
                        userAccountData.PhoneNumber = userAccountDto.PhoneNumber;
                        userAccountData.IsPhoneVerified = userAccountDto.IsPhoneVerified;
                        userAccountData.IsAccreditedInvestor = userAccountDto.IsAccreditedInvestor;
                        userAccountData.Capacity = userAccountDto.Capacity;
                        userAccountData.HeardFrom = userAccountDto.HeardFrom;
                        userAccountData.SecondaryEmail = userAccountDto.SecondaryEmail;
                        userAccountData.ReceiveEmailNotifications = userAccountDto.ReceiveEmailNotifications;
                        userAccountData.DateOfBirth = userAccountDto.DateOfBirth;
                        userAccountData.IsTwoFactorAuthEnabled = userAccountDto.IsTwoFactorAuthEnabled;
                        userAccountData.CompanyNewsLetterUpdates = userAccountDto.CompanyNewsLetterUpdates;
                        userAccountData.NewInvestmentAnnouncements = userAccountDto.NewInvestmentAnnouncements;
                        userAccountData.ProfileImageUrl = blobFilePath;
                        userAccountData.ModifiedBy = userAccountDto.Id.ToString();
                        if(userAccountDto.IsNewUser)
                        {
                            //Changing Role from NewUser 4 to Investor 1 on Registration completion
                            userAccountData.RoleId = 1;//Investor 
                        }                        
                        _unitOfWork.UserAccountRepository.Update(userAccountData);
                        _unitOfWork.Save();
                        transaction.Commit();                        
                    }
                    return 1; //Success
                }

                catch (Exception e)
                {
                    var ex = e.ToString();
                    return 0;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return 0;//Failure
        }
        public int UpdatePassword(UpdatePasswordDto passwordDto)
        {
            try
            {
                var user = _unitOfWork.UserAccountRepository.GetNoTrackWithInclude(x => x.Id == passwordDto.UserId).FirstOrDefault();
                if (user != null)
                {                    
                    byte[] existingSalt = user.PasswordSalt;
                    string existingHashedPassword = user.Password;
                    var hashedNewPassword = Convert.ToBase64String(Helper.HashPasswordWithSalt(Encoding.UTF8.GetBytes(passwordDto.NewPassword), existingSalt));
                    var hashedOldPassword = Convert.ToBase64String(Helper.HashPasswordWithSalt(Encoding.UTF8.GetBytes(passwordDto.OldPassword), existingSalt));
                    if(hashedNewPassword == user.Password)
                    {
                        return 1;// New Password should not same as Current Password
                    }
                    if (hashedOldPassword == existingHashedPassword)
                    {
                        if (hashedNewPassword != user.OldPassword)
                        {
                            using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                            {
                                byte[] salt = Helper.GenerateSalt();
                                user.PasswordSalt = salt;
                                var newPasswordHash = Helper.HashPasswordWithSalt(Encoding.UTF8.GetBytes(passwordDto.NewPassword), salt);
                                user.Password = Convert.ToBase64String(newPasswordHash);
                                user.PasswordChangedOn = DateTime.Now;
                                user.OldPassword = existingHashedPassword;
                                user.LastLogin = DateTime.Now;
                                // updates user password and salt
                                _unitOfWork.UserAccountRepository.UpdateProperty(
                                    user
                                    , new List<string>
                                        {
                                    "Password",
                                    "PasswordSalt",
                                    "PasswordChangedOn",
                                    "OldPassword"
                                        });
                                _unitOfWork.Save();
                                transaction.Commit();                                
                                return 0;// Sucesss
                            }
                        }
                        else
                            return 3; // New password should not be same as previously changed password
                    }
                    else
                    {
                        return 2;//Existing Password and Confirmation Password does not match
                    }
                }
            }
            catch(Exception e)
            {
                e.ToString();
            }
            finally
            {
                _unitOfWork.Dispose();
            }
            return 0;//Success
        }
        public async Task<bool> AddProfileImage(DocumentModelDto documentDto)
        {            
            var userAccount = _unitOfWork.UserAccountRepository.GetNoTrackWithInclude(x => x.Id == documentDto.UserId).FirstOrDefault();
            if (userAccount != null)
            {
                var contextData = _unitOfWork.UserAccountRepository.Context;
                try
                {                    
                    using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                    {
                        if (documentDto.Files != null)
                        {
                            Helper _helper = new Helper(_configuration);
                            var blobFilePath = (await _helper.DocumentSaveAndUpload(documentDto.Files.First(), documentDto.UserId, documentDto.Type)).ToString();

                            UserAccount userAccountData = userAccount;
                            userAccountData.ProfileImageUrl = blobFilePath;
                            userAccountData.ModifiedBy = documentDto.UserId.ToString();
                            contextData.Update(userAccountData);
                            contextData.SaveChanges();
                            transaction.Commit();
                        }                       
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                    return false;//Failure 
                }
                finally
                {
                    contextData = null;
                }
                return true;
            }
            return false;
        }
        public bool UpdateOTP(string OTP, int userId, UserAccount user)
        {
            try
            {
                using (var transaction = _unitOfWork.UserOTPRepository.Context.Database.BeginTransaction())
                {
                    UserOTP otpData = new UserOTP();
                    otpData.OTP = OTP;
                    otpData.UserId = userId;
                    otpData.CreatedBy = userId.ToString();
                    otpData.CreatedOn = DateTime.Now;
                    otpData.Active = true;                   
                    _unitOfWork.UserOTPRepository.Insert(otpData);
                    _unitOfWork.Save();
                    transaction.Commit();
                }                
                using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                {
                    UserAccount userAccountData = user;
                    user.OneTimePassword = OTP;
                    user.ModifiedBy = userId.ToString();
                    _unitOfWork.UserAccountRepository.Update(userAccountData);
                    _unitOfWork.Save();
                    transaction.Commit();
                }
                return true; //Success
            }

            catch (Exception e)
            {
                var ex = e.ToString();
                return false;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public bool UpdateUserEmailId(int userId, UserAccount user,string emailId)
        {
            try
            {
                using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                {
                    UserAccount userAccountData = user;
                    user.EmailId = emailId;
                    user.IsEmailVerified = true;
                    user.OneTimePassword = null;
                    user.ModifiedBy = userId.ToString();
                    _unitOfWork.UserAccountRepository.Update(userAccountData);
                    _unitOfWork.Save();
                    transaction.Commit();
                }
                using (var transaction = _unitOfWork.UserOTPRepository.Context.Database.BeginTransaction())
                {
                    UserOTP otpData = _unitOfWork.UserOTPRepository.Get(x => x.UserId == userId && x.Active == true);
                    if (otpData != null)
                    { 
                        otpData.Active = false;
                        otpData.ModifiedBy = userId.ToString();
                        _unitOfWork.UserOTPRepository.Update(otpData);
                        _unitOfWork.Save();
                        transaction.Commit();
                    }
                }
                string message = "You verified Email-Id " + emailId + "successfully";
                var notification = _notificationRepository.AddNotification(userId, "User EmailId Verified", message);
                if (notification != null)
                {
                    _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                }
                return true; //Success
            }

            catch (Exception e)
            {
                var ex = e.ToString();
                return false;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public bool UpdateUserPhoneNumber(int userId, UserAccount user, string phoneNumber)
        {
            try
            {
                using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                {
                    UserAccount userAccountData = user;
                    user.PhoneNumber = phoneNumber;
                    user.IsPhoneVerified = true;
                    user.OneTimePassword = null;
                    user.ModifiedBy = userId.ToString();
                    _unitOfWork.UserAccountRepository.Update(userAccountData);
                    _unitOfWork.Save();
                    transaction.Commit();
                }
                using (var transaction = _unitOfWork.UserOTPRepository.Context.Database.BeginTransaction())
                {
                    UserOTP otpData = _unitOfWork.UserOTPRepository.Get(x => x.UserId == userId && x.Active == true);
                    if (otpData != null)
                    {
                        otpData.Active = false;
                        otpData.ModifiedBy = userId.ToString();
                        _unitOfWork.UserOTPRepository.Update(otpData);
                        _unitOfWork.Save();
                        transaction.Commit();
                    }
                }
                string message = "You verified phone number " + phoneNumber + "successfully";
                var notification = _notificationRepository.AddNotification(userId, "User Phonenumber Verified", message);
                if (notification != null)
                {
                    _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                }
                return true; //Success
            }

            catch (Exception e)
            {
                var ex = e.ToString();
                return false;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public int AddUserAccount(NewUserAccountDto newAccount)
        {
            int userId = 0;
            int userPermissionId = 0;            
            try
            {
               
                using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                {
                    UserAccount userAccount = new UserAccount();
                    userAccount.FirstName = newAccount.FirstName;
                    userAccount.LastName = newAccount.LastName;
                    userAccount.NickName = newAccount.NickName;
                    userAccount.EmailId = newAccount.EmailId;
                    userAccount.RoleId = 4;
                    userAccount.Title = newAccount.Title;
                    userAccount.Active = true;
                    userAccount.Status = 1;                                     
                    userAccount.CreatedBy = newAccount.CurrentUserId.ToString();                    
                    _unitOfWork.UserAccountRepository.Insert(userAccount);
                    _unitOfWork.Save();
                    transaction.Commit();
                    userId = userAccount.Id;
                }                
                using (var permissiontransaction = _unitOfWork.UserPermissionRepository.Context.Database.BeginTransaction())
                {
                    UserPermission userPermission = new UserPermission();
                    userPermission.UserId = newAccount.CurrentUserId;
                    userPermission.AccessGrantedTo = userId;
                    userPermission.IsNotificationEnabled = newAccount.IsNotificationEnabled;
                    userPermission.Permission = newAccount.Permission;
                    userPermission.Active = true;
                    userPermission.CreatedBy = newAccount.CurrentUserId.ToString();                    
                    _unitOfWork.UserPermissionRepository.Insert(userPermission);
                    _unitOfWork.Save();
                    permissiontransaction.Commit();
                    userPermissionId = userPermission.Id;
                }
               
                string messgae = "User " + newAccount.FirstName + " " + newAccount.LastName + " added successfully";
                var notification = _notificationRepository.AddNotification(newAccount.CurrentUserId, "User Created", messgae);
                if (notification != null)
                {                    
                    _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                }
                return userId; //Success
            }

            catch (Exception e)
            {
                var ex = e.ToString();

                if (userId != 0)
                    _unitOfWork.UserAccountRepository.Delete(x => x.Id == userId);
                if (userPermissionId != 0)
                    _unitOfWork.UserPermissionRepository.Delete(x => x.Id == userPermissionId); 
                
                return userId;// 0 Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }                        
        }
        public bool UpdateNewUserAccount(NewUserAccountDto newUserAccount)
        {
            var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == newUserAccount.Id);
            if (userAccount.Id != 0)
            {
                try
                {
                    using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                    {                        
                        userAccount.FirstName = newUserAccount.FirstName;
                        userAccount.LastName = newUserAccount.LastName;
                        userAccount.NickName = newUserAccount.NickName;
                        userAccount.EmailId = newUserAccount.EmailId;
                        userAccount.Title = newUserAccount.Title;
                        userAccount.ModifiedBy = newUserAccount.CurrentUserId.ToString();
                        _unitOfWork.UserAccountRepository.Update(userAccount);
                        _unitOfWork.Save();
                        transaction.Commit();
                    }

                    var userPermission = _unitOfWork.UserPermissionRepository.Get(x => x.AccessGrantedTo == newUserAccount.Id);

                    using (var permissiontransaction = _unitOfWork.UserPermissionRepository.Context.Database.BeginTransaction())
                    {
                        if (userPermission.IsNotificationEnabled != newUserAccount.IsNotificationEnabled)
                            userPermission.IsNotificationEnabled = newUserAccount.IsNotificationEnabled;
                        if (newUserAccount.Permission != userPermission.Permission)
                            userPermission.Permission = newUserAccount.Permission;
                        userPermission.ModifiedBy = newUserAccount.CurrentUserId.ToString();
                        _unitOfWork.UserPermissionRepository.Update(userPermission);
                        _unitOfWork.Save();
                        permissiontransaction.Commit();                         
                    }
                    string messgae = "You updated user details " + newUserAccount.FirstName + " " + newUserAccount.LastName + "successfully";
                    var notification = _notificationRepository.AddNotification(newUserAccount.CurrentUserId, "UserAccount updated", messgae);
                    if (notification != null)
                    {
                        _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                    }
                    return true; //Success
                }                
                catch (Exception e)
                {
                    var ex = e.ToString();
                    return false;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return false;//Failure                      
        }
        public bool DeleteNewUserAccount(int currentUserId, int newUserId)
        {
            var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == newUserId && x.Active == true);
            if (userAccount.Id != 0)
            {
                try
                {
                    /*using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                    {
                        userAccount.Active = false;                        
                        userAccount.ModifiedBy = currentUserId.ToString();
                        _unitOfWork.UserAccountRepository.Update(userAccount);
                        _unitOfWork.Save();
                        transaction.Commit();
                    }*/

                    var userPermission = _unitOfWork.UserPermissionRepository.Get(x => x.AccessGrantedTo == newUserId && x.Active == true);
                    if (userPermission.Id != 0)
                    {
                        using (var permissiontransaction = _unitOfWork.UserPermissionRepository.Context.Database.BeginTransaction())
                        {
                            userPermission.Active = false;
                            userPermission.ModifiedBy = currentUserId.ToString();
                            _unitOfWork.UserPermissionRepository.Update(userPermission);
                            _unitOfWork.Save();
                            permissiontransaction.Commit();
                        }
                    }
                    string message = "You deleted user " + userAccount.FirstName + " " + userAccount.LastName + "successfully";
                    var notification = _notificationRepository.AddNotification(userAccount.Id, "UserPermission deleted", message);
                    if (notification != null)
                    {
                        _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                    }
                    return true; //Success
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                    return false;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return false;//Failure                      
        }
        public List<UserPermissionDto> GetAccessGrantedToOthers(int userId)
        {
            //Get access granted to others
            List<UserPermissionDto> userPermissions = new List<UserPermissionDto>();
            var contextData = _unitOfWork.UserPermissionRepository.Context;
            try
            {
                userPermissions = (from userPermission in contextData.UserPermission
                                   join userAccount in contextData.UserAccount on userPermission.AccessGrantedTo equals userAccount.Id
                                   where userPermission.UserId == userId && userPermission.Active == true
                                   select new UserPermissionDto
                                   {                                       
                                       UserId = userPermission.AccessGrantedTo,
                                       FirstName = userAccount.FirstName,
                                       LastName = userAccount.LastName,
                                       NickName = userAccount.NickName,
                                       Title = userAccount.Title,
                                       RoleId = userAccount.RoleId,
                                       EmailId = userAccount.EmailId,
                                       Permission = userPermission.Permission,                                       
                                       IsNotificationEnabled = userPermission.IsNotificationEnabled,
                                       CreatedBy = userPermission.CreatedBy,
                                       CreatedOn = userPermission.CreatedOn,
                                       ModifiedBy = userPermission.ModifiedBy,
                                       ModifiedOn = userPermission.ModifiedOn
                                   }).OrderByDescending(x=>x.CreatedOn).ToList();
            }
            catch(Exception e)
            {
                e.ToString();
                userPermissions = null;
            }
            finally
            {
                contextData = null;
            }
            return userPermissions;
        }
        public List<UserPermissionDto> GetAccessGrantedToUser(int userId)
        {
            //Get access granted to others
            List<UserPermissionDto> userPermissions = new List<UserPermissionDto>();
            var contextData = _unitOfWork.UserPermissionRepository.Context;
            try
            {
                userPermissions = (from userPermission in contextData.UserPermission
                                   join userAccount in contextData.UserAccount on userPermission.UserId equals userAccount.Id
                                   where userPermission.AccessGrantedTo == userId && userPermission.Active == true
                                   select new UserPermissionDto
                                   {                                      
                                       UserId = userPermission.UserId,
                                       FirstName = userAccount.FirstName,
                                       LastName = userAccount.LastName,
                                       NickName = userAccount.NickName,
                                       Title = userAccount.Title,
                                       RoleId = userAccount.RoleId,
                                       EmailId = userAccount.EmailId,
                                       Permission = userPermission.Permission,                                       
                                       IsNotificationEnabled = userPermission.IsNotificationEnabled,
                                       CreatedBy = userPermission.CreatedBy,
                                       CreatedOn = userPermission.CreatedOn,
                                       ModifiedBy = userPermission.ModifiedBy,
                                       ModifiedOn = userPermission.ModifiedOn
                                   }).OrderByDescending(x => x.CreatedOn).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
                userPermissions = null;
            }
            finally
            {
                contextData = null;
            }
            return userPermissions;
        }
        public bool UpdateNewUserAccountPassword(UserAccount userAccount)
        {
            try
            {             
                using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                {
                    byte[] salt = Helper.GenerateSalt();
                    userAccount.PasswordSalt = salt;
                    var newPasswordHash = Helper.HashPasswordWithSalt(Encoding.UTF8.GetBytes(userAccount.Password), salt);
                    userAccount.Password = Convert.ToBase64String(newPasswordHash);
                    userAccount.ModifiedBy = userAccount.Id.ToString();
                    _unitOfWork.UserAccountRepository.Update(userAccount);                                            
                    _unitOfWork.Save();
                    transaction.Commit();
                    return true;// Sucesss                                                   
                }                    
                
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }
            finally
            {
                _unitOfWork.Dispose();
            }            
        }
    }
}
