using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Credor.Web.API.Controllers
{
    [Route("Account")]
    [Produces("application/json")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;        
        public IConfiguration _configuration { get; }

        public AccountController(IAccountService accountService, IConfiguration configuration)
        {            
            _accountService = accountService;
            _configuration = configuration;
           
        }
        [HttpGet]
        [Route("getuseraccount/{userid}")]
        public IActionResult GetUserAccount(int userid)
        {
            var userAccount = _accountService.GetUserAccount(userid);
            return Ok(userAccount);
        }
        [HttpPut]
        [Route("updateuseraccount")]
        public async Task<IActionResult> UpdateUserAccount([FromForm] UpdateUserAccountDto userAccount)
        {
            var result = await _accountService.UpdateUserAccount(userAccount);
            return Ok(result);
        }
        [HttpPut]
        [Route("updatepassword")]
        public IActionResult UpdatePassword(UpdatePasswordDto passwordDto)
        {
            var result = _accountService.UpdatePassword(passwordDto);
            return Ok(result);
        }
        [HttpPut]
        [Route("uploadprofileimage")]
        public async Task<IActionResult> UploadProfileImage([FromForm] DocumentModelDto profileImage)
        {
            var result = await _accountService.AddProfileImage(profileImage);
            return Ok(result);
        }
        [HttpPut]
        [Route("uploadprofileimagetest")]
        public async Task<IActionResult> UploadProfileImagetest(IFormFile profileImage)
        {
            List<IFormFile> filelist = new List<IFormFile>();
            DocumentModelDto dto = new DocumentModelDto();
            dto.UserId = 1;
            dto.Type = 7;
            //dto.Files = filelist;
            //dto.Files.Add(profileImage);
            var result = await _accountService.AddProfileImage(dto);
            return Ok(result);
        }
        [HttpPut]
        [Route("verifyuserphonenumber")]
        public IActionResult VerifyUserPhoneNumber([FromBody] UserAccountVerificationDto phoneVerification)
        {
            var result = _accountService.VerifyUserPhoneNumber(phoneVerification);
            return Ok(result);
        }
        [HttpPut]
        [Route("verifyuseremailid")]
        public IActionResult VerifyUserEmailId([FromBody] UserAccountVerificationDto emailIdVerification)
        {
            var result = _accountService.VerifyUserEmailId(emailIdVerification);         
            return Ok(result);
        }
        [HttpPut]
        [Route("sendonetimepassword")]
        public IActionResult SendOneTimePassword([FromBody]UserAccountVerificationDto otp)
        {
            var result = _accountService.SendOneTimePassword(otp,false);
            return Ok(result);
           
        }
        [HttpPut]
        [Route("resendonetimepassword")]
        public IActionResult ReSendOneTimePassword([FromBody] UserAccountVerificationDto otp)
        {
            var result = _accountService.SendOneTimePassword(otp,true);
            return Ok(result);
        }
        [HttpPost]
        [Route("addnewuseraccount")]
        public async Task<IActionResult> AddNewUserAccount([FromBody] NewUserAccountDto newAccount)
        {
            var result = await _accountService.AddNewUserAccount(newAccount);
            return Ok(result);
        }

        [HttpPut]
        [Route("updatenewuseraccount")]
        public IActionResult UpdateNewUserAccount([FromBody] NewUserAccountDto newAccount)
        {
            var result =  _accountService.UpdateNewUserAccount(newAccount);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deletenewuseraccount/{currentuserid}/{newuserid}")]
        public IActionResult DeleteNewUserAccount(int currentuserid, int newuserid)
        {
            var result =  _accountService.DeleteNewUserAccount(currentuserid,newuserid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getaccessgrantedtoothers/{userid}")]
        public IActionResult GetAccessGrantedToOthers(int userid)
        {
            var result = _accountService.GetAccessGrantedToOthers(userid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getaccessgrantedtouser/{userid}")]
        public IActionResult GetAccessGrantedToUser(int userid)
        {
            var result = _accountService.GetAccessGrantedToUser(userid);
            return Ok(result);
        }
        [HttpPut]
        [Route("updatenewuseraccountpassword")]
        public IActionResult UpdateNewUserAccountPassword([FromBody] AddPasswordDto passwordDto)
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
            ClaimsPrincipal principal = tokenHandler.ValidateToken(passwordDto.Token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            string userId = principal.Identities.Select(x => x.Claims).ToList()[0].ToList()[6].Value.ToString().Trim();
            string emailId = principal.Identities.Select(x => x.Claims).ToList()[0].ToList()[5].Value.ToString().Trim();
            if (userId != null)
            {                
                var result = _accountService.UpdateNewUserAccountPassword(Convert.ToInt32(userId), passwordDto.Password);
                if (result)
                {
                    //Send OTP to EmailId
                    UserAccountVerificationDto userAccountVerification = new UserAccountVerificationDto();
                    userAccountVerification.Id = Convert.ToInt32(userId);
                    userAccountVerification.EmailId = emailId;
                    var isOtpSent = _accountService.SendOneTimePassword(userAccountVerification, false);                    
                    return Ok(isOtpSent);
                }
                return Ok(result);
            }
            else
                return Ok(false);
        }

        [HttpPut]
        [Route("registernewuseraccount")]
        public IActionResult RegisterNewUserAccount(UpdateUserAccountDto userAccount)
        {
            userAccount.IsNewUser = true;
            var result = _accountService.UpdateNewUserAccount(userAccount);
            return Ok(result);
        }
    }
}
