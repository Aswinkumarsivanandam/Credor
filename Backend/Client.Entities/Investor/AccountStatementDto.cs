using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class AccountStatementDto
    {
        public int InvestorId { get; set; }
        public string InvestorName { get; set; }
        public ContactDto CredorContact { get; set; }
        public List<DistributionsDto> Distributions { get; set; }
        public decimal TotalInvested { get; set; }
        public DateTime StatementDate { get; set; }
        public List<InvestmentOverviewDto> InvestmentOverviews { get; set; }
        public List<DistributionsSummaryDto> DistributionsSummaries { get; set; }
    }
}
