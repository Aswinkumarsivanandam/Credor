using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Data.Entities;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public interface IAuthenticationRepository
    {
        UserAccountDto VerifyUserAccount(string UserName, string Password);
        int CreateUserAccount(UserAccountDto userAccount);        
        UserAccountDto UpdateUserAccount(UserAccountDto userAccount);
        bool ResetPassword(int id, ResetCredentialDto userCredential);
        UserAccount ForgotPassword(UserCredentialDto userCredential);
        List<UserAccountDto> GetAllUsers();
        public List<StateOrProvince> GetStateOrProvinceList();
        public List<CapacityDto> GetCapacityRanges();
        UserAccountDto GetUserById(int UserId);
        List<RoleFeatureMappingDto> GetRoleFeatureMapping(int roleId);
        List<UserFeatureMappingDto> GetUserFeatureMapping(int userId);

    }
}
