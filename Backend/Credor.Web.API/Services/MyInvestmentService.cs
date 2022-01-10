using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public class MyInvestmentService : IMyInvestmentService
    {

        private readonly IMyInvestmentRepository _myInvestmentRepository;
        public MyInvestmentService(IMyInvestmentRepository myInvestmentRepository)
        {
            _myInvestmentRepository = myInvestmentRepository;
        }
        public List<MyInvestmentDto> GetInvestmentAndReservationsByUserId(int userId)
        {
            return _myInvestmentRepository.GetInvestmentAndReservationsByUserId(userId);
        }
        public List<MyInvestmentDto> GetInvestmentListByUserId(int userId)
        {
            return _myInvestmentRepository.GetInvestmentListByUserId(userId);
        }
        public List<MyInvestmentDto> GetReservationListByUserId(int userId)
        {
            return _myInvestmentRepository.GetReservationListByUserId(userId);
        }
        public MyInvestmentDto GetInvestmentDetailById(int userId, int investmentId)
        {
            return _myInvestmentRepository.GetInvestmentDetailById(userId, investmentId);
        }
        public MyInvestmentDto GetReservationDetailById(int userId, int reservationId)
        {
            return _myInvestmentRepository.GetReservationDetailById(userId, reservationId);
        }
        public List<InvestmentStatusDto> GetInvestmentStatuses()
        {
            return _myInvestmentRepository.GetInvestmentStatuses();
        }
        public InvestmentSummaryDto GetHeaderElements(int userId)
        {
            return _myInvestmentRepository.GetHeaderElements(userId);
        }
    }
}
