using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Data.Entities;

namespace Credor.Web.API
{
    public interface IAccountRepository
    {
        UserAccountDto GetUserAccount(int userId);
        Task<int> UpdateUserAccount(UpdateUserAccountDto userAccount);
        int UpdatePassword(UpdatePasswordDto passwordDto);
        Task<bool> AddProfileImage(DocumentModelDto documentDto);
        bool UpdateOTP(string OTP, int userId, UserAccount user);
        bool UpdateUserEmailId(int userId, UserAccount user, string emailId);
        bool UpdateUserPhoneNumber(int userId, UserAccount user, string phoneNumber);
        int AddUserAccount(NewUserAccountDto userAccount);
        bool UpdateNewUserAccount(NewUserAccountDto newUserAccount);
        bool DeleteNewUserAccount(int currentUserId, int newUserId);
        List<UserPermissionDto> GetAccessGrantedToOthers(int userId);
        List<UserPermissionDto> GetAccessGrantedToUser(int userId);
        bool UpdateNewUserAccountPassword(UserAccount userAccount);
    }
}
