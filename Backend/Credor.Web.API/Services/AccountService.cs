using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Web.API.Common.UnitOfWork;
using Credor.Web.API.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Credor.Web.API
{
    public class AccountService : IAccountService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;

        public IConfiguration _configuration { get; }
        public AccountService(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _unitOfWork = new UnitOfWork(configuration);
            _configuration = configuration;
        }
        public UserAccountDto GetUserAccount(int userId)
        {
            return _accountRepository.GetUserAccount(userId);
        }
        public List<UserPermissionDto> GetAccessGrantedToOthers(int userId)
        {
            return _accountRepository.GetAccessGrantedToOthers(userId);
        }
        public List<UserPermissionDto> GetAccessGrantedToUser(int userId)
        {
            return _accountRepository.GetAccessGrantedToUser(userId);
        }
        public Task<int> UpdateUserAccount(UpdateUserAccountDto userAccount)
        {
            return _accountRepository.UpdateUserAccount(userAccount);                        
        }
        public int UpdatePassword(UpdatePasswordDto passwordDto)
        {
            return _accountRepository.UpdatePassword(passwordDto);
        }
        public Task<bool> AddProfileImage(DocumentModelDto documentDto)
        {
            return _accountRepository.AddProfileImage(documentDto);
        }
        public bool SendOneTimePassword(UserAccountVerificationDto otp, bool isResend)
        {                       
            var contextdata = _unitOfWork.UserAccountRepository.Context;
            var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == otp.Id && x.Active == true);

            Helper _helper = new Helper(_configuration);
            string OTP = "";
            if (isResend)            
                OTP = userAccount.OneTimePassword;                           
            else 
                OTP = _helper.GetRandomOTP();
            //To send OTP to new mailId for Email Verification
            if (userAccount !=null && !string.IsNullOrEmpty(otp.EmailId))
            {
                try
                {
                    var env = _configuration["environment"];
                    string mailAddress = _configuration["MailAddress"];
                    string mailDisplayName = _configuration["MailDisplayName"];
                    MailMessage mail = new MailMessage();
                    SmtpClient smtpserver = new SmtpClient("smtp-mail.outlook.com");
                    mail.From = new MailAddress(mailAddress, mailDisplayName);
                    mail.To.Add(otp.EmailId);

                    var emailTemplate = _unitOfWork.CredorEmailTemplateRepository.GetWithInclude(x => x.TemplateName.ToLower() == "VerifyEmailOTP".ToLower()).FirstOrDefault();
                   
                    StringBuilder msgSubject = new StringBuilder(emailTemplate.Subject);
                    msgSubject.Replace("{{OTP}}", OTP);
                    mail.Subject = msgSubject.ToString();
                    
                    StringBuilder msgBody = new StringBuilder(emailTemplate.BodyContent);

                    
                    msgBody.Replace("{{Name}}", userAccount.FirstName + " " + userAccount.LastName);                    
                    msgBody.Replace("{{OTP}}", OTP);
                    mail.Body = msgBody.ToString();
                   
                    var result = _helper.MailSend(mail, smtpserver);
                    if (result)
                    {
                        if (!isResend)
                        {
                            var status = _accountRepository.UpdateOTP(OTP, otp.Id, userAccount);
                            return status;
                        }
                        else
                            return true; // Resend OTP Email Success
                    }
                    else return false;//Email send failure

                }
                catch (Exception e)
                {
                    e.ToString();
                    return false;
                }
            }
            //To send OTP to phone number
            else if(userAccount !=null && !string.IsNullOrEmpty(otp.PhoneNumber))
            {               
                //To do: Sending OTP to phone number
                var status = _accountRepository.UpdateOTP(OTP, otp.Id, userAccount);
                return status;
            }
            return false;
        }
        public bool VerifyUserEmailId(UserAccountVerificationDto emailVerification)
        {
            var contextdata = _unitOfWork.UserAccountRepository.Context;
            var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == emailVerification.Id && x.Active == true);

            if(userAccount!=null)
            {
                if (userAccount.OneTimePassword == emailVerification.OneTimePassword)
                {
                    var result = _accountRepository.UpdateUserEmailId(emailVerification.Id, userAccount, emailVerification.EmailId);
                    if (result)
                    {
                        //Send Email Verified mail
                        try
                        {
                            var env = _configuration["environment"];
                            string mailAddress = _configuration["MailAddress"];
                            string mailDisplayName = _configuration["MailDisplayName"];
                            MailMessage mail = new MailMessage();
                            SmtpClient smtpserver = new SmtpClient("smtp-mail.outlook.com");
                            mail.From = new MailAddress(mailAddress, mailDisplayName);
                            mail.To.Add(emailVerification.EmailId);

                            var emailTemplate = _unitOfWork.CredorEmailTemplateRepository.GetWithInclude(x => x.TemplateName.ToLower() == "EmailVerified".ToLower()).FirstOrDefault();

                            mail.Subject = emailTemplate.Subject;
                            StringBuilder msgBody = new StringBuilder(emailTemplate.BodyContent);

                            msgBody.Replace("{{Name}}", userAccount.FirstName + " " + userAccount.LastName);
                            msgBody.Replace("{{EmailId}}", emailVerification.EmailId);                            
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
                }
                else
                    return false;
            }
            return false;
        }
        public bool VerifyUserPhoneNumber(UserAccountVerificationDto phonenoVerification)
        {
            var contextdata = _unitOfWork.UserAccountRepository.Context;
            var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == phonenoVerification.Id && x.Active == true);

            if (userAccount != null)
            {
                if (userAccount.OneTimePassword == phonenoVerification.OneTimePassword)
                {
                    return _accountRepository.UpdateUserPhoneNumber(phonenoVerification.Id, userAccount, phonenoVerification.PhoneNumber);
                }
                else
                    return false;
            }
            return false;
        }
        public async Task<bool> AddNewUserAccount(NewUserAccountDto newAccount)
        {
            var token = "";
            var userId = _accountRepository.AddUserAccount(newAccount);
            if (userId != 0)//Send Invite email
            {
                var contextdata = _unitOfWork.UserAccountRepository.Context;
                var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == userId && x.Active == true);
                if(userAccount != null)
                {
                    newAccount.Id = userAccount.Id;
                    token = await GenerateJSONWebToken(newAccount, _configuration);
                }
                //Send Join Invite mail
                try
                {
                    var env = _configuration["environment"];
                    string mailAddress = _configuration["MailAddress"];
                    string mailDisplayName = _configuration["MailDisplayName"];
                    MailMessage mail = new MailMessage();
                    SmtpClient smtpserver = new SmtpClient("smtp-mail.outlook.com");
                    mail.From = new MailAddress(mailAddress, mailDisplayName);
                    mail.To.Add(newAccount.EmailId);

                    var emailTemplate = _unitOfWork.CredorEmailTemplateRepository.GetWithInclude(x => x.TemplateName.ToLower() == "JoinInvite".ToLower()).FirstOrDefault();

                    mail.Subject = emailTemplate.Subject;
                    StringBuilder msgBody = new StringBuilder(emailTemplate.BodyContent);

                    msgBody.Replace("{{NewUserName}}", newAccount.FirstName + " " + newAccount.LastName);
                    msgBody.Replace("{{CurrentUserName}}", userAccount.FirstName + " " + userAccount.LastName);
                    msgBody.Replace("{{SignUpURL}}", "<a href=" + env + "signup/" + userId + "?token=" + token  + "/> Click Here to Sign-Up");                    
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
            else
                return false;
        }
        public async Task<string> GenerateJSONWebToken(NewUserAccountDto newUserAccount, IConfiguration configuration)
        {
            try
            {
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                SignUpUserTokenModelDto signUpDetails = new SignUpUserTokenModelDto();
                signUpDetails.FirstName = newUserAccount.FirstName;
                signUpDetails.LastName = newUserAccount.LastName;
                signUpDetails.EmailId = newUserAccount.EmailId;
                signUpDetails.NickName = newUserAccount.NickName;
                signUpDetails.RoleId = 4;// New User
                signUpDetails.Permission = newUserAccount.Permission;
                signUpDetails.UserId = newUserAccount.Id;
                    var UserClaims = new Claim[]
                              {
                            new Claim(JwtRegisteredClaimNames.Sub, signUpDetails.EmailId),
                            new Claim("FirstName", signUpDetails.FirstName),
                            new Claim("LastName", signUpDetails.LastName),
                            new Claim("RoleId",  signUpDetails.RoleId.ToString()),
                            new Claim("Permission",signUpDetails.Permission.ToString()),
                            new Claim("EmailId",signUpDetails.EmailId),
                            new Claim("Id", signUpDetails.UserId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Convert.ToString(Guid.NewGuid()))
                              };

                    JwtSecurityToken token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                           _configuration["Jwt:Issuer"],
                           UserClaims,
                           signingCredentials: credentials);

                    return await Task.FromResult<string>(new JwtSecurityTokenHandler().WriteToken(token));                
            }
            catch (Exception)
            {
                return null;
            }

        }
        public bool UpdateNewUserAccount(NewUserAccountDto newUserAccount)
        {
            var result = _accountRepository.UpdateNewUserAccount(newUserAccount);
            if(result)
            {
                //Send Permission Updated mail
                try
                {
                    var env = _configuration["environment"];
                    string mailAddress = _configuration["MailAddress"];
                    string mailDisplayName = _configuration["MailDisplayName"];
                    MailMessage mail = new MailMessage();
                    SmtpClient smtpserver = new SmtpClient("smtp-mail.outlook.com");
                    mail.From = new MailAddress(mailAddress, mailDisplayName);
                    mail.To.Add(newUserAccount.EmailId);

                    var emailTemplate = _unitOfWork.CredorEmailTemplateRepository.GetWithInclude(x => x.TemplateName.ToLower() == "PermissionUpdate".ToLower()).FirstOrDefault();

                    mail.Subject = emailTemplate.Subject;
                    StringBuilder msgBody = new StringBuilder(emailTemplate.BodyContent);

                    msgBody.Replace("{{NewUserName}}", newUserAccount.FirstName + " " + newUserAccount.LastName);
                    msgBody.Replace("{{CurrentUserName}}", newUserAccount.FirstName + " " + newUserAccount.LastName);
                    
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
            return false;
        }
        public bool UpdateNewUserAccount(UpdateUserAccountDto newUserAccount)
        {
            var result = _accountRepository.UpdateUserAccount(newUserAccount);
            if (result.Result == 1)
            {
                return true;
            }
            return false;
        }
        public bool DeleteNewUserAccount(int currentUserId, int newUserId)
        {
            return _accountRepository.DeleteNewUserAccount(currentUserId, newUserId);
        }
        public bool UpdateNewUserAccountPassword(int userId, string Password)
        {
            var contextdata = _unitOfWork.UserAccountRepository.Context;
            var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == userId && x.Active == true);
            userAccount.Password = Password;

            var result = _accountRepository.UpdateNewUserAccountPassword(userAccount);
            if(result)
            {
                //Send Access provided mail                
                try
                {
                    var env = _configuration["environment"];
                    string mailAddress = _configuration["MailAddress"];
                    string mailDisplayName = _configuration["MailDisplayName"];
                    MailMessage mail = new MailMessage();
                    SmtpClient smtpserver = new SmtpClient("smtp-mail.outlook.com");
                    mail.From = new MailAddress(mailAddress, mailDisplayName);
                    mail.To.Add(userAccount.EmailId);

                    var emailTemplate = _unitOfWork.CredorEmailTemplateRepository.GetWithInclude(x => x.TemplateName.ToLower() == "AccessUpdate".ToLower()).FirstOrDefault();

                    mail.Subject = emailTemplate.Subject;
                    StringBuilder msgBody = new StringBuilder(emailTemplate.BodyContent);

                    msgBody.Replace("{{Name}}", userAccount.FirstName + " " + userAccount.LastName);
                    msgBody.Replace("{{URL}}", "https://techtownequity.invportal.com/login");

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
            return result;
        }
    }
}
