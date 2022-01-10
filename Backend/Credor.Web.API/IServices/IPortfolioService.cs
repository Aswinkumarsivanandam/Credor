using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public interface IPortfolioService
    {
        List<PortfolioOfferingDto> GetPortfolioOfferings();
        List<PortfolioOfferingDto> GetPortfolioReservations();
        PortfolioOfferingDto GetPortfolioOffering(int offeringId);
        PortfolioOfferingDto GetPortfolioReservation(int reservationId);
        int AddPortfolioOffering(PortfolioOfferingDto portfolioOffering); 
        int AddPortfolioReservation(PortfolioOfferingDto portfolioReservation);
        int UpdatePortfolioOffering(PortfolioOfferingDto portfolioOffering);
        int UpdatePortfolioReservation(PortfolioOfferingDto portfolioReservation);
        bool DeletePortfolioOffering(int adminUserId, int offeringId);
        bool DeletePortfolioReservation(int adminUserId , int reservationId);
        List<PortfolioOfferingDto> GetArchivedPortfolioOfferings();
        List<PortfolioOfferingDto> GetArchivedPortfolioOfferingAndReservations();
        bool RestorePortfolioOffering(int AdminUserId, int offeringId);
        bool RestorePortfolioReservation(int AdminUserId, int reservationId);
        Task<AddPortfolioGalleryResultDto> AddPortfolioGalleryImages(AddPortfolioGalleryDto portfolioGalleryDto);
        UpdatePortfolioGalleryResultDto UpdatePortfolioGalleryImage(PortfolioGalleryDto portfolioGalleryDto);
        PortfolioSummaryDto AddPortfolioSummary(PortfolioSummaryDto portfolioSummaryDto);
        PortfolioSummaryDto UpdatePortfolioSummary(PortfolioSummaryDto portfolioSummaryDto);
        Task<PortfolioDocumentsResultDto> UploadOfferingDocuments(DocumentModelDto documentDto);
        PortfolioDocumentsResultDto DeleteOfferingDocument(int adminuserid,int offeringId, int documentId);
        bool AddDefaultPortfolioKeyHighlights(int id,int adminuserid);
        UpdatePortfolioKeyHightlightDto UpdatePortfolioKeyHightlight(UpdatePortfolioKeyHightlightDto updatePortfolioKeyHightlight);
        PortfolioLocationDto AddPortfolioLocation(PortfolioLocationDto portfolioLocation);
        PortfolioLocationDto UpdatePortfolioLocation(PortfolioLocationDto portfolioLocation);
        PortfolioFundingInstructionsDto AddPortfolioFundingInstructions(PortfolioFundingInstructionsDto portfolioFundingInstructions);
        PortfolioFundingInstructionsDto UpdatePortfolioFundingInstructions(PortfolioFundingInstructionsDto portfolioFundingInstructions);
        PortfolioInvestmentsSummaryDto GetPortfolioInvestorsSummary(int offeringId);        
        List<PortfolioInvestmentDataDto> GetPortfolioOfferingInvestments(int offeringId);
        PortfolioReservationsSummaryDto GetPortfolioReservationsSummary(int offeringId);
        ReservationsSummaryDto GetReservationsSummary(int reservationId);
        List<PortfolioReservationDataDto> GetPortfolioOfferingReservations(int offeringId);
        List<PortfolioReservationDataDto> GetReservationsList(int reservationId);
        bool AddInvestmentNotes(InvestmentNotesDto investmentNotesDto);
        bool AddReservationNotes(ReservationNotesDto reservationNotes);
        List<InvestmentStatusDto> GetInvestmentStatuses();
        CapTableDataDto GetPortfolioOfferingCapTable(int offeringid);
        bool AddOfferingDistributions(OfferingDistributionDto distributionsData);
        OfferingDistributionDto ConfirmDistributions(int offeringDistributionId, int adminuserid);
        List<PortfolioDistributionTypeDto> GetPortfolioDistributionTypes();
        List<OfferingDistributionDto> GetOfferingDistributions(int offeringid);
        OfferingDistributionDto UpdateOfferingDistribution(OfferingDistributionDto offeringDistributionDto);
        bool DeleteOfferingDistribution(int offeringDistributionId, int adminUserId);
        bool UpdateCapTable(UpdateCapTableDto capTableDto);
        OfferingDistributionDto GetOfferingDistributionDetail(int offeringDistributionId);
        List<CapTableInvestorDto> GetInvestors(int offeringId);        
    }
}
