using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Web.API;
using Credor.Web.API.Common.UnitOfWork;
using Credor.Data.Entities;
using Credor.Web.API.Shared;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using System.Net.Mail;

namespace Credor.Web.API
{
    public class AuthenticationService : IAuthenticationService
    {
       private readonly UnitOfWork _unitOfWork;
        private readonly IAuthenticationRepository _authenticationRepository;

        public IConfiguration _configuration { get; }
        public AuthenticationService(IAuthenticationRepository authenticationRepository,IConfiguration configuration)
        {
            _authenticationRepository = authenticationRepository;
            _unitOfWork = new UnitOfWork(configuration);
            _configuration = configuration;
        }
        public List<CapacityDto> GetCapacityRanges()
        {
            return _authenticationRepository.GetCapacityRanges();
        }
        public int CreateUserAccount(UserAccountDto userAccount)
        {
           return _authenticationRepository.CreateUserAccount(userAccount);       
        }

        public async Task<string> ForgotPassword(UserCredentialDto userCredential)
        {
            UserAccount userAccount = _authenticationRepository.ForgotPassword(userCredential);
            if (userAccount != null)
            {               
                string token = await GenerateJSONWebToken(userCredential, _configuration);
                var status = SendEmail(userAccount.EmailId, userAccount.FirstName + ' ' + userAccount.LastName, token, userCredential.EncodedDate);
                if (status)
                {
                    return userAccount.EmailId;//Success
                }
                else
                    return "1"; //Send Email Failure                                                
            }
            else
                return "2";//Account Not Exists
        }

        public UserAccountDto GetUserAccount(int Id)
        {
            throw new NotImplementedException();
        }

       public bool ResetPassword(int id,ResetCredentialDto userCredential)
        {
            return _authenticationRepository.ResetPassword(id,userCredential);
            
        }
     
        public UserAccountDto UpdateUserAccount(UserAccountDto userAccount)
        {
            throw new NotImplementedException();
        }

        public UserAccountDto VerifyUserAccount(string UserName, string Password)
        {
            return _authenticationRepository.VerifyUserAccount(UserName, Password);
        }

        public List<UserAccountDto> GetAllUsers()
        {
           return _authenticationRepository.GetAllUsers();                             
        }


        public async Task<string> GenerateJSONWebToken(UserCredentialDto userCredential, IConfiguration configuration)
        {
            try
            {
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var contextdata = _unitOfWork.UserAccountRepository.Context;

                var userAccount = _unitOfWork.UserAccountRepository.Get(x=> x.EmailId == userCredential.EmailId && x.Active == true && x.Status == 1);
                AuthUserTokenModelDto userDetails = new AuthUserTokenModelDto();                
                if (userAccount != null)
                {
                    userDetails = (from u in contextdata.UserAccount
                                   where (u.EmailId.ToLower() == userCredential.EmailId.ToLower())
                                   select new AuthUserTokenModelDto
                                   {
                                       Name = u.FirstName != null && u.LastName != null ? u.FirstName.ToString() + " " + u.LastName.ToString() : "",
                                       EmailId = u.EmailId != null ? u.EmailId.ToString() : "",
                                       RoleId = u.RoleId != 0 ? u.RoleId : 0,
                                       Id = u.Id.ToString(),
                                   }).FirstOrDefault();
                }              
                if (userCredential.Password != null)
                {
                    var UserClaims = new Claim[]
                                  {
                            new Claim(JwtRegisteredClaimNames.Sub, userCredential.EmailId),
                            new Claim(JwtRegisteredClaimNames.Sub, userCredential.Password),
                            new Claim("Name", userDetails.Name != null ? userDetails.Name:""),
                            new Claim("EmailId", userDetails.EmailId != null ?Convert.ToString( userDetails.EmailId): ""),                           
                            new Claim("RoleId",  userDetails.RoleId.ToString()),
                            new Claim("Id", userDetails.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Convert.ToString(Guid.NewGuid()))
                                  };

                    JwtSecurityToken token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                           _configuration["Jwt:Issuer"],
                           UserClaims,
                           signingCredentials: credentials);

                    return await Task.FromResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    var UserClaims = new Claim[]
                              {
                            new Claim(JwtRegisteredClaimNames.Sub, userDetails.Name),
                            new Claim("Name", userDetails.Name != null? userDetails.Name.ToString() : ""),
                            new Claim("EmailId", userDetails.EmailId != null ?Convert.ToString( userDetails.EmailId): ""),                           
                            new Claim("RoleId",  userDetails.RoleId.ToString()),
                            new Claim("Id", userDetails.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Convert.ToString(Guid.NewGuid()))
                              };

                    JwtSecurityToken token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                           _configuration["Jwt:Issuer"],
                           UserClaims,
                           signingCredentials: credentials);

                    return await Task.FromResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

       private bool SendEmail(string EmailId, string name, string token, string EncodeDate)
        {
            try
            {
                var env = _configuration["environment"];
                string mailAddress = _configuration["MailAddress"];
                string mailDisplayName = _configuration["MailDisplayName"];
                string SmtpClientName = _configuration["SmtpClientName"];
                MailMessage mail = new MailMessage();
                SmtpClient smtpserver = new SmtpClient(SmtpClientName);                
                mail.From = new MailAddress(mailAddress, mailDisplayName);
                mail.To.Add(EmailId);

                var emailTemplate = _unitOfWork.CredorEmailTemplateRepository.GetWithInclude(x => x.TemplateName.ToLower() == "ForgotPassword".ToLower()).FirstOrDefault();

                mail.Subject = emailTemplate.Subject;
                StringBuilder msgBody = new StringBuilder(emailTemplate.BodyContent);

                msgBody.Replace("{{Name}}", name);
                msgBody.Replace("{{URL}}", "<a href=" + env + "reset-password?token=" + token + "&expiry=" + EncodeDate + "/> Reset Password");
                mail.Body = msgBody.ToString();
                Helper _helper = new Helper(_configuration);
                return _helper.MailSend(mail, smtpserver);
                
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }
        }
       public List<StateOrProvince> GetStateOrProvinceList()
        {
            return _authenticationRepository.GetStateOrProvinceList();
        }
       public UserAccountDto GetUserById(int userId)
        {
            return _authenticationRepository.GetUserById(userId);
        }
        public List<RoleFeatureMappingDto> GetRoleFeatureMapping(int roleId)
        {
            return _authenticationRepository.GetRoleFeatureMapping(roleId);
        }
        public List<UserFeatureMappingDto> GetUserFeatureMapping(int userId)
        {
            return _authenticationRepository.GetUserFeatureMapping(userId);
        }
    }
}
