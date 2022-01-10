using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Web.API;
using Credor.Web.API.Common.UnitOfWork;
using Credor.Web.API.Shared;
using Credor.Data.Entities;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Credor.Web.API.Common;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UnitOfWork _unitOfWork;
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;        
        public AuthenticationRepository(IHubContext<NotificationHub> hubContext, 
                                        IMapper mapper,
                                        INotificationRepository notificationRepository,
                                        IConfiguration configuration)
        {
            _unitOfWork = new UnitOfWork(configuration);
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }
        public List<CapacityDto> GetCapacityRanges()
        {
            List<CapacityDto> ranges = new List<CapacityDto>();
            var contextData = _unitOfWork.CapacityRepository.Context;
            try
            {
                ranges = (from range in contextData.Capacity
                            select new CapacityDto
                            {
                                Id = range.Id,
                                CapacityRange = range.CapacityRange,
                                Active = range.Active
                            }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
                return ranges;
            }
            finally
            {
                contextData = null;
            }
            return ranges;
        }
        public int CreateUserAccount(UserAccountDto userAccount)
        {
            if (userAccount != null)
            {
                try
                {
                    using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                    {
                        if (!string.IsNullOrEmpty(userAccount.EmailId))
                        {
                            var isEmailIdExists = _unitOfWork.UserAccountRepository.GetWithInclude(x => x.EmailId.ToLower() == userAccount.EmailId.ToLower() && x.Active == true && x.Status == 1).FirstOrDefault();
                            if (isEmailIdExists != null)
                            {
                                return 3;//  Already an active/approved account exists with this EmailId
                            }
                        }

                       /* else if (!string.IsNullOrEmpty(userAccount.UserName))
                        {
                            var isUserNameExists = _unitOfWork.UserAccountRepository.GetWithInclude(x => x.UserName.ToLower() == userAccount.UserName.ToLower() && x.Active == true && x.Status == 1).FirstOrDefault();
                            if (isUserNameExists != null)
                            {
                                return 4;//  Already an active/approved account exists with this Username
                            }
                        }*/

                        byte[] salt = Helper.GenerateSalt();
                        userAccount.PasswordSalt = salt;
                        userAccount.Password = Convert.ToBase64String(Helper.HashPasswordWithSalt(Encoding.UTF8.GetBytes(userAccount.Password), salt));
                        userAccount.CreatedOn = DateTime.Now;

                        UserAccount account = _mapper.Map<UserAccount>(userAccount);
                        account.RoleId = 2; //Lead
                        _unitOfWork.UserAccountRepository.Insert(account);
                        _unitOfWork.Save();
                        transaction.Commit();
                        var userId = account.Id;

                        //Adding Owner Profile
                        using (var profiletransaction = _unitOfWork.UserProfileRepository.Context.Database.BeginTransaction())
                        {
                            try
                            {
                                Helper _helper = new Helper();
                                UserProfile userProfile = new UserProfile();
                                userProfile.UserId = userId;
                                userProfile.Type = 3; //Individual
                                userProfile.FirstName = userAccount.FirstName;
                                userProfile.LastName = userAccount.LastName;
                                userProfile.IsOwner = true;
                                userProfile.BankAccountId = null;
                                userProfile.Active = true;
                                userProfile.Status = 1;
                                userProfile.CreatedBy = userId.ToString();
                                userProfile.CreatedOn = DateTime.Now;
                                userProfile.DisplayId = _helper.GetRandomString(2);
                                _unitOfWork.UserProfileRepository.Insert(userProfile);
                                _unitOfWork.Save();
                                profiletransaction.Commit();

                                string messgae =  userAccount.FirstName + " " + userAccount.LastName + " Registered";
                                var notification = _notificationRepository.AddNotification(userId, "UserAccount Created", messgae);
                                if (notification != null)
                                {
                                    _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                                }
                            }
                            catch (Exception e)
                            {
                                e.ToString();
                                _unitOfWork.UserAccountRepository.Delete(x => x.Id == userId);
                                _unitOfWork.Save();
                                return 0;//Failure
                            }
                        }
                        return 1; //Success
                    }                    
                }

                catch (Exception e)
                {
                    e.ToString();
                    return 5;//Database Exception
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return 2;//Invalid Data
        }
        public UserAccount ForgotPassword(UserCredentialDto userCredential)
        {
            try
            {
                /*if (!string.IsNullOrEmpty(userCredential.UserName))
                {
                    var user = _unitOfWork.UserAccountRepository.GetWithInclude(x => x.UserName.ToLower() == userCredential.UserName.ToLower() && x.Active == true && x.Status == 1).FirstOrDefault();
                    if (user != null)
                    {
                        return user;
                    }
                }*/
                if (!string.IsNullOrEmpty(userCredential.EmailId))
                {
                    var user = _unitOfWork.UserAccountRepository.GetWithInclude(x => x.EmailId.ToLower() == userCredential.EmailId.ToLower() && x.Active == true && x.Status == 1).FirstOrDefault();
                    if (user != null)
                    {
                        return user;
                    }
                }
            }
            catch(Exception)
            {
                return null;
            }
            return null;
        }
       
       public bool ResetPassword(int id, ResetCredentialDto userCredential)
        {
            try
            {
                var user = _unitOfWork.UserAccountRepository.GetNoTrackWithInclude(x => x.Id == id).FirstOrDefault();
                if (user != null)
                {
                    byte[] existingSalt = user.PasswordSalt;
                    string existingHashedPassword = "";
                    var hashedPassword = Convert.ToBase64String(Helper.HashPasswordWithSalt(Encoding.UTF8.GetBytes(userCredential.Password), existingSalt));                   
                    existingHashedPassword = user.Password;                    
                    if (hashedPassword != existingHashedPassword)
                    {
                        using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                        {
                            byte[] salt = Helper.GenerateSalt();
                            user.PasswordSalt = salt;
                            var newPasswordHash = Helper.HashPasswordWithSalt(Encoding.UTF8.GetBytes(userCredential.Password), salt);
                            user.Password = Convert.ToBase64String(newPasswordHash);
                            user.PasswordChangedOn = DateTime.Now;
                            user.OldPassword = existingHashedPassword;
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
                            return true;
                        }
                    }
                    else
                    {
                        return false; 
                    }
                }
            }
            catch
            {
            }
            finally
            {
                _unitOfWork.Dispose();
            }
            return false;

        }

        public UserAccountDto UpdateUserAccount(UserAccountDto userAccount)
        {
            throw new NotImplementedException();
        }

        public UserAccountDto VerifyUserAccount(string UserName, string Password)
        {
            UserAccountDto userAccount = null;
            try
            {

                UserAccount user = _unitOfWork.UserAccountRepository.GetNoTrackWithInclude(x => x.EmailId == UserName && x.Active == true && x.Status == 1).FirstOrDefault();
                if (user != null)
                {
                    byte[] existingSalt = user.PasswordSalt;
                    
                    dynamic hashedPassword = null;
                    dynamic existingHashedPassword = null;
                    hashedPassword = Convert.ToBase64String(Helper.HashPasswordWithSalt(Encoding.UTF8.GetBytes(Password), existingSalt));
                    existingHashedPassword = user.Password;
                    if (hashedPassword == existingHashedPassword)
                    {
                        userAccount = _mapper.Map<UserAccountDto>(user);
                        if (userAccount != null)
                        {
                            user.LastLogin = DateTime.Now;
                            _unitOfWork.UserAccountRepository.Update(user);
                            _unitOfWork.Save();                            
                        }                        
                        return userAccount;
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userAccount;
        }
        public List<UserAccountDto> GetAllUsers()
        {
            List<UserAccountDto> userAccountList = new List<UserAccountDto>();            
            var contextData = _unitOfWork.UserAccountRepository.Context;
            try
            {              
                userAccountList = (from user in contextData.UserAccount                                  
                                  select new UserAccountDto
                                  {
                                      Id = user.Id,
                                      RoleId = user.RoleId,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      NickName = user.NickName,
                                      EmailId = user.EmailId,
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
                                      CreatedOn = user.CreatedOn                                     
                                  }).OrderBy(x => x.CreatedOn).ToList();               
            }
            catch
            {
                userAccountList = null;
            }
            finally
            {
                contextData = null;
            }
            return userAccountList;
        }
        public List<StateOrProvince> GetStateOrProvinceList()
        {
            try
            {
                var result = _unitOfWork.StateOrProvinceRepository.GetAll().ToList(); ;
                return result;
            }
            catch
            {
                return null;
            }
        }
        public UserAccountDto GetUserById(int userId)
        {
            UserAccountDto userAccount = new UserAccountDto();
            var contextData = _unitOfWork.UserAccountRepository.Context;
            try
            {
                userAccount = (from user in contextData.UserAccount
                               join up in contextData.UserProfile on user.Id equals up.UserId
                                   where user.Id == userId && up.IsOwner == true
                                   select new UserAccountDto
                                   {
                                       Id = user.Id,
                                       RoleId = user.RoleId,
                                       FirstName = user.FirstName,
                                       LastName = user.LastName,
                                       NickName = user.NickName,
                                       EmailId = user.EmailId,
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
                                       Isowner = up.IsOwner,
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
        public List<RoleFeatureMappingDto> GetRoleFeatureMapping(int roleId)
        {
            List<RoleFeatureMappingDto> roleMappings = new List<RoleFeatureMappingDto>();
            var contextData = _unitOfWork.RoleFeatureMappingRepository.Context;
            try
            {
                roleMappings = (from roleFeatureMapping in contextData.RoleFeatureMapping
                                where roleFeatureMapping.RoleId == roleId
                                && roleFeatureMapping.Active == true
                                select new RoleFeatureMappingDto
                                {
                                    Id = roleFeatureMapping.Id,
                                    RoleId = roleFeatureMapping.RoleId,
                                    FeatureName = roleFeatureMapping.FeatureName,
                                    Active = roleFeatureMapping.Active
                                }).ToList();
            }
            catch
            {
                roleMappings = null;
            }
            finally
            {
                contextData = null;
            }
            return roleMappings;

        }
        public List<UserFeatureMappingDto> GetUserFeatureMapping(int userId)
        {
            List<UserFeatureMappingDto> userMappings = new List<UserFeatureMappingDto>();
            var contextData = _unitOfWork.UserFeatureMappingRepository.Context;
            try
            {
                userMappings = (from userFeatureMapping in contextData.UserFeatureMapping
                                where userFeatureMapping.UserId == userId
                                && userFeatureMapping.Active == true
                                select new UserFeatureMappingDto
                                {
                                    Id = userFeatureMapping.Id,
                                    UserId = userFeatureMapping.UserId,
                                    FeatureName = userFeatureMapping.FeatureName
                                }).ToList();
            }
            catch
            {
                userMappings = null;
            }
            finally
            {
                contextData = null;
            }
            return userMappings;
        }
    }
}
