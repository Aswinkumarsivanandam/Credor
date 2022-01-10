using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public interface IMyInvestmentService
    {
        List<MyInvestmentDto> GetInvestmentAndReservationsByUserId(int userId);
        List<MyInvestmentDto> GetInvestmentListByUserId(int userId);
        List<MyInvestmentDto> GetReservationListByUserId(int userId);
        MyInvestmentDto GetInvestmentDetailById(int userId, int investmentId);
        MyInvestmentDto GetReservationDetailById(int userId, int reservationId);
        List<InvestmentStatusDto> GetInvestmentStatuses();
        InvestmentSummaryDto GetHeaderElements(int userId);
    }
}
