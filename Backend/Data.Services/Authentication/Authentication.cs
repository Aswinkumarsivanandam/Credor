using Credor.Client.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Credor.Data.Entities;

namespace Credor.Data.Services
{
    public class Authentication : IAuthentication
    {
      
        UserAccountDto IAuthentication.CreateUserAccount(UserAccountDto userAccount)
        {

            using (var context = new ApplicationDbContext())
            {

                var userAccount_temp= new UserAccountDto
                {
                    
                };
                userAccount = userAccount_temp;
                //var user =  Mapper.Map<UserAccountDto,UserAccount>(userAccount);
               // context.UserAccount.Add(userAccount_temp);
                context.SaveChanges();

            }
            return userAccount;
            //throw new NotImplementedException();
        }

        bool IAuthentication.ForgotPassword(UserAccountDto userAccount)
        {
            throw new NotImplementedException();
        }

        UserAccountDto IAuthentication.GetUserAccount(int Id)
        {
            throw new NotImplementedException();
        }

        bool IAuthentication.ResetPassword(UserAccountDto userAccount)
        {
            throw new NotImplementedException();
        }

        UserAccountDto IAuthentication.UpdateUserAccount(UserAccountDto userAccount)
        {
            throw new NotImplementedException();
        }

        UserAccountDto IAuthentication.VerifyUserAccount(string UserId)
        {
            throw new NotImplementedException();
        }
    }
}
