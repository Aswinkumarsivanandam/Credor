using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Web.API;
using Microsoft.Extensions.Configuration;
using Credor.Client.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Credor.Web.API.Controllers
{
    [Route("Authentication")]
    [Produces("application/json")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public IConfiguration _configuration { get; }

        public AuthenticationController(IAuthenticationService authenticationService,
                                        IConfiguration configuration
                                        )
        {
            _configuration = configuration;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] UserAccountDto userAccount)
        {
            var status = _authenticationService.CreateUserAccount(userAccount);
            if (status == 1)
            {
                return Ok(new { StatusCode = status, Message = "Registration Successfull" });
            }
            else if (status == 2)
            {
                return Ok(new { StatusCode = status, Message = "Invalid Data" });
            }
            else if (status == 3)
            {
                return Ok(new { StatusCode = status, Message = "Active/Approved Account Already Exists" });
            }
            else
                return Ok(status);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentialDto userCredential)
        {

            string username = userCredential.UserName;
            if (string.IsNullOrEmpty(username))
            {
                username = userCredential.EmailId;
            }
            if (username.Trim() != "" && userCredential.Password.Trim() != "")
            {
                UserAccountDto userAccount;
                userAccount = _authenticationService.VerifyUserAccount(username, userCredential.Password);
                if (userAccount != null)
                {
                     var tokenString = await _authenticationService.GenerateJSONWebToken(userCredential, _configuration);
                     if (tokenString != null)
                     {
                         return Ok(new { AccessToken = tokenString, Data = userAccount });
                     }
                     else
                     {
                        return Ok(new { StatusCode = 0, Message = "Login Failed" });
                    }                     
                }
                else
                {
                    return Ok(new { StatusCode = 0, Message = "Invalid Credentials" }); 
                }                                   
                               
            }

            return Ok(new { StatusCode = 0, Message = "Invalid Credentials" }); ;

        }

        [HttpPut]
        [Route("forgotpassword")]
        public IActionResult ForgotPassword([FromBody] UserCredentialDto userCredential)
        {
            var result = _authenticationService.ForgotPassword(userCredential).Result;
            if (result == "1")
            {
                return Ok(new { Message = "Send Email Failure", Data = result });
            }
            if (result == "0")
            {
                return Ok(new { Messgae = "Account Not Exists", Data = result });
            }
            return Ok(new { Data = result });                             
        }

        [HttpPut]
        [Route("resetpassword")]
        public IActionResult ResetExistingPassword([FromBody] ResetCredentialDto userCredential)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var claims = from c in User.Claims select new { c.Type, c.Value };
            ClaimsPrincipal principal = tokenHandler.ValidateToken(userCredential.Token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            string userId = principal.Identities.Select(x => x.Claims).ToList()[0].ToList()[4].Value.ToString().Trim();
            //var userId = _userService.GetUs
            var existingUser = _authenticationService.ResetPassword(Convert.ToInt32(userId),userCredential);
            return Ok(existingUser);
        }


        [HttpPut]        
        [Route("updateuseraccount")]
        public IActionResult EditUserAccount([FromBody] UserAccountDto userAccount)
        {
            var updatedStatus = _authenticationService.UpdateUserAccount(userAccount);
            return Ok(updatedStatus);
        }

        [HttpGet]
        [Route("getallusers")]
        public IActionResult GetAllUsers()
        {
            var userList = _authenticationService.GetAllUsers();
            return Ok(userList);
        }
        [HttpGet]
        [Route("getuserbyid/{userid}")]
        public IActionResult GetUserById(int userid)
        {
            var userList = _authenticationService.GetUserById(userid);
            return Ok(userList);
        }

        [HttpGet]
        [Route("getstateorprovincelist")]
        public IActionResult GetStateOrProvinceList()
        {
            var userProfileTypes = _authenticationService.GetStateOrProvinceList();
            return Ok(userProfileTypes);
        }
        [HttpGet]
        [Route("getcapacityranges")]
        public IActionResult GetCapacityRanges()
        {
            var ranges = _authenticationService.GetCapacityRanges();
            return Ok(ranges);
        }
        [HttpGet]
        [Route("getrolefeaturemapping/{roleid}")]
        public IActionResult GetRoleFeatureMapping(int roleid)
        {
            var mappings = _authenticationService.GetRoleFeatureMapping(roleid);
            return Ok(mappings);
        }
        [HttpGet]
        [Route("getuserfeaturemapping/{userid}")]
        public IActionResult GetUserFeatureMapping(int userid)
        {
            var mappings = _authenticationService.GetUserFeatureMapping(userid);
            return Ok(mappings);
        }
    }
}
