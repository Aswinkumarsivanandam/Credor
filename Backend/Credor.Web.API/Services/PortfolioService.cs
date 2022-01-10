using Credor.Web.API.Common.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using System.IO;
using System.Net.Mail;
using System.Text;
using Credor.Web.API.Shared;

namespace Credor.Web.API
{
    public class PortfolioService : IPortfolioService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IPortfolioRepository _portfolioRepository;

        public IConfiguration _configuration { get; }
        public PortfolioService(IPortfolioRepository portfolioRepository,
                                IConfiguration configuration)
        {
            _unitOfWork = new UnitOfWork(configuration);
            _configuration = configuration;
            _portfolioRepository = portfolioRepository;

        }
        public List<PortfolioOfferingDto> GetPortfolioOfferings()
        {
            return _portfolioRepository.GetPortfolioOfferings();
        }

        public List<PortfolioOfferingDto> GetPortfolioReservations()
        {
            return _portfolioRepository.GetPortfolioReservations();
        }

        public PortfolioOfferingDto GetPortfolioOffering(int offeringId)
        {
            return _portfolioRepository.GetPortfolioOffering(offeringId);
        }

        public PortfolioOfferingDto GetPortfolioReservation(int reservationId)
        {
            return _portfolioRepository.GetPortfolioReservation(reservationId);
        }

        public int AddPortfolioOffering(PortfolioOfferingDto portfolioOffering)
        {
            return _portfolioRepository.AddPortfolioOffering(portfolioOffering);
        }

        public int AddPortfolioReservation(PortfolioOfferingDto portfolioReservation)
        {
            return _portfolioRepository.AddPortfolioReservation(portfolioReservation);
        }

        public int UpdatePortfolioOffering(PortfolioOfferingDto portfolioOffering)
        {
            return _portfolioRepository.UpdatePortfolioOffering(portfolioOffering);
        }

        public int UpdatePortfolioReservation(PortfolioOfferingDto portfolioReservation)
        {
            return _portfolioRepository.UpdatePortfolioReservation(portfolioReservation);
        }

        public bool DeletePortfolioOffering(int adminUserId, int offeringId)
        {
            return _portfolioRepository.DeletePortfolioOffering(adminUserId, offeringId);
        }

