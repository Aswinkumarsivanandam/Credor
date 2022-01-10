using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Web.API;
using Microsoft.Extensions.Configuration;
using Credor.Client.Entities;

namespace Credor.Web.API.Controllers
{
    [Route("Profile")]
    [Produces("application/json")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public IConfiguration _configuration { get; }

        public ProfileController(IProfileService profileService,
                                        IConfiguration configuration
                                        )
        {
            _configuration = configuration;
            _profileService = profileService;
        }

        [HttpGet]
        [Route("getuserprofiletypes")]
        public IActionResult GetUserProfileTypes()
        {
            var userProfileTypes = _profileService.GetUserProfileTypes();
            return Ok(userProfileTypes);
        }
        [HttpGet]
        [Route("getdistributiontypes")]
        public IActionResult GetDistributionTypes()
        {
            var distributionTypes = _profileService.GetDistributionTypes();
            return Ok(distributionTypes);
        }
        [HttpGet]
        [Route("getstateorprovincelist")]
        public IActionResult GetStateOrProvinceList()
        {
            var stateOrProvincelist = _profileService.GetStateOrProvinceList();
            return Ok(stateOrProvincelist);
        }
        [HttpPost]
        [Route("createuserprofile")]
        public IActionResult CreateUserProfile([FromBody]UserProfileDto userProfile)
        {
            var result = _profileService.CreateUserProfile(userProfile);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateuserprofile")]
        public IActionResult UpdateUserProfile([FromBody] UserProfileDto userProfile)
        {
            var result = _profileService.UpdateUserProfile(userProfile);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deleteuserprofile/{userid}/{userprofileid}")]
        public IActionResult DeleteUserProfile(int userid, int userprofileid)
        {
            var result = _profileService.DeleteUserProfile(userid,userprofileid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getuserprofile/{userid}/{userprofileid}")]
        public IActionResult GetUserProfile(int userid, int userprofileid)
        {
            var result = _profileService.GetUserProfile(userid,userprofileid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getalluserprofile/{userid}")]
        public IActionResult GetAllUserProfile(int userid)
        {
            var result = _profileService.GetAllUserProfile(userid);
            return Ok(result);
        }
        [HttpPut]
        [Route("addinvestor")]
        public IActionResult AddInvestor([FromBody]InvestorDto invester)
        {
            var result = _profileService.AddInvestor(invester);
            return Ok(result);
        }
        [HttpGet]
        [Route("getallinvestors/{userid}")]
        public IActionResult GetInvestors(int userid)
        {
            var result = _profileService.GetInvestors(userid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getinvestorsbyuserprofile/{userprofileid}")]
        public IActionResult GetInvestorsByUserProfile(int userprofileid)
        {
            var result = _profileService.GetInvestorsByUserProfile(userprofileid);
            return Ok(result);
        }
    }
}
