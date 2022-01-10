using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Data.Entities;

namespace Credor.Web.API
{
    public interface IProfileService
    {
        public List<UserProfileType> GetUserProfileTypes();
        public List<DistributionType> GetDistributionTypes();
        public List<StateOrProvince> GetStateOrProvinceList();
        public int CreateUserProfile(UserProfileDto userProfile);
        public int UpdateUserProfile(UserProfileDto userProfile);
        public int DeleteUserProfile(int userid, int userprofileid, int adminuserid = 0);
        public UserProfileDto GetUserProfile(int userId,int userProfileId);
        public List<UserProfileDto> GetAllUserProfile(int userId);
        public int AddInvestor(InvestorDto invester);
        public List<InvestorDto> GetInvestors(int userId);
        public List<InvestorDto> GetInvestorsByUserProfile(int userId);        
    }
}
