using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Web.API.Common.UnitOfWork;
using AutoMapper;
using Credor.Client.Entities;
using Credor.Data.Entities;
using Credor.Web.API.Shared;
using Microsoft.AspNetCore.SignalR;
using Credor.Web.API.Common;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class ProfileRepository : IProfileRespository
    {
        private readonly UnitOfWork _unitOfWork;
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public ProfileRepository(IMapper mapper,
                                 INotificationRepository notificationRepository,
                                 IHubContext<NotificationHub> hubContext,
                                  IConfiguration configuration)
        {
            _unitOfWork = new UnitOfWork(configuration);
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
    }
        public List<UserProfileType> GetUserProfileTypes()
        {
            try
            {
                var result = _unitOfWork.UserProfileTypeRepository.GetAll().ToList();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public List<DistributionType> GetDistributionTypes()
        {
            try
            {
                var result = _unitOfWork.DistributionTypeRepository.GetAll().ToList();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public List<StateOrProvince> GetStateOrProvinceList()
        {
            try
            {
                var result = _unitOfWork.StateOrProvinceRepository.GetAll().ToList();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public int CreateUserProfile(UserProfileDto userProfileDto)
        {
            int userProfileId;
            if (userProfileDto.Id == 0)
            {
                try
                {
                    using (var transaction = _unitOfWork.UserProfileRepository.Context.Database.BeginTransaction())
                    {
                        Helper _helper = new Helper();
                        UserProfile userProfile = _mapper.Map<UserProfile>(userProfileDto);                        
                        userProfile.Active = true;
                        userProfile.Status = 1;
                        userProfile.DisplayId = _helper.GetRandomString(2);
                        if (userProfileDto.AdminUserId == 0)
                            userProfile.CreatedBy = userProfileDto.UserId.ToString();
                        else
                            userProfile.CreatedBy = userProfileDto.AdminUserId.ToString();
                        userProfile.CreatedOn = DateTime.Now;                        
                        _unitOfWork.UserProfileRepository.Insert(userProfile);
                        _unitOfWork.Save();
                        transaction.Commit();
                        userProfileId = userProfile.Id;
                    }
                    if (userProfileDto.Type == 5 && userProfileDto.Investors.Count > 0)
                    {
                        foreach (InvestorDto investerDto in userProfileDto.Investors)
                        {
                            if (investerDto.Id == 0)
                            {
                                var result = AddInvestor(investerDto, userProfileId,userProfileDto.UserId);
                                if (result == 0)
                                {
                                    _unitOfWork.InvestorRepository.Delete(x => x.UserProfileId == userProfileId);
                                    _unitOfWork.UserProfileRepository.Delete(x => x.Id == userProfileId);
                                }
                            }
                            else
                            {
                                var result = UpdateInvestor(investerDto,userProfileDto.UserId);
                                if (result == 0)
                                {                                    
                                    _unitOfWork.UserProfileRepository.Delete(x => x.Id == userProfileId);
                                }
                            }
                        }
                    }
                    string profileType = GetProfileTypeName(userProfileDto.Type);
                    
                    string message = "You added "+ profileType + " profile called "+ userProfileDto.FirstName + " " + userProfileDto.LastName;
                    var notification = _notificationRepository.AddNotification(userProfileDto.UserId, "User Profile Added", message);
                    if (notification != null)
                    {
                        _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                    }
                    return 1; //Success
                }

                catch (Exception ex)
                {
                    ex.ToString();
                    return 0;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return 0;//Failure
        }
        private string GetProfileTypeName(int profileType)
        {
            switch (profileType)
            {
                case 1:
                    return "IRA";                  
                case 2:
                    return "LLC, Corporation, or Partnership";
                case 3:
                    return "Individual";
                case 4:
                    return "Trust";
                case 5:
                    return "Joint Registration";
                case 6:
                    return "Retirement Plan";
                default:
                    return "" ;
            }            
        }
        public int UpdateUserProfile(UserProfileDto userProfileDto)
        {
           var profile = _unitOfWork.UserProfileRepository.Get(x => x.Id == userProfileDto.Id);
            if (profile.Id != 0)
            {
                try
                {
                    using (var transaction = _unitOfWork.UserProfileRepository.Context.Database.BeginTransaction())
                    {
                        UserProfile userProfile = profile;
                        if (userProfileDto.Type == 1)
                        {
                            userProfile.Name = userProfileDto.Name;
                            userProfile.InCareOf = userProfileDto.InCareOf;
                            userProfile.StreetAddress1 = userProfileDto.StreetAddress1;
                            userProfile.StreetAddress2 = userProfileDto.StreetAddress2;
                            userProfile.City = userProfileDto.City;
                            userProfile.StateOrProvinceId = userProfileDto.StateOrProvinceId;
                            userProfile.Country = userProfileDto.Country;
                            userProfile.State = userProfileDto.State;
                            userProfile.ZipCode = userProfileDto.ZipCode;
                            userProfile.TaxId = userProfileDto.TaxId;
                            userProfile.DistributionTypeId = userProfileDto.DistributionTypeId == 0 ? null : userProfileDto.DistributionTypeId;
                            userProfile.DistributionDetail = userProfileDto.DistributionDetail;
                            userProfile.CheckInCareOf = userProfileDto.CheckInCareOf;
                            userProfile.CheckAddressLine1 = userProfileDto.CheckAddressLine1;
                            userProfile.CheckAddressLine2 = userProfileDto.CheckAddressLine2;
                            userProfile.CheckCity = userProfileDto.CheckInCareOf;
                            userProfile.CheckStateId = userProfileDto.CheckStateId;
                            userProfile.CheckZip = userProfile.CheckZip;
                            userProfile.BankAccountId = userProfileDto.BankAccountId;

                        }
                        if (userProfileDto.Type == 2)
                        {
                            userProfile.Name = userProfileDto.Name;
                            userProfile.InCareOf = userProfileDto.InCareOf;
                            userProfile.StreetAddress1 = userProfileDto.StreetAddress1;
                            userProfile.StreetAddress2 = userProfileDto.StreetAddress2;
                            userProfile.City = userProfileDto.City;
                            userProfile.StateOrProvinceId = userProfileDto.StateOrProvinceId;
                            userProfile.Country = userProfileDto.Country;
                            userProfile.State = userProfileDto.State;
                            userProfile.ZipCode = userProfileDto.ZipCode;
                            userProfile.TaxId = userProfileDto.TaxId;
                            userProfile.DistributionTypeId = userProfileDto.DistributionTypeId == 0 ? null : userProfileDto.DistributionTypeId;
                            userProfile.IsDisregardedEntity = userProfileDto.IsDisregardedEntity;
                            userProfile.OwnerTaxId = userProfileDto.OwnerTaxId;
                            userProfile.IsIRALLC = userProfileDto.IsIRALLC;
                            userProfile.DistributionDetail = userProfileDto.DistributionDetail;
                            userProfile.CheckInCareOf = userProfileDto.CheckInCareOf;
                            userProfile.CheckAddressLine1 = userProfileDto.CheckAddressLine1;
                            userProfile.CheckAddressLine2 = userProfileDto.CheckAddressLine2;
                            userProfile.CheckCity = userProfileDto.CheckInCareOf;
                            userProfile.CheckStateId = userProfileDto.CheckStateId;
                            userProfile.CheckZip = userProfile.CheckZip;
                            userProfile.BankAccountId = userProfileDto.BankAccountId;
                        }
                        if (userProfileDto.Type == 3)
                        {
                            userProfile.FirstName = userProfileDto.FirstName;
                            userProfile.LastName = userProfileDto.LastName;
                            userProfile.StreetAddress1 = userProfileDto.StreetAddress1;
                            userProfile.StreetAddress2 = userProfileDto.StreetAddress2;
                            userProfile.City = userProfileDto.City;
                            userProfile.StateOrProvinceId = userProfileDto.StateOrProvinceId;
                            userProfile.Country = userProfileDto.Country;
                            userProfile.State = userProfileDto.State;
                            userProfile.ZipCode = userProfileDto.ZipCode;
                            userProfile.TaxId = userProfileDto.TaxId;
                            userProfile.DistributionTypeId = userProfileDto.DistributionTypeId == 0 ? null : userProfileDto.DistributionTypeId;
                            userProfile.DistributionDetail = userProfileDto.DistributionDetail;
                            userProfile.CheckInCareOf = userProfileDto.CheckInCareOf;
                            userProfile.CheckAddressLine1 = userProfileDto.CheckAddressLine1;
                            userProfile.CheckAddressLine2 = userProfileDto.CheckAddressLine2;
                            userProfile.CheckCity = userProfileDto.CheckInCareOf;
                            userProfile.CheckStateId = userProfileDto.CheckStateId;
                            userProfile.CheckZip = userProfile.CheckZip;
                            userProfile.BankAccountId = userProfileDto.BankAccountId;
                        }
                        if (userProfileDto.Type == 4)
                        {
                            userProfile.TrustName = userProfileDto.TrustName;
                            userProfile.InCareOf = userProfileDto.InCareOf;
                            userProfile.StreetAddress1 = userProfileDto.StreetAddress1;
                            userProfile.StreetAddress2 = userProfileDto.StreetAddress2;
                            userProfile.City = userProfileDto.City;
                            userProfile.StateOrProvinceId = userProfileDto.StateOrProvinceId;
                            userProfile.Country = userProfileDto.Country;
                            userProfile.State = userProfileDto.State;
                            userProfile.ZipCode = userProfileDto.ZipCode;
                            userProfile.TaxId = userProfileDto.TaxId;
                            userProfile.DistributionTypeId = userProfileDto.DistributionTypeId == 0 ? null : userProfileDto.DistributionTypeId;
                            userProfile.DistributionDetail = userProfileDto.DistributionDetail;
                            userProfile.CheckInCareOf = userProfileDto.CheckInCareOf;
                            userProfile.CheckAddressLine1 = userProfileDto.CheckAddressLine1;
                            userProfile.CheckAddressLine2 = userProfileDto.CheckAddressLine2;
                            userProfile.CheckCity = userProfileDto.CheckInCareOf;
                            userProfile.CheckStateId = userProfileDto.CheckStateId;
                            userProfile.CheckZip = userProfile.CheckZip;
                            userProfile.BankAccountId = userProfileDto.BankAccountId;
                        }
                        if (userProfileDto.Type == 6)
                        {
                            userProfile.RetirementPlanName = userProfileDto.RetirementPlanName;
                            userProfile.SignorName = userProfileDto.SignorName;
                            userProfile.StreetAddress1 = userProfileDto.StreetAddress1;
                            userProfile.StreetAddress2 = userProfileDto.StreetAddress2;
                            userProfile.City = userProfileDto.City;
                            userProfile.StateOrProvinceId = userProfileDto.StateOrProvinceId;
                            userProfile.Country = userProfileDto.Country;
                            userProfile.State = userProfileDto.State;
                            userProfile.ZipCode = userProfileDto.ZipCode;
                            userProfile.TaxId = userProfileDto.TaxId;
                            userProfile.DistributionTypeId = userProfileDto.DistributionTypeId == 0 ? null : userProfileDto.DistributionTypeId;
                            userProfile.DistributionDetail = userProfileDto.DistributionDetail;
                            userProfile.CheckInCareOf = userProfileDto.CheckInCareOf;
                            userProfile.CheckAddressLine1 = userProfileDto.CheckAddressLine1;
                            userProfile.CheckAddressLine2 = userProfileDto.CheckAddressLine2;
                            userProfile.CheckCity = userProfileDto.CheckInCareOf;
                            userProfile.CheckStateId = userProfileDto.CheckStateId;
                            userProfile.CheckZip = userProfile.CheckZip;
                            userProfile.BankAccountId = userProfileDto.BankAccountId;

                        }
                        if (userProfileDto.Type == 5)
                        {
                            userProfile.Name = userProfileDto.Name;
                            userProfile.InCareOf = userProfileDto.InCareOf;
                            userProfile.StreetAddress1 = userProfileDto.StreetAddress1;
                            userProfile.StreetAddress2 = userProfileDto.StreetAddress2;
                            userProfile.City = userProfileDto.City;
                            userProfile.StateOrProvinceId = userProfileDto.StateOrProvinceId;
                            userProfile.Country = userProfileDto.Country;
                            userProfile.State = userProfileDto.State;
                            userProfile.ZipCode = userProfileDto.ZipCode;
                            userProfile.TaxId = userProfileDto.TaxId;
                            userProfile.DistributionTypeId = userProfileDto.DistributionTypeId == 0? null: userProfileDto.DistributionTypeId;
                            userProfile.DistributionDetail = userProfileDto.DistributionDetail;
                            userProfile.CheckInCareOf = userProfileDto.CheckInCareOf;
                            userProfile.CheckAddressLine1 = userProfileDto.CheckAddressLine1;
                            userProfile.CheckAddressLine2 = userProfileDto.CheckAddressLine2;
                            userProfile.CheckCity = userProfileDto.CheckCity;
                            userProfile.CheckStateId = userProfileDto.CheckStateId;
                            userProfile.CheckZip = userProfile.CheckZip;
                            userProfile.BankAccountId = userProfileDto.BankAccountId;
                        }
                        if (userProfileDto.AdminUserId == 0)
                            userProfile.ModifiedBy = userProfileDto.UserId.ToString();
                        else
                            userProfile.ModifiedBy = userProfileDto.AdminUserId.ToString();                        
                        
                        _unitOfWork.UserProfileRepository.Update(userProfile);
                        _unitOfWork.Save();
                        transaction.Commit();

                        if (userProfileDto.Type == 5 && userProfileDto.Investors.Count > 0)
                        {
                            foreach (InvestorDto investerDto in userProfileDto.Investors)
                            {
                                if (investerDto.Id != 0)
                                {
                                    var result = UpdateInvestor(investerDto,userProfileDto.UserId);

                                    if (result == 0)
                                    {
                                        //Handle roll back.
                                        return 0;
                                    }
                                }
                                else
                                {
                                    var result = AddInvestor(investerDto,userProfileDto.Id,userProfileDto.UserId);
                                    if (result == 0)
                                    {
                                        //Handle roll back.
                                        return 0;
                                    }
                                }
                            }
                        }
                    }
                    string message = "You updated Profile " + userProfileDto.FirstName + " " + userProfileDto.LastName;
                    var notification = _notificationRepository.AddNotification(userProfileDto.UserId, "User Profile Updated", message);
                    if (notification != null)
                    {
                        _hubContext.Clients.All.SendAsync("Push_Notification", notification);
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
                return 0;//Failure
        }
        public UserProfileDto GetUserProfile(int userId, int userProfileId)
        {
            try
            {
                var contextData = _unitOfWork.UserProfileRepository.Context;

                var userProfileData = (from userProfile in contextData.UserProfile
                                    where userProfile.Id == userProfileId && userProfile.UserId == userId
                                    select new UserProfileDto
                                    {
                                        Id = userProfile.Id,
                                        UserId = userProfile.UserId,
                                        ProfileDisplayId = userProfile.DisplayId,
                                        Type = userProfile.Type,
                                        Name = userProfile.Name,
                                        FirstName = userProfile.FirstName,
                                        LastName = userProfile.LastName,
                                        TrustName = userProfile.TrustName,
                                        RetirementPlanName = userProfile.RetirementPlanName,
                                        SignorName = userProfile.SignorName,
                                        InCareOf = userProfile.InCareOf,
                                        StreetAddress1 = userProfile.StreetAddress1,
                                        StreetAddress2 = userProfile.StreetAddress2,
                                        City = userProfile.City,
                                        StateOrProvinceId = userProfile.StateOrProvinceId,
                                        Country = userProfile.Country,
                                        State = userProfile.State,
                                        ZipCode = userProfile.ZipCode,
                                        TaxId = userProfile.TaxId,
                                        DistributionTypeId = userProfile.DistributionTypeId == null?0:userProfile.DistributionTypeId,
                                        IsDisregardedEntity = userProfile.IsDisregardedEntity,
                                        IsIRALLC = userProfile.IsIRALLC,
                                        OwnerTaxId = userProfile.OwnerTaxId,
                                        DistributionDetail = userProfile.DistributionDetail,
                                        CheckInCareOf = userProfile.CheckInCareOf,
                                        CheckAddressLine1 = userProfile.CheckAddressLine1,
                                        CheckAddressLine2 = userProfile.CheckAddressLine2,
                                        CheckCity = userProfile.CheckCity,
                                        CheckStateId = userProfile.CheckStateId,
                                        CheckZip = userProfile.CheckZip,
                                        BankAccountId = userProfile.BankAccountId,
                                        Active = userProfile.Active,
                                        Status = userProfile.Status,
                                        IsOwner  = userProfile.IsOwner,
                                        CreatedBy = userProfile.CreatedBy,
                                        CreatedOn = userProfile.CreatedOn,
                                        ModifiedOn = userProfile.ModifiedOn,
                                        ModifiedBy = userProfile.ModifiedBy,
                                        ApprovedOn = userProfile.ApprovedOn,
                                        ApprovedBy = userProfile.ApprovedBy,
                                        Investors= (from investor in contextData.Investor
                                                         where investor.UserProfileId == userProfile.Id                                                         
                                                         select new InvestorDto
                                                         {
                                                             Id = investor.Id,
                                                             UserProfileId = investor.UserProfileId,
                                                             FirstName = investor.FirstName,
                                                             LastName = investor.LastName,
                                                             EmailId = investor.EmailId,
                                                             Phone = investor.Phone,
                                                             IsNotificationEnabled = investor.IsNotificationEnabled,
                                                             IsOwner = investor.IsOwner,
                                                             Active = investor.Active,                                                             
                                                             Status= investor.Status,
                                                             CreatedBy = investor.CreatedBy,
                                                             CreatedOn = investor.CreatedOn,
                                                             ModifiedBy = investor.ModifiedBy,
                                                             ModifiedOn = investor.ModifiedOn
                                                         }).ToList(),
                                        Investments = (from investment in contextData.Investment
                                                       where investment.UserProfileId == userProfile.Id
                                                       select new InvestmentDto
                                                       {
                                                           Id = investment.Id,
                                                           UserProfileId = investment.UserProfileId,
                                                           Amount = investment.Amount,
                                                           FundedDate = investment.FundedDate
                                                       }).ToList()
                                    }).FirstOrDefault();
                contextData = null;
                return userProfileData;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<UserProfileDto> GetAllUserProfile(int userId)
        {
            List<UserProfileDto> userProfileList = new List<UserProfileDto>();
            var contextData = _unitOfWork.UserProfileRepository.Context;
            try
            {
                userProfileList = (from userProfile in contextData.UserProfile
                                   join dType in contextData.UserProfileType on userProfile.Type equals dType.Id
                                   where userProfile.UserId == userId && userProfile.Active == true

                                   select new UserProfileDto
                                   {
                                       Id = userProfile.Id,
                                       UserId = userProfile.UserId,
                                       ProfileDisplayId = userProfile.DisplayId,
                                       ProfileName = userProfile.Name + (userProfile.FirstName + " " + userProfile.LastName) + userProfile.RetirementPlanName
                                                     + userProfile.TrustName,
                                       Type = userProfile.Type,
                                       TypeName = dType.Name,
                                       Name = userProfile.Name,
                                       FirstName = userProfile.FirstName,
                                       LastName = userProfile.LastName,
                                       TrustName = userProfile.TrustName,
                                       RetirementPlanName = userProfile.RetirementPlanName,
                                       SignorName = userProfile.SignorName,
                                       InCareOf = userProfile.InCareOf,
                                       StreetAddress1 = userProfile.StreetAddress1,
                                       StreetAddress2 = userProfile.StreetAddress2,
                                       City = userProfile.City,
                                       StateOrProvinceId = userProfile.StateOrProvinceId,
                                       Country = userProfile.Country,
                                       State = userProfile.State,
                                       ZipCode = userProfile.ZipCode,
                                       TaxId = userProfile.TaxId,
                                       DistributionTypeId = userProfile.DistributionTypeId == null ? 0 : userProfile.DistributionTypeId,
                                       IsDisregardedEntity = userProfile.IsDisregardedEntity,
                                       IsIRALLC = userProfile.IsIRALLC,
                                       IsOwner = userProfile.IsOwner,
                                       OwnerTaxId = userProfile.OwnerTaxId,
                                       DistributionDetail = userProfile.DistributionDetail,
                                       CheckInCareOf = userProfile.CheckInCareOf,
                                       CheckAddressLine1 = userProfile.CheckAddressLine1,
                                       CheckAddressLine2 = userProfile.CheckAddressLine2,
                                       CheckCity = userProfile.CheckCity,
                                       CheckStateId = userProfile.CheckStateId,
                                       CheckZip = userProfile.CheckZip,
                                       BankAccountId = userProfile.BankAccountId,
                                       Active = userProfile.Active,
                                       Status = userProfile.Status,
                                       CreatedBy = userProfile.CreatedBy,
                                       CreatedOn = userProfile.CreatedOn,
                                       ModifiedOn = userProfile.ModifiedOn,
                                       ModifiedBy = userProfile.ModifiedBy,
                                       ApprovedOn = userProfile.ApprovedOn,
                                       ApprovedBy = userProfile.ApprovedBy,

                                       Investors = (from investor in contextData.Investor
                                                    where investor.UserProfileId == userProfile.Id
                                                    select new InvestorDto
                                                    {
                                                        Id = investor.Id,
                                                        UserProfileId = investor.UserProfileId,
                                                        FirstName = investor.FirstName,
                                                        LastName = investor.LastName,
                                                        EmailId = investor.EmailId,
                                                        Phone = investor.Phone,
                                                        IsNotificationEnabled = investor.IsNotificationEnabled,
                                                        IsOwner = investor.IsOwner,
                                                        Active = investor.Active,
                                                        Status = investor.Status,
                                                        CreatedBy = investor.CreatedBy,
                                                        CreatedOn = investor.CreatedOn,
                                                        ModifiedBy = investor.ModifiedBy,
                                                        ModifiedOn = investor.ModifiedOn
                                                    }).ToList(),
                                       Investments = (from investment in contextData.Investment
                                                      where investment.UserProfileId == userProfile.Id
                                                      select new InvestmentDto
                                                      { Id = investment.Id,
                                                          UserProfileId = investment.UserProfileId,
                                                          Amount = investment.Amount,
                                                          FundedDate = investment.FundedDate
                                                      }).ToList()
                                   }).OrderBy(x => x.CreatedOn).ToList();

            }
            catch(Exception e)
            {
                var x = e.ToString();
                userProfileList = null;
            }
            finally
            {
                contextData = null;
            }
            return userProfileList;
        }
        public int DeleteUserProfile(int userid, int userprofileid,int adminUserId = 0)
        {
            var profile = _unitOfWork.UserProfileRepository.Get(x => x.Id == userprofileid && x.UserId == userid);
            if (profile.Id != 0)
            {
                try
                {
                    using (var transaction = _unitOfWork.UserProfileRepository.Context.Database.BeginTransaction())
                    {

                        UserProfile userProfile = profile;
                        if (adminUserId == 0)
                            userProfile.ModifiedBy = userProfile.UserId.ToString();
                        else
                            userProfile.ModifiedBy = adminUserId.ToString();
                        userProfile.ModifiedOn = DateTime.Now;
                        userProfile.Active = false;
                        _unitOfWork.UserProfileRepository.Update(userProfile);
                        _unitOfWork.Save();
                        transaction.Commit();
                    }
                    string message = "You deleted Profile " + profile.FirstName + " " + profile.LastName;
                    var notification = _notificationRepository.AddNotification(userid, "User Profile Updated", message);
                    if (notification != null)
                    {
                        _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                    }
                    return 1; //Success
                }

                catch (Exception e)
                {
                    e.ToString();
                    return 0;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return 0;//Failure
        }
        public int AddInvestor(InvestorDto investerDto,int userProfileId,int userId)
        {
            if (userProfileId != 0)
            {
                try
                {
                    using (var transaction = _unitOfWork.InvestorRepository.Context.Database.BeginTransaction())
                    {

                        Investor investor = _mapper.Map<Investor>(investerDto);
                        investor.UserProfileId = userProfileId;
                        investor.Active = true;
                        investor.Status = 1;
                        investor.CreatedBy = userId.ToString();
                        investor.CreatedOn = DateTime.Now;
                        _unitOfWork.InvestorRepository.Insert(investor);
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
                //finally
                //{
                //    _unitOfWork.Dispose();
                //}
            }
            else
                return 0;//Failure
        }
        public int AddInvestor(InvestorDto investerDto)
        {
            try
            {
                using (var transaction = _unitOfWork.InvestorRepository.Context.Database.BeginTransaction())
                {

                    Investor investor = _mapper.Map<Investor>(investerDto);
                    investor.Active = true;
                    investor.Status = 1;                    
                    investor.CreatedBy = investerDto.UserProfileId.ToString();
                    investor.CreatedOn = DateTime.Now;
                    _unitOfWork.InvestorRepository.Insert(investor);
                    _unitOfWork.Save();
                    transaction.Commit();
                }
                return 1; //Success
            }

            catch (Exception)
            {
                return 0;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public int UpdateInvestor(InvestorDto investerDto,int userId)
        {
            var investorData = _unitOfWork.InvestorRepository.Get(x => x.Id == investerDto.Id);
            if (investorData.Id != 0)
            {
                try
                {
                    using (var transaction = _unitOfWork.InvestorRepository.Context.Database.BeginTransaction())
                    {
                        Investor investor = investorData;
                        investor.FirstName = investerDto.FirstName;
                        investor.LastName = investerDto.LastName;
                        investor.EmailId = investerDto.EmailId;
                        investor.Phone = investerDto.Phone;
                        investor.IsNotificationEnabled = investerDto.IsNotificationEnabled;                        
                        investor.Status = investerDto.Status;
                        investor.Active = investerDto.Active;
                        investor.ModifiedBy = userId.ToString();
                        _unitOfWork.InvestorRepository.Update(investor);
                        _unitOfWork.Save();
                        transaction.Commit();

                    }
                    return 1;
                }
                catch
                {
                    return 0;
                }
                finally
                {
                    investorData = null;
                }                
            }
            return 0;
        }
        public List<InvestorDto> GetInvestors(int userId)
        {
            try
            {
                var contextData = _unitOfWork.InvestorRepository.Context;

                var investors = (from userProfile in contextData.UserProfile
                                 join investor in contextData.Investor on userProfile.Id equals investor.UserProfileId
                                 where userProfile.UserId == userId && userProfile.Type == 5// Joint Registration                                      
                                 select new InvestorDto
                                 {
                                     Id = investor.Id,
                                     UserProfileId = investor.UserProfileId,
                                     FirstName = investor.FirstName,
                                     LastName = investor.LastName,
                                     EmailId = investor.EmailId,
                                     Phone = investor.Phone,                                     
                                     IsNotificationEnabled = investor.IsNotificationEnabled,
                                     IsOwner = investor.IsOwner,
                                     Active = investor.Active,
                                     Status = investor.Status,
                                     CreatedBy = investor.CreatedBy,
                                     CreatedOn = investor.CreatedOn,
                                     ModifiedBy = investor.ModifiedBy,
                                     ModifiedOn = investor.ModifiedOn
                                 }).ToList();                                      
                contextData = null;
                return investors;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<InvestorDto> GetInvestorsByUserProfile(int userprofileId)
        {
            try
            {
                var contextData = _unitOfWork.InvestorRepository.Context;

                var investors = (from userProfile in contextData.UserProfile
                                 join investor in contextData.Investor on userProfile.Id equals investor.UserProfileId
                                 where userProfile.Id == userprofileId && userProfile.Type == 5// Joint Registration                                      
                                 select new InvestorDto
                                 {
                                     Id = investor.Id,
                                     UserProfileId = investor.UserProfileId,
                                     FirstName = investor.FirstName,
                                     LastName = investor.LastName,
                                     EmailId = investor.EmailId,
                                     Phone = investor.Phone,
                                     IsNotificationEnabled = investor.IsNotificationEnabled,
                                     IsOwner = investor.IsOwner,
                                     Active = investor.Active,
                                     Status = investor.Status,
                                     CreatedBy = investor.CreatedBy,
                                     CreatedOn = investor.CreatedOn,
                                     ModifiedBy = investor.ModifiedBy,
                                     ModifiedOn = investor.ModifiedOn
                                 }).ToList();
                contextData = null;
                return investors;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
