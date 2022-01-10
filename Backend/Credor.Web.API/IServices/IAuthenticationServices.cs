using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Data.Entities;
using Credor.Client.Entities;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public interface IAuthenticationService
    {
        UserAccountDto VerifyUserAccount(string username, string password);
        int CreateUserAccount(UserAccountDto userAccount);       
        UserAccountDto UpdateUserAccount(UserAccountDto userAccount);       
        bool ResetPassword(int id,ResetCredentialDto userCredential);
        Task<string> ForgotPassword(UserCredentialDto userCredential);
        Task<string> GenerateJSONWebToken(UserCredentialDto userCredential, IConfiguration configuration);
        List<UserAccountDto> GetAllUsers();
        public List<StateOrProvince> GetStateOrProvinceList();
        public List<CapacityDto> GetCapacityRanges();
        UserAccountDto GetUserById(int UserId);
        List<RoleFeatureMappingDto> GetRoleFeatureMapping(int roleId);
        List<UserFeatureMappingDto> GetUserFeatureMapping(int userId);
    }
}
