using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Credor.Client.Entities;
using Credor.Data.Entities;
using Credor.Web.API.Common;
using Credor.Web.API.Common.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class LeadRepository: ILeadRepository
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IHubContext<NotificationHub> _hubContext;
        //ResourceManager rm = new ResourceManager("Credor.Web.API.Resource", Assembly.GetExecutingAssembly());
        public LeadRepository(IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = new UnitOfWork(configuration);          
            //_hubContext = context;
            _mapper = mapper;
        }
        public UserAccountDto GetUserAccount(int leadId)
        {
            UserAccountDto userAccount = new UserAccountDto();
            var contextData = _unitOfWork.UserAccountRepository.Context;
            try
            {
                userAccount = (from user in contextData.UserAccount
                               where user.Id == leadId
                               && user.RoleId == 2 //Lead
                               && user.Active == true
                               select new UserAccountDto
                               {
                                   Id = user.Id,
                                   FirstName = user.FirstName,
                                   LastName = user.LastName,
                                   NickName = user.NickName,
                                   EmailId = user.EmailId,
                                   SecondaryEmail = user.SecondaryEmail,
                                   ReceiveEmailNotifications = user.ReceiveEmailNotifications,
                                   DateOfBirth = user.DateOfBirth,
                                   ProfileImageUrl = user.ProfileImageUrl,
                                   Capacity = user.Capacity,
                                   HeardFrom = user.HeardFrom,
                                   PhoneNumber = user.PhoneNumber,
                                   Status = user.Status,
                                   Residency = user.Residency,
                                   Country = user.Country,
                                   IsAccreditedInvestor = user.IsAccreditedInvestor,
                                   IsEmailVerified = user.IsEmailVerified,
                                   IsPhoneVerified = user.IsPhoneVerified,
                                   IsTOCApproved = user.IsTOCApproved,
                                   IsTwoFactorAuthEnabled = user.IsTwoFactorAuthEnabled,
                                   CreatedBy = user.CreatedBy,
                                   CreatedOn = user.CreatedOn,
                                   PasswordChangedOn = user.PasswordChangedOn,
                                   VerifyAccount = user.VerifyAccount,
                                   CompanyNewsLetterUpdates = user.CompanyNewsLetterUpdates,
                                   NewInvestmentAnnouncements = user.NewInvestmentAnnouncements,
                                   Notes = (from notes in contextData.UserNotes
                                            where notes.UserId == leadId
                                            && notes.Active == true
                                            select new UserNotesDto
                                            {
                                                Id = notes.Id,
                                                UserId = notes.UserId,
                                                Notes = notes.Notes,
                                                CreatedBy = notes.CreatedBy,
                                                CreatedOn = notes.CreatedOn,
                                                ModifiedBy = notes.ModifiedBy,
                                                ModifiedOn = notes.ModifiedOn
                                            }).FirstOrDefault()                                
                                }).FirstOrDefault();               
            }
            catch
            {
                userAccount = null;
            }
            finally
            {
                contextData = null;
            }
            return userAccount;
        }
        public List<UserAccountDto> GetAllLeadAccounts()
        {
            List<UserAccountDto> userAccounts = new List<UserAccountDto>();
            var contextData = _unitOfWork.UserAccountRepository.Context;
            try
            {
                userAccounts = (from user in contextData.UserAccount
                               where user.RoleId == 2 //Lead
                               && user.Active == true
                               select new UserAccountDto
                               {
                                   Id = user.Id,
                                   FirstName = user.FirstName,
                                   LastName = user.LastName,
                                   NickName = user.NickName,
                                   FullName = user.FirstName + " " + user.LastName,
                                   EmailId = user.EmailId,
                                   SecondaryEmail = user.SecondaryEmail,
                                   ReceiveEmailNotifications = user.ReceiveEmailNotifications,
                                   DateOfBirth = user.DateOfBirth,
                                   ProfileImageUrl = user.ProfileImageUrl,
                                   Capacity = user.Capacity,
                                   HeardFrom = user.HeardFrom,
                                   PhoneNumber = user.PhoneNumber,
                                   Status = user.Status,
                                   Residency = user.Residency,
                                   Country = user.Country,
                                   IsAccreditedInvestor = user.IsAccreditedInvestor,
                                   IsEmailVerified = user.IsEmailVerified,
                                   IsPhoneVerified = user.IsPhoneVerified,
                                   IsTOCApproved = user.IsTOCApproved,
                                   IsTwoFactorAuthEnabled = user.IsTwoFactorAuthEnabled,
                                   CreatedBy = user.CreatedBy,
                                   CreatedOn = user.CreatedOn,
                                   PasswordChangedOn = user.PasswordChangedOn,
                                   VerifyAccount = user.VerifyAccount,
                                   CompanyNewsLetterUpdates = user.CompanyNewsLetterUpdates,
                                   NewInvestmentAnnouncements = user.NewInvestmentAnnouncements,
                                   LastLogin = user.LastLogin                                   
                               }).ToList();
            }
            catch
            {
                userAccounts = null;
            }
            finally
            {
                contextData = null;
            }
            return userAccounts;
        }
        public bool AddLeadAccount(UserAccountDto userAccountDto)
        {
            try
            {
                using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                {
                    UserAccount account = _mapper.Map<UserAccount>(userAccountDto);
                    account.RoleId = 2; //Lead
                    if(userAccountDto.VerifyAccount == true)
                    {
                        account.IsEmailVerified = true;
                        account.IsPhoneVerified = true;
                    }
                    account.CreatedBy = userAccountDto.AdminUserId.ToString();
                    account.CreatedOn = DateTime.Now;
                    _unitOfWork.UserAccountRepository.Insert(account);
                    _unitOfWork.Save();
                    transaction.Commit();
                    var userId = account.Id;
                    return true;
                }
            }
            catch(Exception e)
            {
                e.ToString();
                return false;
            }                        
        }
        public int UpdateLeadAccount(UserAccountDto userAccountDto)
        {
            var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == userAccountDto.Id);
            if (userAccount != null)
            {
                try
                {
                    using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                    {
                        UserAccount userAccountData = userAccount;
                        userAccountData.FirstName = userAccountDto.FirstName;
                        userAccountData.LastName = userAccountDto.LastName;
                        userAccountData.NickName = userAccountDto.NickName;
                        userAccountData.PhoneNumber = userAccountDto.PhoneNumber;
                        userAccountData.EmailId = userAccountDto.EmailId;
                        userAccountData.IsEmailVerified = userAccountDto.IsEmailVerified;
                        userAccountData.Residency = userAccountDto.Residency;
                        userAccountData.PhoneNumber = userAccountDto.PhoneNumber;
                        userAccountData.IsPhoneVerified = userAccountDto.IsPhoneVerified;
                        userAccountData.IsAccreditedInvestor = userAccountDto.IsAccreditedInvestor;
                        userAccountData.Capacity = userAccountDto.Capacity;
                        userAccountData.HeardFrom = userAccountDto.HeardFrom;
                        userAccountData.SecondaryEmail = userAccountDto.SecondaryEmail;
                        userAccountData.ReceiveEmailNotifications = userAccountDto.ReceiveEmailNotifications;
                        userAccountData.DateOfBirth = userAccountDto.DateOfBirth;
                        userAccountData.IsTwoFactorAuthEnabled = userAccountDto.IsTwoFactorAuthEnabled;
                        userAccountData.VerifyAccount = userAccountDto.VerifyAccount;
                        userAccountData.CompanyNewsLetterUpdates = userAccountDto.CompanyNewsLetterUpdates;
                        userAccountData.NewInvestmentAnnouncements = userAccountDto.NewInvestmentAnnouncements;
                        userAccountData.ModifiedBy = userAccountDto.AdminUserId.ToString();                          
                        _unitOfWork.UserAccountRepository.Update(userAccountData);
                        _unitOfWork.Save();
                        transaction.Commit();
                    }
                    return 1; //Success
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                    return 0;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return 0;//Failure;
            }
        public int DeleteLeadAccount(int adminuserId, int leadId)
        {
            var adminAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == adminuserId);
            if (adminAccount != null && adminAccount.RoleId == 3 && adminAccount.Active == true) //Admin user                
            {
                try
                {
                    var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == leadId);
                    using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                    {                        
                        UserAccount userAccountData = userAccount;
                        userAccountData.Active = false ;
                        userAccountData.ModifiedBy = adminuserId.ToString();
                        _unitOfWork.UserAccountRepository.Update(userAccountData);
                        _unitOfWork.Save();
                        transaction.Commit();
                    }
                    return 1; //Success
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                    return 0;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return 0;//Failure;
        }
        public List<UserAccountDto> AddLeadAccounts(IFormFile bulkInsertFile)
        {
            return null;
        }
        public LeadSummary GetLeadSummary()
        {
            LeadSummary elements = new LeadSummary();
            var contextData = _unitOfWork.UserAccountRepository.Context;
            try
            {
                elements.TotalLeads = (from lead in contextData.UserAccount
                                              where lead.RoleId == 2 // Lead Role
                                              && lead.Active == true //Active Leads                                         
                                              select lead.Id).Count();

                elements.AccreditedLeads = (from lead in contextData.UserAccount
                                            where lead.RoleId == 2 // Lead Role
                                            && lead.Active == true //Active Leads      
                                            && lead.IsAccreditedInvestor == true //Accredited Account
                                               select lead.Id).Count();

                elements.VerifiedLeads = (from lead in contextData.UserAccount
                                            where lead.RoleId == 2 // Lead Role
                                            && lead.Active == true //Active Leads      
                                            && (lead.IsEmailVerified == true //Account verified via Email
                                             || lead.IsPhoneVerified) // Account verified via text code
                                               select lead.Id).Count();

                elements.UnverifiedLeads = (from lead in contextData.UserAccount
                                            where lead.RoleId == 2 // Lead Role
                                            && lead.Active == true //Active Leads      
                                            && lead.IsEmailVerified == false //Account not verified via Email
                                            && lead.IsPhoneVerified == false // Account not verified via text code
                                            && lead.IsAccreditedInvestor == false //Not Accredited Account
                                            select lead.Id).Count();
                return elements;
            }
            catch (Exception e)
            {
                e.ToString();
                return elements;
            }
            finally
            {
                contextData = null;
            }
        }
        public int DeleteLeads(DeleteUserAccountDto deleteUserAccountDto)
        {
            
            var adminAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == deleteUserAccountDto.AdminUserId);
            if (adminAccount != null && adminAccount.RoleId == 3 && adminAccount.Active == true) //Active Admin user
            {
                try
                {
                    List<UserAccount> leadAccounts = new List<UserAccount>();
                    using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                    {
                        foreach (int leadId in deleteUserAccountDto.Ids)
                        {
                            var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == leadId);
                            UserAccount userAccountData = userAccount;
                            userAccountData.Active = false;
                            userAccountData.ModifiedBy = deleteUserAccountDto.AdminUserId.ToString();
                            leadAccounts.Add(userAccountData);
                        }
                        _unitOfWork.UserAccountRepository.UpdateList(leadAccounts);
                        _unitOfWork.Save();
                        transaction.Commit();
                        return 1;
                    }
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                    return 0;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return 0;//Failure;
        }
        public bool AddLeadNotes(UserNotesDto notesDto)
        {
            var leadAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == notesDto.UserId);
            var adminAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == notesDto.AdminUserId);
            if (leadAccount != null && adminAccount != null)
            {
                try
                {                    
                    using (var transaction = _unitOfWork.UserNotesRepository.Context.Database.BeginTransaction())
                    {
                        UserNotes userNotesData = new UserNotes();
                        userNotesData.UserId = notesDto.UserId;
                        userNotesData.Notes = notesDto.Notes;
                        userNotesData.CreatedBy = adminAccount.FirstName +" "+ adminAccount.LastName;
                        userNotesData.CreatedOn = DateTime.Now;
                        userNotesData.Active = true;                        
                        _unitOfWork.UserNotesRepository.Insert(userNotesData);
                        _unitOfWork.Save();
                        transaction.Commit();
                    }
                    return true; //Success
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                    return false;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return false;//Failure;           
        }
        public bool UpdateLeadNotes(UserNotesDto notesDto)
        {
            var leadAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == notesDto.UserId);
            var adminAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == notesDto.AdminUserId);
            if (leadAccount != null && adminAccount != null)
            {
                try
                {
                    var userNotes = _unitOfWork.UserNotesRepository.Get(x => x.Id == notesDto.Id);
                    using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                    {
                        UserNotes userNotesData = userNotes;
                        userNotesData.Notes = notesDto.Notes;                       
                        userNotesData.ModifiedBy = adminAccount.FirstName + " " + adminAccount.LastName;
                        _unitOfWork.UserNotesRepository.Update(userNotesData);
                        _unitOfWork.Save();
                        transaction.Commit();
                    }
                    return true; //Success
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                    return false;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return false;//Failure; 
        }
        public bool DeleteLeadNotes(int adminUserId, int leadId)
        {
            var leadAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == leadId);
            var adminAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == adminUserId);
            if (leadAccount != null && adminAccount != null)
            {
                try
                {
                    var userNotes = _unitOfWork.UserNotesRepository.Get(x => x.Id == leadId);
                    if (userNotes != null)
                    { 
                        using (var transaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                        {
                            UserNotes userNotesData = userNotes;
                            userNotesData.Active = false;
                            userNotesData.ModifiedBy = adminAccount.FirstName + " " + adminAccount.LastName;
                            _unitOfWork.Save();
                            transaction.Commit();
                        }
                    }
                    return true; //Success
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                    return false;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return false;//Failure; 
        }
        public List<UserNotesDto> GetLeadNotes(int leadid)
        {
            try
            {
                List<UserNotesDto> userNotes = new List<UserNotesDto>();
                var contextData = _unitOfWork.UserAccountRepository.Context;
                userNotes = (from notes in contextData.UserNotes
                             where notes.UserId == leadid
                             && notes.Active == true
                             select new UserNotesDto
                             {
                                 Id = notes.Id,
                                 UserId = notes.UserId,
                                 Notes = notes.Notes,
                                 CreatedBy = notes.CreatedBy,
                                 CreatedOn = notes.CreatedOn,
                                 ModifiedBy = notes.ModifiedBy,
                                 ModifiedOn = notes.ModifiedOn
                             }).ToList();
                return userNotes;
            }
            catch (Exception e)
            {
                var ex = e.ToString();
                return null;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }            
        }
        public List<UserAccountDto> GetUnRegisteredLeadAccounts()
        {
            try
            {
                List<UserAccountDto> userAccounts = new List<UserAccountDto>();
                var contextData = _unitOfWork.UserAccountRepository.Context;
                userAccounts = (from lead in contextData.UserAccount
                             where lead.RoleId == 2 //Lead
                             && lead.Active == true
                             && lead.LastLogin == null //Not logged-in
                             select new UserAccountDto
                             {
                                 Id = lead.Id,
                                 RoleId = lead.Id,
                                 FirstName = lead.FirstName,
                                 LastName = lead.LastName,
                                 EmailId = lead.EmailId                                 
                             }).ToList();
                return userAccounts;
            }
            catch (Exception e)
            {
                var ex = e.ToString();
                return null;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public bool AddLeadTags(TagDto tagDto)
        {            
            var adminAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == tagDto.AdminUserId && x.Active == true && x.RoleId == 3); //Active admin user
            if (adminAccount != null)
            {
                try
                {
                    using (var transaction = _unitOfWork.TagRepository.Context.Database.BeginTransaction())
                    {
                        Tag tagData = new Tag();
                        tagData.AdminUserId = tagDto.AdminUserId;
                        tagData.Name = tagDto.Name;
                        tagData.Active = true;
                        tagData.Type = 2;//Lead
                        tagData.CreatedBy = tagDto.AdminUserId.ToString();
                        tagData.CreatedOn = DateTime.Now;                        
                        _unitOfWork.TagRepository.Insert(tagData);
                        _unitOfWork.Save();
                        transaction.Commit();
                        var tagId = tagData.Id;

                        if(tagDto.tagDetails != null)
                        {
                            using (var tagDetailtransaction = _unitOfWork.TagDetailRepository.Context.Database.BeginTransaction())
                            {
                                var contextData = _unitOfWork.TagDetailRepository.Context;
                                List<TagDetail> tagDetails = new List<TagDetail>();
                                foreach (TagDetailDto tagDetail in tagDto.tagDetails)
                                {
                                    TagDetail tagDetailData = new TagDetail();
                                    tagDetailData.TagId = tagId;
                                    tagDetailData.UserId = tagDetail.UserId;
                                    tagDetailData.Active = true;
                                    tagDetailData.CreatedBy = tagDto.AdminUserId.ToString();
                                    tagDetailData.CreatedOn = DateTime.Now;
                                    tagDetailData.Active = true;
                                    tagDetails.Add(tagDetailData);
                                }
                                contextData.AddRange(tagDetails);
                                contextData.SaveChanges();
                                tagDetailtransaction.Commit();
                            }                            
                        }                       
                    }
                    return true; //Success
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                    return false;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return false;//Failure; 
        }
        public bool UpdateLeadTags(TagDto tagDto)
        {
            var adminAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == tagDto.AdminUserId && x.Active == true && x.RoleId == 3); //Active admin user
            if (adminAccount != null)
            {
                try
                {
                    var tag = _unitOfWork.TagRepository.Get(x => x.Id == tagDto.Id && x.Active == true);
                    if (tag != null)
                    {
                        using (var transaction = _unitOfWork.TagRepository.Context.Database.BeginTransaction())
                        {
                            Tag tagData = tag;
                            if (tag.Name != tagDto.Name)
                            {
                                tag.Name = tagDto.Name;
                                tagData.ModifiedBy = tagDto.AdminUserId.ToString();
                                tagData.ModifiedOn = DateTime.Now;
                                _unitOfWork.TagRepository.Update(tagData);
                                _unitOfWork.Save();
                                transaction.Commit();
                            }
                        }                       
                        var contextData = _unitOfWork.TagDetailRepository.Context;                            
                        using (var tagDetailtransaction = _unitOfWork.TagDetailRepository.Context.Database.BeginTransaction())
                        {
                            if (tagDto.tagDetails != null)
                            {
                                foreach(TagDetailDto tagDetail in tagDto.tagDetails)
                                {
                                    var tagData = _unitOfWork.TagDetailRepository.Get(x => x.UserId == tagDetail.UserId 
                                                                                        && x.Active == true
                                                                                        && x.TagId == tagDto.Id);
                                    if (tagData != null)
                                    {
                                        TagDetail tagDetailData = tagData;
                                        if (tagDetail.Active == false)
                                        {
                                            tagDetailData.Active = false;
                                            tagDetailData.ModifiedBy = tagDto.AdminUserId.ToString();
                                            tagDetailData.ModifiedOn = DateTime.Now;
                                            _unitOfWork.TagDetailRepository.Update(tagDetailData);
                                            _unitOfWork.Save();                                            
                                        }
                                    }
                                    else
                                    {
                                        TagDetail tagDetailData = new TagDetail();
                                        tagDetailData.TagId = tagDto.Id;
                                        tagDetailData.UserId = tagDetail.UserId;
                                        tagDetailData.Active = true;
                                        tagDetailData.CreatedBy = tagDto.AdminUserId.ToString();
                                        tagDetailData.CreatedOn = DateTime.Now;
                                        tagDetailData.Active = true;                                       
                                        _unitOfWork.TagDetailRepository.Insert(tagDetailData);
                                        _unitOfWork.Save();                                       
                                    }
                                }
                                tagDetailtransaction.Commit();                                
                            }                        
                        }
                    }
                    return true; //Success
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                    return false;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return false;//Failure; 
        }
        public bool DeleteLeadTags(int adminUserId, int tagId)
        {           
            var adminAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == adminUserId && x.Active == true && x.RoleId == 3); //Active admin user
            if (adminAccount != null)
            {
                try
                {
                    var tag = _unitOfWork.TagRepository.Get(x => x.Id == tagId && x.Active == true);
                    if (tag != null)
                    {
                        using (var transaction = _unitOfWork.TagRepository.Context.Database.BeginTransaction())
                        {
                            Tag tagData = tag;
                            tagData.Active = false;
                            tagData.ModifiedBy = adminUserId.ToString();
                            tagData.ModifiedOn = DateTime.Now;
                            _unitOfWork.TagRepository.Update(tagData);
                            _unitOfWork.Save();
                            transaction.Commit();
                        }
                        var tagDetails = _unitOfWork.TagDetailRepository.GetMany(x => x.TagId == tagId && x.Active == true);
                        if (tagDetails != null)
                        {
                            var contextData = _unitOfWork.TagDetailRepository.Context;
                            List<TagDetail> tagDetailsData = new List<TagDetail>();
                            using (var tagDetailtransaction = _unitOfWork.TagDetailRepository.Context.Database.BeginTransaction())
                            {
                                foreach (TagDetail tagDetail in tagDetails)
                                {
                                    TagDetail tagDetailData = tagDetail;
                                    tagDetailData.Active = false;
                                    tagDetailData.ModifiedBy = adminUserId.ToString();
                                    tagDetailData.ModifiedOn = DateTime.Now;
                                    tagDetailsData.Add(tagDetailData);
                                }
                                contextData.UpdateRange(tagDetailsData);
                                contextData.SaveChanges();
                                tagDetailtransaction.Commit();
                            }                                                     
                        }
                    }
                    return true; //Success
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                    return false;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return false;//Failure; 
        }
        public List<TagDto> GetLeadTags(int adminUserId)
        {
            try
            {
                List<TagDto> tags = new List<TagDto>();
                var contextData = _unitOfWork.TagRepository.Context;
                tags = (from tag in contextData.Tag
                        where tag.AdminUserId == adminUserId
                        && tag.Active == true
                        && tag.Type == 2 // Lead tags
                        select new TagDto
                        {
                            Id = tag.Id,
                            AdminUserId = tag.AdminUserId,
                            Name = tag.Name,
                            Active = tag.Active,
                            Type = tag.Type,
                            CreatedBy = tag.CreatedBy,
                            CreatedOn = tag.CreatedOn,
                            ModifiedBy = tag.ModifiedBy,
                            ModifiedOn = tag.ModifiedOn,
                            tagDetails = (from tagDetail in contextData.TagDetail
                                          where tagDetail.TagId == tag.Id
                                          && tagDetail.Active == true
                                          select new TagDetailDto
                                          {
                                              Id = tagDetail.Id,
                                              TagId = tagDetail.TagId,
                                              UserId = tagDetail.UserId,
                                              Active = tagDetail.Active,
                                              CreatedBy = tagDetail.CreatedBy,
                                              CreatedOn = tagDetail.CreatedOn,
                                              ModifiedBy = tagDetail.ModifiedBy,
                                              ModifiedOn = tagDetail.ModifiedOn
                                          }).ToList()
                        }).ToList();
                return tags;
            }
            catch (Exception e)
            {
                var ex = e.ToString();
                return null;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
    }  
}
