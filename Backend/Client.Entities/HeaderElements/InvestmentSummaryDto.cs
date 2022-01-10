using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class InvestmentSummaryDto
    {
        public int UserId { get; set; }
        public int? ActiveInvestments { get; set; }
        public decimal? TotalInvested { get; set; }
        public decimal? TotalEarnings { get; set; }
        public decimal? TotalReturn { get; set; }
        public int? PendingInvestments { get; set; }
    }
}
