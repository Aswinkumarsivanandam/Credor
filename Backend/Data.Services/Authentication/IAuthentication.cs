using System;
using System.Collections.Generic;
using System.Text;
using Credor.Client.Entities;

namespace Credor.Data.Services
{
    public interface IAuthentication
    {
        UserAccountDto  VerifyUserAccount(string UserId);
        UserAccountDto CreateUserAccount(UserAccountDto userAccount);
        UserAccountDto GetUserAccount(int Id);
        UserAccountDto UpdateUserAccount(UserAccountDto userAccount);

        bool ResetPassword(UserAccountDto userAccount);

        bool ForgotPassword(UserAccountDto userAccount);
    }
}
