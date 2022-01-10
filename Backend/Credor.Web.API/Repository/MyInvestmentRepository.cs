using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Web.API.Common.UnitOfWork;
using AutoMapper;
using Credor.Data.Entities;
using Credor.Client.Entities;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class MyInvestmentRepository : IMyInvestmentRepository
    {
        private readonly UnitOfWork _unitOfWork;
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public MyInvestmentRepository(IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = new UnitOfWork(_configuration);
            _mapper = mapper;
        }
        public InvestmentSummaryDto GetHeaderElements(int userId)
        {
            InvestmentSummaryDto elements = null;
            var contextData = _unitOfWork.InvestmentRepository.Context;
            try
            {
                elements = (from summary in contextData.InvestmentSummary
                            where summary.UserId == userId
                            select new InvestmentSummaryDto
                            {
                                UserId = summary.UserId,
                                ActiveInvestments = summary.ActiveInvestments == null ? 0 : summary.ActiveInvestments,
                                PendingInvestments = summary.PendingInvestments == null ? 0 : summary.PendingInvestments,
                                TotalInvested = summary.TotalInvested,
                                TotalEarnings = summary.TotalEarnings,
                                TotalReturn = summary.TotalReturn
                            }
                            ).FirstOrDefault();
                if (elements == null)
                {
                    elements = new InvestmentSummaryDto();
                    elements.ActiveInvestments = (from investment in contextData.Investment
                                                  where investment.UserId == userId
                                                  && investment.Status == 1// Approved by admin
                                                  && investment.Active == true // Active Investment
                                                  select investment.Id).Count();
                    elements.PendingInvestments = (from investment in contextData.Investment
                                                   where investment.UserId == userId
                                                   && (investment.Status == 3 // Pending Investor Signature and Funding
                                                    || investment.Status == 4) // Pending Funding
                                                   && investment.Active == true // Active Investment
                                                   select investment.Id).Count();
                    elements.TotalInvested = (from investment in contextData.Investment
                                              where investment.UserId == userId
                                              && investment.Status == 1// Approved by admin
                                              && investment.Active == true // Active Investment
                                              select investment.Amount).Sum();

                    elements.TotalEarnings = (from payment in contextData.Payment
                                              join investment in contextData.Investment on payment.InvestmentId equals investment.Id
                                              where investment.UserId == userId
                                              && payment.Type == 1 // Credit payment                                         
                                              select investment.Amount).Sum();
                    if (elements.TotalEarnings > elements.TotalInvested)
                        elements.TotalReturn = ((elements.TotalEarnings / elements.TotalInvested) * 100) - 100;
                    else
                        elements.TotalReturn = ((elements.TotalEarnings / elements.TotalInvested) * 100);
                    return elements;
                }
                else
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
        public List<InvestmentStatusDto> GetInvestmentStatuses()
        {
            List<InvestmentStatusDto> statuses = new List<InvestmentStatusDto>();
            var contextData = _unitOfWork.InvestmentStatusRepository.Context;
            try
            {
                statuses = (from status in contextData.InvestmentStatus
                           select new InvestmentStatusDto {
                                Id = status.Id,
                                Name = status.Name,
                                Active = status.Active
                           }).ToList();
            }
            catch(Exception e)
            {
                e.ToString();
                return statuses;
            }
            finally 
            {
                contextData = null;
            }
            return statuses;
        }                       
        public List<MyInvestmentDto> GetInvestmentAndReservationsByUserId(int userId)
        {
            try
            {
                var contextData = _unitOfWork.InvestmentRepository.Context;

                var investmentData = (from investment in contextData.Investment
                                      join userprofile in contextData.UserProfile on investment.UserProfileId equals userprofile.Id
                                      join offering in contextData.PortfolioOffering on investment.OfferingId equals offering.Id
                                      where investment.UserId == userId && investment.Active == true
                                      select new MyInvestmentDto
                                      {
                                          Id = investment.Id,
                                          UserId = investment.UserId,
                                          UserProfileId = investment.UserProfileId,
                                          ProfileDisplayId = userprofile.DisplayId,
                                          Amount = investment.Amount,
                                          TotalEarnings = (from payment in contextData.Payment                                                    
                                                    where payment.InvestmentId == investment.Id
                                                    && payment.Type == 1 // Credit payment                                         
                                                    select payment.Amount).Sum(),
                                          FundedDate = investment.FundedDate,
                                          Status = investment.Status,
                                          IsConfirmed = investment.IsConfirmed,                                         
                                          IsReservation = investment.IsReservation,
                                          IseSignCompleted = investment.IseSignCompleted,  
                                          WireTransferDate = investment.WireTransferDate,
                                          Active = investment.Active,
                                          CreatedOn = investment.CreatedOn,
                                          CreatedBy = investment.CreatedBy,
                                          ModifiedOn = investment.ModifiedOn,
                                          ModifiedBy = investment.ModifiedBy,
                                          ApprovedBy = investment.ApprovedBy,
                                          ApprovedOn = investment.ApprovedOn,
                                          OfferingId = offering.Id,
                                          OfferingName = offering.Name,
                                          OfferingEntityName = offering.EntityName,
                                          OfferingPictureUrl = offering.PictureUrl,
                                          UserProfile = (from profile in contextData.UserProfile where profile.Id == investment.UserProfileId
                                                         select new UserProfileDto 
                                                         {
                                                             Id= profile.Id,
                                                             Type = profile.Type,
                                                             Name = profile.Name,
                                                             FirstName = profile.FirstName,
                                                             LastName = profile.LastName,
                                                             RetirementPlanName = profile.RetirementPlanName,
                                                             TrustName = profile.TrustName                                                         
                                                         }).FirstOrDefault(),
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
                                                           Latitude = loc.Latitude,
                                                           Longitude = loc.Longitude,
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
                                                   }).FirstOrDefault(),
                                        Updates = (from update in contextData.PortfolioUpdates
                                                     where update.InvestmentId == investment.Id
                                                     select new PortfolioUpdatesDto
                                                     {
                                                         Id = update.Id,
                                                         OfferingId = investment.OfferingId,
                                                         Name = offering.Name,
                                                         Subject = update.Subject,
                                                         Content = update.Content,
                                                         ImageUrl = update.ImageUrl,
                                                         CreatedOn = update.CreatedOn,
                                                         CreatedBy = update.CreatedBy,
                                                         Status = update.Status,
                                                         Active = update.Active
                                                     }).ToList()
                                      }).ToList();
                contextData = null;
                return investmentData;

            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }
        public List<MyInvestmentDto> GetInvestmentListByUserId(int userId)
        {
            try
            {
                var contextData = _unitOfWork.InvestmentRepository.Context;

                var investmentData = (from investment in contextData.Investment
                                      join userprofile in contextData.UserProfile on investment.UserProfileId equals userprofile.Id
                                      join offering in contextData.PortfolioOffering on investment.OfferingId equals offering.Id
                                      where investment.UserId == userId && investment.IsReservation == false
                                      select new MyInvestmentDto
                                      {
                                          Id = investment.Id,
                                          UserId = investment.UserId,
                                          UserProfileId = investment.UserProfileId,
                                          ProfileDisplayId = userprofile.DisplayId,
                                          Amount = investment.Amount,
                                          FundedDate = investment.FundedDate,
                                          Status = investment.Status,
                                          IsReservation = investment.IsReservation,
                                          IsConfirmed = investment.IsConfirmed,
                                          IseSignCompleted = investment.IseSignCompleted,
                                          WireTransferDate = investment.WireTransferDate,                                          
                                          Active = investment.Active,
                                          CreatedOn = investment.CreatedOn,
                                          CreatedBy = investment.CreatedBy,
                                          ModifiedOn = investment.ModifiedOn,
                                          ModifiedBy = investment.ModifiedBy,
                                          ApprovedOn = investment.ApprovedOn,
                                          ApprovedBy = investment.ApprovedBy,
                                          OfferingId = offering.Id,
                                          OfferingName = offering.Name,
                                          OfferingEntityName = offering.EntityName,
                                          OfferingPictureUrl = offering.PictureUrl,
                                          UserProfile = (from profile in contextData.UserProfile
                                                         where profile.Id == investment.UserProfileId
                                                         select new UserProfileDto
                                                         {
                                                             Id = profile.Id,
                                                             Type = profile.Type,
                                                             Name = profile.Name,
                                                             FirstName = profile.FirstName,
                                                             LastName = profile.LastName,
                                                             RetirementPlanName = profile.RetirementPlanName,
                                                             TrustName = profile.TrustName
                                                         }).FirstOrDefault(),
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
                                                   }).FirstOrDefault(),
                Updates = (from update in contextData.PortfolioUpdates
                                                     where update.InvestmentId == investment.Id
                                                     select new PortfolioUpdatesDto
                                                     {
                                                         Id = update.Id,
                                                         OfferingId = investment.OfferingId,
                                                         Name = offering.Name,
                                                         Subject = update.Subject,
                                                         Content = update.Content,
                                                         ImageUrl = update.ImageUrl,
                                                         CreatedOn = update.CreatedOn,
                                                         CreatedBy = update.CreatedBy,
                                                         Status = update.Status,
                                                         Active = update.Active
                                                     }).ToList()
                                      }).ToList();
                contextData = null;
                return investmentData;

            }
            catch (Exception e)
            { 
                e.ToString();
                return null;
            }
        }
        public List<MyInvestmentDto> GetReservationListByUserId(int userId)
        {
            try
            {
                var contextData = _unitOfWork.InvestmentRepository.Context;

                var reservationData = (from reservation in contextData.Investment
                                      join userprofile in contextData.UserProfile on reservation.UserProfileId equals userprofile.Id
                                      join offering in contextData.PortfolioOffering on reservation.OfferingId equals offering.Id
                                      where reservation.UserId == userId && reservation.IsReservation == true
                                      select new MyInvestmentDto
                                      {
                                          Id = reservation.Id,
                                          UserId = reservation.UserId,
                                          UserProfileId = reservation.UserProfileId,
                                          ProfileDisplayId = userprofile.DisplayId,
                                          Amount = reservation.Amount,
                                          FundedDate = reservation.FundedDate,
                                          Status = reservation.Status,
                                          IsReservation = reservation.IsReservation,
                                          IsConfirmed = reservation.IsConfirmed,
                                          IseSignCompleted = reservation.IseSignCompleted,
                                          WireTransferDate = reservation.WireTransferDate,
                                          Active = reservation.Active,
                                          CreatedOn = reservation.CreatedOn,
                                          CreatedBy = reservation.CreatedBy,
                                          ModifiedOn = reservation.ModifiedOn,
                                          ModifiedBy = reservation.ModifiedBy,
                                          ApprovedOn = reservation.ApprovedOn,
                                          ApprovedBy = reservation.ApprovedBy,
                                          OfferingId = offering.Id,
                                          OfferingName = offering.Name,
                                          OfferingEntityName = offering.EntityName,
                                          OfferingPictureUrl = offering.PictureUrl,
                                          UserProfile = (from profile in contextData.UserProfile
                                                         where profile.Id == reservation.UserProfileId
                                                         select new UserProfileDto
                                                         {
                                                             Id = profile.Id,
                                                             Type = profile.Type,
                                                             Name = profile.Name,
                                                             FirstName = profile.FirstName,
                                                             LastName = profile.LastName,
                                                             RetirementPlanName = profile.RetirementPlanName,
                                                             TrustName = profile.TrustName
                                                         }).FirstOrDefault(),
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
                                                   }).FirstOrDefault(),
                Updates = (from update in contextData.PortfolioUpdates
                                                     where update.InvestmentId == reservation.Id
                                                     select new PortfolioUpdatesDto
                                                     {
                                                         Id = update.Id,
                                                         OfferingId = reservation.OfferingId,
                                                         Name = offering.Name,
                                                         Subject = update.Subject,
                                                         Content = update.Content,
                                                         ImageUrl = update.ImageUrl,
                                                         CreatedOn = update.CreatedOn,
                                                         CreatedBy = update.CreatedBy,
                                                         Status = update.Status,
                                                         Active = update.Active
                                                     }).ToList()
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
        public MyInvestmentDto GetInvestmentDetailById(int userId, int investmentId)
        {
            try
            {
                var contextData = _unitOfWork.InvestmentRepository.Context;

                var investmentData = (from investment in contextData.Investment
                                      where investment.Id == investmentId && investment.UserId == userId && investment.IsReservation == false
                                      join userprofile in contextData.UserProfile on investment.UserProfileId equals userprofile.Id
                                      join offering in contextData.PortfolioOffering on investment.OfferingId equals offering.Id
                                      select new MyInvestmentDto
                                      {
                                          Id = investment.Id,
                                          UserId = investment.UserId,
                                          UserProfileId = investment.UserProfileId,
                                          ProfileDisplayId = userprofile.DisplayId,
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
                                          ApprovedBy = investment.ApprovedBy,
                                          ApprovedOn = investment.ApprovedOn,
                                          OfferingName = offering.Name,
                                          OfferingEntityName = offering.EntityName,
                                          OfferingPictureUrl = offering.PictureUrl,
                                          UserProfile = (from profile in contextData.UserProfile
                                                         where profile.Id == investment.UserProfileId
                                                         select new UserProfileDto
                                                         {
                                                             Id = profile.Id,
                                                             Type = profile.Type,
                                                             Name = profile.Name,
                                                             FirstName = profile.FirstName,
                                                             LastName = profile.LastName,
                                                             RetirementPlanName = profile.RetirementPlanName,
                                                             TrustName = profile.TrustName
                                                         }).FirstOrDefault(),
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
                                                   }).FirstOrDefault(),
                                          Updates = (from update in contextData.PortfolioUpdates
                                                     where update.InvestmentId == investment.Id
                                                     select new PortfolioUpdatesDto
                                                     {
                                                         Id = update.Id,
                                                         OfferingId = investment.OfferingId,
                                                         Name = offering.Name,
                                                         Subject = update.Subject,
                                                         Content = update.Content,
                                                         ImageUrl = update.ImageUrl,
                                                         CreatedOn = update.CreatedOn,
                                                         CreatedBy = update.CreatedBy,
                                                         Status = update.Status,
                                                         Active = update.Active
                                                     }).ToList()
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
        public MyInvestmentDto GetReservationDetailById(int userId, int reservationId)
        {
            try
            {
                var contextData = _unitOfWork.InvestmentRepository.Context;

                var reservationData = (from reservation in contextData.Investment
                                      where reservation.Id == reservationId && reservation.UserId == userId && reservation.IsReservation == true
                                      join userprofile in contextData.UserProfile on reservation.UserProfileId equals userprofile.Id
                                      join offering in contextData.PortfolioOffering on reservation.OfferingId equals offering.Id
                                      select new MyInvestmentDto
                                      {
                                          Id = reservation.Id,
                                          UserId = reservation.UserId,
                                          UserProfileId = reservation.UserProfileId,
                                          ProfileDisplayId = userprofile.DisplayId,
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
                                          ApprovedOn = reservation.ApprovedOn,
                                          ApprovedBy = reservation.ApprovedBy,
                                          OfferingName = offering.Name,
                                          OfferingEntityName = offering.EntityName,
                                          OfferingPictureUrl = offering.PictureUrl,
                                          UserProfile = (from profile in contextData.UserProfile
                                                         where profile.Id == reservation.UserProfileId
                                                         select new UserProfileDto
                                                         {
                                                             Id = profile.Id,
                                                             Type = profile.Type,
                                                             Name = profile.Name,
                                                             FirstName = profile.FirstName,
                                                             LastName = profile.LastName,
                                                             RetirementPlanName = profile.RetirementPlanName,
                                                             TrustName = profile.TrustName
                                                         }).FirstOrDefault(),
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
                                                       }).ToList(),                                          
                                          Updates = (from update in contextData.PortfolioUpdates
                                                     where update.InvestmentId == reservation.Id
                                                     select new PortfolioUpdatesDto
                                                     {
                                                         Id = update.Id,
                                                         OfferingId = reservation.OfferingId,
                                                         Name = offering.Name,
                                                         Subject = update.Subject,
                                                         Content = update.Content,
                                                         ImageUrl = update.ImageUrl,
                                                         CreatedOn = update.CreatedOn,
                                                         CreatedBy = update.CreatedBy,
                                                         Status = update.Status,
                                                         Active = update.Active
                                                     }).ToList()
                                      }).FirstOrDefault();
                contextData = null;
                return reservationData;

            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }
    }
}
