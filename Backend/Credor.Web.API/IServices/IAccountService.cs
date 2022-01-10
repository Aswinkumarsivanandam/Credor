using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public interface IAccountService
    {
        UserAccountDto GetUserAccount(int userId);
        Task<int> UpdateUserAccount(UpdateUserAccountDto userAccount);
        int UpdatePassword(UpdatePasswordDto passwordDto);
        Task<bool> AddProfileImage(DocumentModelDto documentDto);
        bool SendOneTimePassword(UserAccountVerificationDto otp, bool isResend);
        bool VerifyUserEmailId(UserAccountVerificationDto emailVerification);
        bool VerifyUserPhoneNumber(UserAccountVerificationDto phonenoVerification);
        Task<bool> AddNewUserAccount(NewUserAccountDto userAccount);
        bool UpdateNewUserAccount(NewUserAccountDto userAccount);
        bool UpdateNewUserAccount(UpdateUserAccountDto newUserAccount);
        bool DeleteNewUserAccount(int currentUserId, int newUserId);
        List<UserPermissionDto> GetAccessGrantedToOthers(int userId);
        List<UserPermissionDto> GetAccessGrantedToUser(int userId);
        bool UpdateNewUserAccountPassword(int userId, string Password);

    }
}
