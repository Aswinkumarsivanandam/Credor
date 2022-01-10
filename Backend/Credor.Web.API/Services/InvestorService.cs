using Credor.Web.API.Common.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Web.API.Shared;
using System.Net.Mail;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace Credor.Web.API
{
    public class InvestorService : IInvestorService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IInvestorRepository _investorRepository;
        private readonly IDocumentRepository _documentRepository;

        public IConfiguration _configuration { get; }
        public InvestorService(IInvestorRepository investorRepository, 
                                IDocumentRepository documentRepository,
                                IConfiguration configuration)
        {
            _investorRepository = investorRepository;
            _documentRepository = documentRepository;
            _unitOfWork = new UnitOfWork(configuration);
            _configuration = configuration;
        }
        public UserAccountDto GetUserAccount(int leadId)
        {
            return _investorRepository.GetUserAccount(leadId);
        }
        public List<UserAccountDto> GetAllInvestorAccounts()
        {
            return _investorRepository.GetAllInvestorAccounts();
        }
        public int UpdateInvestorAccount(UserAccountDto userAccountDto)
        {
            return _investorRepository.UpdateInvestorAccount(userAccountDto);
        }
        public bool AddInvestorNotes(UserNotesDto notesDto)
        {
            return _investorRepository.AddInvestorNotes(notesDto);
        }
        public bool UpdateInvestorNotes(UserNotesDto notesDto)
        {
            return _investorRepository.UpdateInvestorNotes(notesDto);
        }
        public bool DeleteInvestorNotes(int adminuserId, int investorId)
        {
            return _investorRepository.DeleteInvestorNotes(adminuserId, investorId);
        }
        public List<UserNotesDto> GetInvestorNotes(int investorId)
        {
            return _investorRepository.GetInvestorNotes(investorId);
        }
        public bool AddInvestorTags(TagDto tag)
        {
            return _investorRepository.AddInvestorTags(tag);
        }
        public bool UpdateInvestorTags(TagDto tag)
        {
            return _investorRepository.UpdateInvestorTags(tag);
        }
        public bool DeleteInvestorTags(int adminUserId, int tagId)
        {
            return _investorRepository.DeleteInvestorTags(adminUserId, tagId);
        }
        public List<TagDto> GetInvestorTags(int adminUserId)
        {
            return _investorRepository.GetInvestorTags(adminUserId);
        }
        public bool ResetPassword(ResetPasswordDto passwordDto)
        {
            return _investorRepository.ResetPassword(passwordDto);
        }
        public async Task<bool> SendResetPasswordLink(ResetPasswordDto passwordDto)
        {
            var contextdata = _unitOfWork.UserAccountRepository.Context;
            var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == passwordDto.UserId && x.Active == true);
                      
            //To send reset password link
            if (userAccount != null)
            {
                try
                {
                    string token = await GenerateJSONWebToken(passwordDto);

                    var env = _configuration["environment"];
                    string mailAddress = _configuration["MailAddress"];
                    string mailDisplayName = _configuration["MailDisplayName"];
                    MailMessage mail = new MailMessage();
                    SmtpClient smtpserver = new SmtpClient("smtp-mail.outlook.com");
                    mail.From = new MailAddress(mailAddress, mailDisplayName);
                    mail.To.Add(userAccount.EmailId);
                    
                    var emailTemplate = _unitOfWork.CredorEmailTemplateRepository.GetWithInclude(x => x.TemplateName.ToLower() == "ForgotPassword".ToLower()).FirstOrDefault();

                    mail.Subject = emailTemplate.Subject;
                    StringBuilder msgBody = new StringBuilder(emailTemplate.BodyContent);

                    msgBody.Replace("{{Name}}", userAccount.FirstName + " "+userAccount.LastName);
                    msgBody.Replace("{{URL}}", "<a href=" + env + "reset-password?token=" + token + "/> Reset Password");
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
        public async Task<string> GenerateJSONWebToken(ResetPasswordDto passwordDto)
        {
            try
            {
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var contextdata = _unitOfWork.UserAccountRepository.Context;

                var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == passwordDto.UserId && x.Active == true);
                AuthUserTokenModelDto userDetails = new AuthUserTokenModelDto();
                if (userAccount != null)
                {
                    userDetails.Name = userAccount.FirstName + " " + userAccount.LastName;
                    userDetails.EmailId = userAccount.EmailId;
                    userDetails.RoleId = userAccount.RoleId != 0 ? userAccount.RoleId : 0;
                    userDetails.Id = userAccount.Id.ToString();                                                                                       
                }
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
            catch (Exception)
            {
                return null;
            }

        }
        public bool AccountVerification(int adminUserId, int investorId,bool isverify)
        {
            var result =  _investorRepository.AccountVerification(adminUserId,investorId, isverify);
            if (result == true && isverify)
            {
                //Send Lead verified mail
                return true;
            }
            else
                return result;
        }
        
        public bool UpdateAccreditation(int adminUserId, int investorId,bool isverify)
        {
            return _investorRepository.UpdateAccreditation(adminUserId, investorId, isverify);
        }
        
        public Task<bool> UploadDocuments(DocumentModelDto documents)
        {
            return _documentRepository.AddDocuments(documents);
        }
        public InvestorSummaryDto GetHeaderElements(int userId)
        {
            return _investorRepository.GetHeaderElements(userId);
        }
        public InvestorsSummaryDto GetInvestorsHeaderElements()
        {
            return _investorRepository.GetInvestorsHeaderElements();
        }
        public List<ReservationDataDto> GetReservations(int investorId)
        {
            return _investorRepository.GetReservations(investorId);
        }
        public List<InvestmentDataDto> GetInvestments(int investorId)
        {
            return _investorRepository.GetInvestments(investorId);
        }
        public List<UserProfileDto> GetAllUserProfile(int userId)
        {
            return _investorRepository.GetAllUserProfile(userId);
        }
        public bool AddReservation(ReservationDataDto reservationDataDto)
        {
            return _investorRepository.AddReservation(reservationDataDto);
        }
        public bool UpdateReservation(ReservationDataDto reservationDataDto)
        {
            return _investorRepository.UpdateReservation(reservationDataDto);
        }
        public bool DeleteReservation(int adminUserId, int reservationId)
        {
            return _investorRepository.DeleteReservation(adminUserId,reservationId);
        }
        public bool AddInvestment(InvestmentDataDto investmentDataDto)
        {
            if(investmentDataDto.eSignedDocument != null)
            {
                DocumentModelDto eSignedDocument = new DocumentModelDto();
                eSignedDocument.AdminUserId = investmentDataDto.AdminUserId;
                eSignedDocument.UserId = investmentDataDto.UserId;
                eSignedDocument.Type = 2;    //Subscription Documents          
                eSignedDocument.Files = investmentDataDto.eSignedDocument;                        
                var filePath = _documentRepository.AddDocument(eSignedDocument);
                if(filePath.Result != null)
                {
                    investmentDataDto.eSignedDocumentPath = filePath.Result;
                }
            }
            return _investorRepository.AddInvestment(investmentDataDto);
        }
        public bool UpdateInvestment(InvestmentDataDto investmentDataDto)
        {
            return _investorRepository.UpdateInvestment(investmentDataDto);
        }
        public bool DeleteInvestment(int adminUserId, int reservationId)
        {
            return _investorRepository.DeleteInvestment(adminUserId, reservationId);
        }
        public AccountStatementDto GetAccountStatement(int investorid)
        {
            return _investorRepository.GetAccountStatement(investorid);
        }
        public List<PortfolioOfferingDto> GetReservationList()
        {
            return _investorRepository.GetReservationList();
        }
        public List<PortfolioOfferingDto> GetOfferingList()
        {
            return _investorRepository.GetOfferingList();
        }
    }
}
