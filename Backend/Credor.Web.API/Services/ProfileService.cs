using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Web.API;
using Credor.Data.Entities;
using Credor.Web.API.Common.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRespository _profileRespository;
        private readonly UnitOfWork _unitOfWork;
        public ProfileService(IProfileRespository profileRespository, 
            IConfiguration configuration)
        {
            _profileRespository = profileRespository;
            _unitOfWork = new UnitOfWork(configuration);
        }
        public List<UserProfileType> GetUserProfileTypes()
        {
            return _profileRespository.GetUserProfileTypes();
        }
        public List<DistributionType> GetDistributionTypes()
        {
            return _profileRespository.GetDistributionTypes();
        }
        public List<StateOrProvince> GetStateOrProvinceList()
        {
            return _profileRespository.GetStateOrProvinceList();
        }
        public int CreateUserProfile(UserProfileDto userProfile)
        {           
            return _profileRespository.CreateUserProfile(userProfile);                                          
        }
        public int UpdateUserProfile(UserProfileDto userProfile)
        {
            return _profileRespository.UpdateUserProfile(userProfile);
        }
        public int DeleteUserProfile(int userid, int userprofileid, int adminUserId)
        {
            return _profileRespository.DeleteUserProfile(userid,userprofileid, adminUserId);
        }
        public UserProfileDto GetUserProfile(int userId, int userProfileId)
        {
            return _profileRespository.GetUserProfile(userId,userProfileId);
        }
        public List<UserProfileDto> GetAllUserProfile(int userId)
        {
            return _profileRespository.GetAllUserProfile(userId);
        }
        public int AddInvestor(InvestorDto invester, int userProfileId)
        {
            return _profileRespository.AddInvestor(invester);
        }
        public int AddInvestor(InvestorDto invester)
        {
            return _profileRespository.AddInvestor(invester);
        }
        public List<InvestorDto> GetInvestors(int userId)
        {
            return _profileRespository.GetInvestors(userId);
        }
        public List<InvestorDto> GetInvestorsByUserProfile(int userprofileId)
        {
            return _profileRespository.GetInvestorsByUserProfile(userprofileId);
        }
    }
}