        public bool DeletePortfolioReservation(int adminUserId, int reservationId)
        {
            return _portfolioRepository.DeletePortfolioReservation(adminUserId, reservationId);
        }
        public List<PortfolioOfferingDto> GetArchivedPortfolioOfferings()
        {
            return _portfolioRepository.GetArchivedPortfolioOfferings();
        }
        public List<PortfolioOfferingDto> GetArchivedPortfolioOfferingAndReservations()
        {
            return _portfolioRepository.GetArchivedPortfolioOfferingAndReservations();
        }
        public bool RestorePortfolioOffering(int AdminUserId, int offeringId)
        {
            return _portfolioRepository.RestorePortfolioOffering(AdminUserId, offeringId);
        }
        public bool RestorePortfolioReservation(int AdminUserId, int reservationId)
        {
            return _portfolioRepository.RestorePortfolioReservation(AdminUserId, reservationId);
        }
        public Task<AddPortfolioGalleryResultDto> AddPortfolioGalleryImages(AddPortfolioGalleryDto portfolioGalleryDto)
        {
            return _portfolioRepository.AddPortfolioGalleryImages(portfolioGalleryDto);
        }
        public UpdatePortfolioGalleryResultDto UpdatePortfolioGalleryImage(PortfolioGalleryDto portfolioGalleryDto)
        {
            return _portfolioRepository.UpdatePortfolioGalleryImage(portfolioGalleryDto);
        }
        public PortfolioSummaryDto AddPortfolioSummary(PortfolioSummaryDto portfolioSummaryDto)
        {
            return _portfolioRepository.AddPortfolioSummary(portfolioSummaryDto);
        }
        public PortfolioSummaryDto UpdatePortfolioSummary(PortfolioSummaryDto portfolioSummaryDto)
        {
            return _portfolioRepository.UpdatePortfolioSummary(portfolioSummaryDto);
        }
        public Task<PortfolioDocumentsResultDto> UploadOfferingDocuments(DocumentModelDto documentDto)
        {
            return _portfolioRepository.UploadOfferingDocuments(documentDto);
        }
        public PortfolioDocumentsResultDto DeleteOfferingDocument(int adminuserid, int offeringId, int documentId)
        {
            return _portfolioRepository.DeleteOfferingDocument(adminuserid,offeringId, documentId);
        }
        public bool AddDefaultPortfolioKeyHighlights(int id,int adminUserId)
        {
            return _portfolioRepository.AddDefaultPortfolioKeyHighlights(id,adminUserId);
        }
        public UpdatePortfolioKeyHightlightDto UpdatePortfolioKeyHightlight(UpdatePortfolioKeyHightlightDto updatePortfolioKeyHightlight)
        {
            return _portfolioRepository.UpdatePortfolioKeyHightlight(updatePortfolioKeyHightlight);
        }
        public PortfolioLocationDto AddPortfolioLocation(PortfolioLocationDto portfolioLocation)
        {
            return _portfolioRepository.AddPortfolioLocation(portfolioLocation);
        }
        public PortfolioLocationDto UpdatePortfolioLocation(PortfolioLocationDto portfolioLocation)
        {
            return _portfolioRepository.UpdatePortfolioLocation(portfolioLocation);
        }
        public PortfolioFundingInstructionsDto AddPortfolioFundingInstructions(PortfolioFundingInstructionsDto portfolioFundingInstructions)
        {
            return _portfolioRepository.AddPortfolioFundingInstructions(portfolioFundingInstructions);
        }
        public PortfolioFundingInstructionsDto UpdatePortfolioFundingInstructions(PortfolioFundingInstructionsDto portfolioFundingInstructions)
        {
            return _portfolioRepository.UpdatePortfolioFundingInstructions(portfolioFundingInstructions);
        }
        public PortfolioInvestmentsSummaryDto GetPortfolioInvestorsSummary(int offeringId)
        {
            return _portfolioRepository.GetPortfolioInvestorsSummary(offeringId);
        }
        public List<PortfolioInvestmentDataDto> GetPortfolioOfferingInvestments(int offeringId)
        {
            return _portfolioRepository.GetPortfolioOfferingInvestments(offeringId);
        }
        public List<PortfolioReservationDataDto> GetPortfolioOfferingReservations(int offeringId)
        {
            return _portfolioRepository.GetPortfolioOfferingReservations(offeringId);
        }
        public PortfolioReservationsSummaryDto GetPortfolioReservationsSummary(int offeringId)
        {
            return _portfolioRepository.GetPortfolioReservationsSummary(offeringId);
        }
        public ReservationsSummaryDto GetReservationsSummary(int reservationId)
        {
            return _portfolioRepository.GetReservationsSummary(reservationId);
        }
        public bool AddInvestmentNotes(InvestmentNotesDto investmentNotesDto)
        {
            return _portfolioRepository.AddInvestmentNotes(investmentNotesDto);
        }
        public bool AddReservationNotes(ReservationNotesDto reservationNotes)
        {
            return _portfolioRepository.AddReservationNotes(reservationNotes);
        }
        public List<InvestmentStatusDto> GetInvestmentStatuses()
        {
            return _portfolioRepository.GetInvestmentStatuses();
        }
        public List<PortfolioReservationDataDto> GetReservationsList(int reservationId)
        {
            return _portfolioRepository.GetReservationsList(reservationId);
        }
        public CapTableDataDto GetPortfolioOfferingCapTable(int offeringid)
        {
            return _portfolioRepository.GetPortfolioOfferingCapTable(offeringid);
        }
        public bool AddOfferingDistributions(OfferingDistributionDto distributionsData)
        {
            var result = _portfolioRepository.AddOfferingDistributions(distributionsData);
            if(distributionsData.IsNotify)
            {
                var offeringName = _unitOfWork.PortfolioOfferingRepository.GetByID(distributionsData.OfferingId).Name;
                foreach (var distribution in distributionsData.Distributions)
                {                    
                    var userAccount = _unitOfWork.UserAccountRepository.GetByID(distribution.UserId);
                    if (userAccount != null)
                    {
                        SendEmail(userAccount.EmailId, distribution.InvestorName, offeringName, distribution.ProfileName);
                    }
                }
            }
            return true;
        }
        private bool SendEmail(string EmailId, string investorName, string offeringName, string profileName)
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

                var emailTemplate = _unitOfWork.CredorEmailTemplateRepository.GetWithInclude(x => x.TemplateName.ToLower() == "DistributionAdded".ToLower()).FirstOrDefault();

                mail.Subject = emailTemplate.Subject;
                StringBuilder msgBody = new StringBuilder(emailTemplate.BodyContent);

                msgBody.Replace("{{Name}}", investorName);
                msgBody.Replace("{{offeringname}}", offeringName);
                msgBody.Replace("{{profilename}}", profileName);
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
        public OfferingDistributionDto ConfirmDistributions(int offeringDistributionId, int adminuserid)
        {
            return _portfolioRepository.ConfirmDistributions(offeringDistributionId, adminuserid);
        }
        public List<PortfolioDistributionTypeDto> GetPortfolioDistributionTypes()
        {
            return _portfolioRepository.GetPortfolioDistributionTypes();
        }
        public List<OfferingDistributionDto> GetOfferingDistributions(int offeringId)
        {
            return _portfolioRepository.GetOfferingDistributions(offeringId);
        }
        public OfferingDistributionDto UpdateOfferingDistribution(OfferingDistributionDto offeringDistributionDto)
        {
            return _portfolioRepository.UpdateOfferingDistribution(offeringDistributionDto);
        }
        public bool DeleteOfferingDistribution(int offeringDistributionId, int adminUserId)
        {
            return _portfolioRepository.DeleteOfferingDistribution(offeringDistributionId, adminUserId);
        }
        public bool UpdateCapTable(UpdateCapTableDto capTableDto)
        {
            return _portfolioRepository.UpdateCapTable(capTableDto);
        }
        public OfferingDistributionDto GetOfferingDistributionDetail(int offeringDistributionId)
        {
            return _portfolioRepository.GetOfferingDistributionDetail(offeringDistributionId);
        }      
        public List<CapTableInvestorDto> GetInvestors(int offeringId)
        {
            return _portfolioRepository.GetInvestors(offeringId);
        }       
    }
}
 