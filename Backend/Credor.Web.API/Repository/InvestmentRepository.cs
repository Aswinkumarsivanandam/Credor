using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Web.API.Common.UnitOfWork;
using AutoMapper;
using Credor.Data.Entities;
using Credor.Client.Entities;
using Microsoft.AspNetCore.SignalR;
using Credor.Web.API.Common;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class InvestmentRepository : IInvestmentRepository
    {
        private readonly UnitOfWork _unitOfWork;
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public InvestmentRepository(IMapper mapper,
                                    INotificationRepository notificationRepository,
                                    IHubContext<NotificationHub> hubContext,
                                    IConfiguration configuration)
        {
            _unitOfWork = new UnitOfWork(configuration);
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }

        public List<OfferingVisibilityDto> GetOfferingVisibilities()
        {
            List<OfferingVisibilityDto> visibilities = new List<OfferingVisibilityDto>();
            var contextData = _unitOfWork.UserProfileTypeRepository.Context;
            try
            {
                visibilities = (from type in contextData.OfferingVisibility
                                select new OfferingVisibilityDto
                                {
                                    Id = type.Id,
                                    AccessTo = type.AccessTo,
                                    Active = type.Active
                                }).ToList();
            }
            catch
            {
                visibilities = null;
            }
            finally
            {
                contextData = null;
            }
            return visibilities;
        }

        public List<PortfolioOfferingDto> GetOfferingsAndReservations()
        {
            try
            {
                var contextData = _unitOfWork.PortfolioOfferingRepository.Context;

                var offeringList = (from offering in contextData.PortfolioOffering
                                    where (offering.Visibility == 2 || offering.Visibility == 3 || offering.Visibility == 6)//All Users/Verified Users/Test(IT and Admin)
                                    select new PortfolioOfferingDto
                                    {
                                        Id = offering.Id,
                                        Name = offering.Name,
                                        PictureUrl = offering.PictureUrl,
                                        EntityName = offering.EntityName,
                                        Active = offering.Active,
                                        Status = offering.Status,
                                        Size = offering.Size,
                                        IsReservation = offering.IsReservation,
                                        Visibility = offering.Visibility,
                                        PublicLandingPageUrl = offering.PublicLandingPageUrl,
                                        ShowPercentageRaised = offering.ShowPercentageRaised,
                                        MinimumInvestment = offering.MinimumInvestment,
                                        IsPrivate = offering.IsPrivate,
                                        CreatedOn = offering.CreatedOn,
                                        KeyHighlights = (from PKH in contextData.PortfolioKeyHighlight
                                                         where PKH.OfferingId == offering.Id
                                                         && PKH.Active == true
                                                         join KH in contextData.KeyHighlight on PKH.KeyHighlightId equals KH.Id
                                                         select new PortfolioKeyHighlightDto
                                                         {
                                                             Id = PKH.Id,
                                                             OfferingId = PKH.OfferingId,
                                                             KeyHighLightId = PKH.KeyHighlightId,
                                                             Name = KH.Name,
                                                             Field = PKH.Field,
                                                             Value = PKH.Value,
                                                             Active = PKH.Active,
                                                             Visibility = PKH.Visibility
                                                         }).ToList(),
                                        Summary = (from summary in contextData.PortfolioSummary
                                                   where summary.OfferingId == offering.Id
                                                   select new PortfolioSummaryDto
                                                   {
                                                       Id = summary.Id,
                                                       OfferingId = summary.OfferingId,
                                                       Summary = summary.Summary,
                                                       Status = summary.Status,
                                                       Active = summary.Active
                                                   }).ToList(),
                                        Documents = (from document in contextData.Document
                                                     where document.OfferingId == offering.Id
                                                     && document.Active == true
                                                     select new PortfolioDocumentDto
                                                     {
                                                         Id = document.Id,
                                                         OfferingId = document.OfferingId,
                                                         Name = document.Name,
                                                         Type = document.Extension,
                                                         FilePath = document.FilePath,
                                                         Status = document.Status,
                                                         Active = document.Active
                                                     }).ToList(),
                                        Locations = (from loc in contextData.PortfolioLocation
                                                     where loc.OfferingId == offering.Id
                                                     select new PortfolioLocationDto
                                                     {
                                                         Id = loc.Id,
                                                         OfferingId = loc.OfferingId,
                                                         Location = loc.Location,
                                                         Latitude = loc.Latitude,
                                                         Longitude = loc.Longitude,
                                                         Status = loc.Status,
                                                         Active = loc.Active
                                                     }).ToList(),
                                        Galleries = (from gallery in contextData.PortfolioGallery
                                                     where gallery.OfferingId == offering.Id
                                                     && gallery.Active == true
                                                     select new PortfolioGalleryDto
                                                     {
                                                         Id = gallery.Id,
                                                         OfferingId = gallery.OfferingId,
                                                         ImageUrl = gallery.ImageUrl,
                                                         Status = gallery.Status,
                                                         Active = gallery.Active
                                                     }).ToList(),
                                        Funds = (from fund in contextData.PortfolioFundingInstructions
                                                 where fund.OfferingId == offering.Id
                                                 && fund.Active == true
                                                 select new PortfolioFundingInstructionsDto
                                                 {
                                                     Id = fund.Id,
                                                     OfferingId = fund.OfferingId,
                                                     ReceivingBank = fund.ReceivingBank,
                                                     BankAddress = fund.BankAddress,
                                                     Beneficiary = fund.Beneficiary,
                                                     BeneficiaryAddress = fund.BeneficiaryAddress,
                                                     AccountType = fund.AccountType,
                                                     AccountNumber = fund.AccountNumber,
                                                     RoutingNumber = fund.RoutingNumber,
                                                     Reference = fund.Reference,
                                                     OtherInstructions = fund.OtherInstructions,
                                                     CheckBenificiary = fund.CheckBenificiary,
                                                     CheckOtherInstructions = fund.CheckOtherInstructions,
                                                     Status = fund.Status,
                                                     Active = fund.Active
                                                 }).FirstOrDefault()
                                    }).OrderByDescending(x => x.CreatedOn).ToList();
                contextData = null;
                return offeringList;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<PortfolioOfferingDto> GetOfferingsAndReservations(int userId)
        {
            try
            {
                var isAccredited = false;
                var isVerified = false;
                var contextData = _unitOfWork.PortfolioOfferingRepository.Context;
                var user = _unitOfWork.UserAccountRepository.GetByID(userId);
                if (user != null)
                {
                    if (user.IsAccreditedInvestor != null)
                        isAccredited = Convert.ToBoolean(user.IsAccreditedInvestor);
                    isVerified = user.IsEmailVerified || user.IsPhoneVerified;
                }
                var offeringVisibilityList = (from offering in contextData.PortfolioOffering
                                    where offering.Visibility != 1 && offering.Visibility != 6 // Exculdes No Users/Test(IT and Admin)
                                    join offeringVisibility in contextData.PortfolioOfferingVisibility
                                        on offering.Id equals offeringVisibility.OfferingVisibilityId                                       
                                    select new PortfolioOfferingVisibilityDto
                                    {
                                        OfferingId = offering.Id,
                                        OfferingVisibiltyId = offeringVisibility.OfferingVisibilityId,
                                        OfferingGroupId = offeringVisibility.OfferingGroupId
                                    }).Distinct().ToList();               

                contextData = null;
                return null;

            }
            catch (Exception e)
            {
                throw e;
            }
        } 

        public List<int> GetOfferingsAndReservationsAllUsers()
        {
            try
            {
                var contextData = _unitOfWork.PortfolioOfferingRepository.Context;

                var offeringList = (from offering in contextData.PortfolioOffering
                                    where (offering.Visibility == 2 && offering.Active == true)//All Users
                                    select offering.Id).ToList();
                contextData = null;
                return offeringList;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<int> GetOfferingsAndReservationsVerifiedUsers()
        {
            try
            {
                var contextData = _unitOfWork.PortfolioOfferingRepository.Context;

                var offeringList = (from offering in contextData.PortfolioOffering
                                    where (offering.Visibility == 3 && offering.Active == true)//Verified Users
                                    select offering.Id).ToList();
                contextData = null;
                return offeringList;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<int> GetOfferingsAndReservationsAccreditedUsers()
        {
            try
            {
                var contextData = _unitOfWork.PortfolioOfferingRepository.Context;

                var offeringList = (from offering in contextData.PortfolioOffering
                                    where (offering.Visibility == 4 && offering.Active == true)//Accredited Only
                                    select offering.Id).ToList();
                contextData = null;
                return offeringList;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PortfolioOfferingDto GetOfferingById(int offeringId)
        {
            try
            {
                var contextData = _unitOfWork.PortfolioOfferingRepository.Context;

                var offeringData = (from offering in contextData.PortfolioOffering
                                    where offering.Id == offeringId && offering.IsReservation == false
                                    select new PortfolioOfferingDto
                                    {
                                        Id = offering.Id,
                                        Name = offering.Name,
                                        PictureUrl = offering.PictureUrl,
                                        EntityName = offering.EntityName,
                                        Active = offering.Active,
                                        Status = offering.Status,
                                        Size = offering.Size,
                                        IsReservation = offering.IsReservation,
                                        Visibility = offering.Visibility,
                                        CreatedOn = offering.CreatedOn,
                                        KeyHighlights = (from PKH in contextData.PortfolioKeyHighlight
                                                         where PKH.OfferingId == offering.Id
                                                         join KH in contextData.KeyHighlight on PKH.KeyHighlightId equals KH.Id
                                                         select new PortfolioKeyHighlightDto
                                                         {
                                                             Id = PKH.Id,
                                                             OfferingId = PKH.OfferingId,
                                                             KeyHighLightId = PKH.KeyHighlightId,
                                                             Name = KH.Name,
                                                             Field = PKH.Field,
                                                             Value = PKH.Value,
                                                             Active = PKH.Active,
                                                             Visibility = PKH.Visibility
                                                         }).ToList(),
                                        Summary = (from summary in contextData.PortfolioSummary
                                                   where summary.OfferingId == offering.Id
                                                   select new PortfolioSummaryDto
                                                   {
                                                       Id = summary.Id,
                                                       OfferingId = summary.OfferingId,
                                                       Summary = summary.Summary,
                                                       Status = summary.Status,
                                                       Active = summary.Active
                                                   }).ToList(),
                                        Documents = (from document in contextData.Document
                                                     where document.OfferingId == offering.Id
                                                     select new PortfolioDocumentDto
                                                     {
                                                         Id = document.Id,  
                                                         OfferingId = document.OfferingId,
                                                         Name = document.Name,
                                                         FilePath = document.FilePath,
                                                         Type = document.Extension,
                                                         Status = document.Status,
                                                         Active = document.Active
                                                     }).ToList(),
                                        Locations = (from loc in contextData.PortfolioLocation
                                                     where loc.OfferingId == offering.Id
                                                     select new PortfolioLocationDto
                                                     {
                                                         Id = loc.Id,
                                                         OfferingId = loc.OfferingId,
                                                         Location = loc.Location,
                                                         Status = loc.Status,
                                                         Active = loc.Active
                                                     }).ToList(),
                                        Galleries = (from gallery in contextData.PortfolioGallery
                                                     where gallery.OfferingId == offering.Id
                                                     select new PortfolioGalleryDto
                                                     {
                                                         Id = gallery.Id,
                                                         OfferingId = gallery.OfferingId,
                                                         ImageUrl = gallery.ImageUrl,
                                                         Status = gallery.Status,
                                                         Active = gallery.Active
                                                     }).ToList(),
                                        Funds = (from fund in contextData.PortfolioFundingInstructions
                                                 where fund.OfferingId == offering.Id
                                                  && fund.Active == true
                                                 select new PortfolioFundingInstructionsDto
                                                 {
                                                     Id = fund.Id,
                                                     OfferingId = fund.OfferingId,
                                                     ReceivingBank = fund.ReceivingBank,
                                                     BankAddress = fund.BankAddress,
                                                     Beneficiary = fund.Beneficiary,
                                                     BeneficiaryAddress = fund.BeneficiaryAddress,
                                                     AccountType = fund.AccountType,
                                                     AccountNumber = fund.AccountNumber,
                                                     RoutingNumber = fund.RoutingNumber,
                                                     MailingAddress = fund.MailingAddress,
                                                     Reference = fund.Reference,
                                                     Memo = fund.Memo,
                                                     Custom = fund.Custom,
                                                     OtherInstructions = fund.OtherInstructions,
                                                     CheckBenificiary = fund.CheckBenificiary,
                                                     CheckOtherInstructions = fund.CheckOtherInstructions,
                                                     Status = fund.Status,
                                                     Active = fund.Active
                                                 }).FirstOrDefault()
                                    }).FirstOrDefault();
                contextData = null;
                return offeringData;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PortfolioOfferingDto GetReservationById(int offeringId)
        {
            try
            {
                var contextData = _unitOfWork.PortfolioOfferingRepository.Context;

                var reservationData = (from offering in contextData.PortfolioOffering
                                       where offering.Id == offeringId && offering.IsReservation == true
                                       select new PortfolioOfferingDto
                                       {
                                           Id = offering.Id,
                                           Name = offering.Name,
                                           PictureUrl = offering.PictureUrl,
                                           EntityName = offering.EntityName,
                                           Active = offering.Active,
                                           Status = offering.Status,
                                           Size = offering.Size,
                                           IsReservation = offering.IsReservation,
                                           Visibility = offering.Visibility,
                                           CreatedOn = offering.CreatedOn,
                                           KeyHighlights = (from PKH in contextData.PortfolioKeyHighlight
                                                            where PKH.OfferingId == offering.Id
                                                            join KH in contextData.KeyHighlight on PKH.KeyHighlightId equals KH.Id
                                                            select new PortfolioKeyHighlightDto
                                                            {
                                                                Id = PKH.Id,
                                                                OfferingId = PKH.OfferingId,
                                                                KeyHighLightId = PKH.KeyHighlightId,
                                                                Name = KH.Name,
                                                                Field = PKH.Field,
                                                                Value = PKH.Value,
                                                                Active = PKH.Active,
                                                                Visibility = PKH.Visibility
                                                            }).ToList(),
                                           Summary = (from summary in contextData.PortfolioSummary
                                                      where summary.OfferingId == offering.Id
                                                      select new PortfolioSummaryDto
                                                      {
                                                          Id = summary.Id,
                                                          OfferingId = summary.OfferingId,
                                                          Summary = summary.Summary,
                                                          Status = summary.Status,
                                                          Active = summary.Active
                                                      }).ToList(),
                                           Documents = (from document in contextData.Document
                                                        where document.OfferingId == offering.Id
                                                        select new PortfolioDocumentDto
                                                        {
                                                            Id = document.Id,
                                                            OfferingId = document.OfferingId,
                                                            Name = document.Name,
                                                            Type = document.Extension,
                                                            FilePath = document.FilePath,
                                                            Status = document.Status,
                                                            Active = document.Active
                                                        }).ToList(),
                                           Locations = (from loc in contextData.PortfolioLocation
                                                        where loc.OfferingId == offering.Id
                                                        select new PortfolioLocationDto
                                                        {
                                                            Id = loc.Id,
                                                            OfferingId = loc.OfferingId,
                                                            Location = loc.Location,
                                                            Status = loc.Status,
                                                            Active = loc.Active
                                                        }).ToList(),
                                           Galleries = (from gallery in contextData.PortfolioGallery
                                                        where gallery.OfferingId == offering.Id
                                                        select new PortfolioGalleryDto
                                                        {
                                                            Id = gallery.Id,
                                                            OfferingId = gallery.OfferingId,
                                                            ImageUrl = gallery.ImageUrl,
                                                            Status = gallery.Status,
                                                            Active = gallery.Active
                                                        }).ToList()                                           
                                       }).FirstOrDefault();
                contextData = null;
                return reservationData;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PortfolioOfferingDto GetOfferingDetailById(int id)
        {
            try
            {
                var contextData = _unitOfWork.PortfolioOfferingRepository.Context;

                var data = (from offering in contextData.PortfolioOffering
                            where offering.Id == id
                            select new PortfolioOfferingDto
                            {
                                Id = offering.Id,
                                Name = offering.Name,
                                PictureUrl = offering.PictureUrl,
                                EntityName = offering.EntityName,
                                Active = offering.Active,
                                Status = offering.Status,
                                Size = offering.Size,
                                IsReservation = offering.IsReservation,
                                Visibility = offering.Visibility,
                                CreatedOn = offering.CreatedOn,
                                KeyHighlights = (from PKH in contextData.PortfolioKeyHighlight
                                                 where PKH.OfferingId == offering.Id
                                                 join KH in contextData.KeyHighlight on PKH.KeyHighlightId equals KH.Id
                                                 select new PortfolioKeyHighlightDto
                                                 {
                                                     Id = PKH.Id,
                                                     OfferingId = PKH.OfferingId,
                                                     KeyHighLightId = PKH.KeyHighlightId,
                                                     Name = KH.Name,
                                                     Field = PKH.Field,
                                                     Value = PKH.Value,
                                                     Active = PKH.Active,
                                                     Visibility = PKH.Visibility
                                                 }).ToList(),
                                Summary = (from summary in contextData.PortfolioSummary
                                           where summary.OfferingId == offering.Id
                                           select new PortfolioSummaryDto
                                           {
                                               Id = summary.Id,
                                               OfferingId = summary.OfferingId,
                                               Summary = summary.Summary,
                                               Status = summary.Status,
                                               Active = summary.Active
                                           }).ToList(),
                                Documents = (from document in contextData.Document
                                             where document.OfferingId == offering.Id
                                             select new PortfolioDocumentDto
                                             {
                                                 Id = document.Id,   
                                                 OfferingId = document.OfferingId,
                                                 Name = document.Name,
                                                 FilePath = document.FilePath,
                                                 Type = document.Extension,
                                                 Status = document.Status,
                                                 Active = document.Active
                                             }).ToList(),
                                Locations = (from loc in contextData.PortfolioLocation
                                             where loc.OfferingId == offering.Id
                                             select new PortfolioLocationDto
                                             {
                                                 Id = loc.Id,
                                                 OfferingId = loc.OfferingId,
                                                 Location = loc.Location,
                                                 Status = loc.Status,
                                                 Active = loc.Active
                                             }).ToList(),
                                Galleries = (from gallery in contextData.PortfolioGallery
                                             where gallery.OfferingId == offering.Id
                                             select new PortfolioGalleryDto
                                             {
                                                 Id = gallery.Id,
                                                 OfferingId = gallery.OfferingId,
                                                 ImageUrl = gallery.ImageUrl,
                                                 Status = gallery.Status,
                                                 Active = gallery.Active
                                             }).ToList(),
                                Funds = (from fund in contextData.PortfolioFundingInstructions
                                         where fund.OfferingId == offering.Id
                                          && fund.Active == true
                                         select new PortfolioFundingInstructionsDto
                                         {
                                             Id = fund.Id,
                                             OfferingId = fund.OfferingId,
                                             ReceivingBank = fund.ReceivingBank,
                                             BankAddress = fund.BankAddress,
                                             Beneficiary = fund.Beneficiary,
                                             BeneficiaryAddress = fund.BeneficiaryAddress,
                                             AccountType = fund.AccountType,
                                             AccountNumber = fund.AccountNumber,
                                             RoutingNumber = fund.RoutingNumber,
                                             MailingAddress = fund.MailingAddress,
                                             Reference = fund.Reference,
                                             Memo = fund.Memo,
                                             Custom = fund.Custom,
                                             OtherInstructions = fund.OtherInstructions,
                                             CheckBenificiary = fund.CheckBenificiary,
                                             CheckOtherInstructions = fund.CheckOtherInstructions,
                                             Status = fund.Status,
                                             Active = fund.Active
                                         }).FirstOrDefault()
                            }).FirstOrDefault();
                contextData = null;
                return data;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int AddInvestment(InvestmentDto investmentDto)
        {
            if (investmentDto != null)
            {
                try
                {
                    using (var transaction = _unitOfWork.InvestmentRepository.Context.Database.BeginTransaction())
                    {

                        Investment investment = _mapper.Map<Investment>(investmentDto);
                        investment.IsReservation = false;
                        investment.CreatedBy = investmentDto.UserId.ToString();
                        investment.CreatedOn = DateTime.Now;
                        investment.Active = true;
                        _unitOfWork.InvestmentRepository.Insert(investment);
                        _unitOfWork.Save();
                        transaction.Commit();
                        var offering = _unitOfWork.PortfolioOfferingRepository.GetByID(investmentDto.OfferingId);                        
                        string message = "You added investment in offering " + offering.Name; 
                        var notification = _notificationRepository.AddNotification(investmentDto.UserId, "Investment added", message);
                        if (notification != null)
                        {
                            _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                        }
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
        public int AddReservation(InvestmentDto ReservationDto)
        {
            if (ReservationDto != null)
            {
                try
                {
                    using (var transaction = _unitOfWork.InvestmentRepository.Context.Database.BeginTransaction())
                    {

                        Investment investment = _mapper.Map<Investment>(ReservationDto);
                        investment.IsReservation = true;
                        investment.Status = 2;//Pending Reservation
                        investment.Active = true;
                        investment.ConfidenceLevel = 1;//Very likely when created by investor
                        investment.CreatedBy = ReservationDto.UserId.ToString();
                        investment.CreatedOn = DateTime.Now;
                        _unitOfWork.InvestmentRepository.Insert(investment);
                        _unitOfWork.Save();
                        transaction.Commit();
                        var offering = _unitOfWork.PortfolioOfferingRepository.GetByID(ReservationDto.OfferingId);
                        string message = "You added investment in reservation " + offering.Name;
                        var notification = _notificationRepository.AddNotification(ReservationDto.UserId, "Investment added", message);
                        if (notification != null)
                        {
                            _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                        }
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
        public int UpdateInvestment(InvestmentDto investmentDto)
        {
            var investmentData = _unitOfWork.InvestmentRepository.Get(x => x.Id == investmentDto.Id);
            if (investmentData.Id != 0 && investmentData.IsReservation == false)
            {
                try
                {
                    using (var transaction = _unitOfWork.InvestorRepository.Context.Database.BeginTransaction())
                    {
                        Investment investment = investmentData;
                        //investment.Amount = investmentDto.Amount;
                        //investment.UserProfileId = investmentDto.UserProfileId;                                   
                        investment.Status = investmentDto.Status;
                        if (investmentDto.IseSignCompleted == true)
                        {
                            investment.IseSignCompleted = investmentDto.IseSignCompleted;
                            investment.DocumenteSignedDate = DateTime.Now;
                            investment.eSignedDocumentPath = "eSignedDocumen1.pdf";//To be done
                        }
                        if (investmentDto.WireTransferDate != null)
                        {
                            investment.WireTransferDate = investmentDto.WireTransferDate;
                        }
                        investment.Active = true;
                        investment.ModifiedBy = investmentDto.UserId.ToString();
                        _unitOfWork.InvestmentRepository.Update(investment);
                        _unitOfWork.Save();
                        transaction.Commit();
                        var offering = _unitOfWork.PortfolioOfferingRepository.GetByID(investmentDto.OfferingId);
                        string message = "You edited investment in offering " + offering.Name;
                        var notification = _notificationRepository.AddNotification(investmentDto.UserId, "Investment updated", message);
                        if (notification != null)
                        {
                            _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                        }
                        using (var userAccountTransaction = _unitOfWork.UserAccountRepository.Context.Database.BeginTransaction())
                        {
                            var userAccount = _unitOfWork.UserAccountRepository.Get(x => x.Id == investmentDto.UserId);
                            if (userAccount != null && userAccount.RoleId == 2) // Lead
                            {
                                //Change investor to lead when investment made
                                UserAccount userAccountData = userAccount;
                                userAccountData.RoleId = 1; // Investor
                                userAccountData.ModifiedBy = investmentDto.UserId.ToString();
                                _unitOfWork.UserAccountRepository.Update(userAccount);
                                _unitOfWork.Save();
                                userAccountTransaction.Commit();
                            }
                            message = "You have been updated to investor from lead";
                            notification = _notificationRepository.AddNotification(investmentDto.UserId, "User updated to investor from lead", message);
                            if (notification != null)
                            {
                                _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                            }
                        }
                    }
                    return 1;
                }
                catch
                {
                    return 0;
                }
                finally
                {
                    investmentData = null;
                }
            }
            return 0;

        }
        public int UpdateReservation(InvestmentDto investmentDto)
        {
            var investmentData = _unitOfWork.InvestmentRepository.Get(x => x.Id == investmentDto.Id);
            if (investmentData.Id != 0 && investmentData.IsReservation == true)
            {
                try
                {
                    using (var transaction = _unitOfWork.InvestorRepository.Context.Database.BeginTransaction())
                    {
                        Investment investment = investmentData;
                        investment.Amount = investmentDto.Amount;
                        investment.UserProfileId = investmentDto.UserProfileId;
                        investment.Status = investmentDto.Status;
                        investment.Active = investmentDto.Active;
                        investment.ModifiedBy = investmentDto.UserId.ToString();
                        _unitOfWork.InvestmentRepository.Update(investment);
                        _unitOfWork.Save();
                        transaction.Commit();
                        var offering = _unitOfWork.PortfolioOfferingRepository.GetByID(investmentDto.OfferingId);
                        string message = "You edited investment in reservation " + offering.Name;
                        var notification = _notificationRepository.AddNotification(investmentDto.UserId, "Investment updated", message);
                        if (notification != null)
                        {
                            _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                        }
                    }
                    return 1;
                }
                catch(Exception e)
                {
                    e.ToString();
                    return 0;
                }
                finally
                {
                    investmentData = null;
                }
            }
            return 0;

        }
        public InvestmentDto GetInvestmentDetailById(int userId, int investmentId)
        {
            try
            {
                var contextData = _unitOfWork.InvestmentRepository.Context;

                var investmentData = (from investment in contextData.Investment
                                      where investment.Id == investmentId && investment.UserId == userId && investment.IsReservation == false
                                      select new InvestmentDto
                                      {
                                          Id = investment.Id,
                                          UserId = investment.UserId,
                                          UserProfileId = investment.UserProfileId,
                                          OfferingId = investment.OfferingId,
                                          Amount = investment.Amount,
                                          FundedDate = investment.FundedDate,
                                          Status = investment.Status,
                                          IsConfirmed = investment.IsConfirmed,
                                          IseSignCompleted = investment.IseSignCompleted,
                                          WireTransferDate = investment.WireTransferDate,
                                          Active = investment.Active,
                                          CreatedOn = investment.CreatedOn,
                                          CreatedBy = investment.CreatedBy,
                                          ModifiedOn = investment.ModifiedOn,
                                          ModifiedBy = investment.ModifiedBy,
                                      }).FirstOrDefault();
                contextData = null;
                return investmentData;

            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }
        public List<InvestmentDto> GetReservationDetailById(int userId)
        {
            try
            {
                var contextData = _unitOfWork.InvestmentRepository.Context;

                var reservationData = (from reservation in contextData.Investment
                                       where reservation.UserId == userId && reservation.IsReservation == true
                                       select new InvestmentDto
                                       {
                                           Id = reservation.Id,
                                           UserId = reservation.UserId,
                                           UserProfileId = reservation.UserProfileId,
                                           OfferingId = reservation.OfferingId,
                                           Amount = reservation.Amount,
                                           FundedDate = reservation.FundedDate,
                                           Status = reservation.Status,
                                           IsConfirmed = reservation.IsConfirmed,
                                           IseSignCompleted = reservation.IseSignCompleted,
                                           WireTransferDate = reservation.WireTransferDate,
                                           Active = reservation.Active,
                                           CreatedOn = reservation.CreatedOn,
                                           CreatedBy = reservation.CreatedBy,
                                           ModifiedOn = reservation.ModifiedOn,
                                           ModifiedBy = reservation.ModifiedBy,
                                       }).ToList();
                contextData = null;
                return reservationData;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }

        }
        public PortfolioFundingInstructionsDto GetPortfolioFundingInstructions(int offeringId)
        {
            try
            {
                var contextData = _unitOfWork.PortfolioOfferingRepository.Context;

                var Funds = (from fund in contextData.PortfolioFundingInstructions
                             where fund.OfferingId == offeringId
                              && fund.Active == true
                             select new PortfolioFundingInstructionsDto
                             {
                                 Id = fund.Id,
                                 OfferingId = fund.OfferingId,
                                 ReceivingBank = fund.ReceivingBank,
                                 BankAddress = fund.BankAddress,
                                 Beneficiary = fund.Beneficiary,
                                 BeneficiaryAddress = fund.BeneficiaryAddress,
                                 AccountType = fund.AccountType,
                                 AccountNumber = fund.AccountNumber,
                                 RoutingNumber = fund.RoutingNumber,
                                 MailingAddress = fund.MailingAddress,
                                 Reference = fund.Reference,
                                 Memo = fund.Memo,
                                 Custom = fund.Custom,
                                 CheckBenificiary = fund.CheckBenificiary,
                                 CheckOtherInstructions = fund.CheckOtherInstructions,
                                 OtherInstructions = fund.OtherInstructions,
                                 Status = fund.Status,
                                 Active = fund.Active
                             }).FirstOrDefault();
                contextData = null;
                return Funds;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }
    }
}
