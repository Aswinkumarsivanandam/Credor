using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Web.API.Common.UnitOfWork;
using Credor.Client.Entities;
using Credor.Data.Entities;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class InvestmentService : IInvestmentService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IInvestmentRepository _investmentRepository;
        public InvestmentService(IInvestmentRepository investmentRepository,
                                 IConfiguration configuration)
        {
            _investmentRepository = investmentRepository;
            _unitOfWork = new UnitOfWork(configuration);
        }
        public List<PortfolioOfferingDto> GetOfferingsAndReservations()
        {           
            return _investmentRepository.GetOfferingsAndReservations();
        }
        public PortfolioOfferingDto GetOfferingById(int offeringId)
        {
            return _investmentRepository.GetOfferingById(offeringId);
        }
        public PortfolioOfferingDto GetReservationById(int offeringId)
        {
            return _investmentRepository.GetReservationById(offeringId);
        }
        public PortfolioOfferingDto GetOfferingDetailById(int offeringId)
        {
            return _investmentRepository.GetOfferingDetailById(offeringId);
        }
        public int AddInvestment(InvestmentDto investment)
        {
            return _investmentRepository.AddInvestment(investment);                
        }
        public int AddReservation(InvestmentDto reservation)
        {
            return _investmentRepository.AddReservation(reservation);
        }
        public List<OfferingVisibilityDto> GetOfferingVisibilities()
        {
            return _investmentRepository.GetOfferingVisibilities();
        }
        public int UpdateInvestment(InvestmentDto investmentDto)
        {
            return _investmentRepository.UpdateInvestment(investmentDto);
        }
        public int UpdateReservation(InvestmentDto investmentDto)
        {
            return _investmentRepository.UpdateReservation(investmentDto);
        }
        public InvestmentDto GetInvestmentDetailById(int userId, int investmentId)
        {
            return _investmentRepository.GetInvestmentDetailById(userId, investmentId);
        }
        public List<InvestmentDto> GetReservationDetailById(int userId)
        {
            return _investmentRepository.GetReservationDetailById(userId);
        }
        public PortfolioFundingInstructionsDto GetPortfolioFundingInstructions(int offeringId)
        {
            return _investmentRepository.GetPortfolioFundingInstructions(offeringId);
        }
        public List<PortfolioOfferingDto> GetOfferingsAndReservations(int userId)
        {
            return _investmentRepository.GetOfferingsAndReservations(userId);
        }
    }
}
