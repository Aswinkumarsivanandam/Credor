using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Data.Entities;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public interface IInvestmentService
    {        
        List<PortfolioOfferingDto> GetOfferingsAndReservations();
        PortfolioOfferingDto GetOfferingById(int offeringId);
        PortfolioOfferingDto GetReservationById(int offeringId);
        PortfolioOfferingDto GetOfferingDetailById(int id);
        int AddInvestment(InvestmentDto investment);
        int AddReservation(InvestmentDto reservation);
        List<OfferingVisibilityDto> GetOfferingVisibilities();
        int UpdateInvestment(InvestmentDto investmentDto);
        int UpdateReservation(InvestmentDto investmentDto);
        InvestmentDto GetInvestmentDetailById(int userId, int investmentId);
        List<InvestmentDto> GetReservationDetailById(int userId);
        PortfolioFundingInstructionsDto GetPortfolioFundingInstructions(int offeringId);
        List<PortfolioOfferingDto> GetOfferingsAndReservations(int userId);
    }
}
