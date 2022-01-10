using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class InvestorsSummaryDto
    {
        public int Id { get; set; }
        public int? TotalInvestors { get; set; }
        public int? TotalInvestments { get; set; }
        public decimal? TotalApprovedAmount { get; set; }
        public decimal? AverageInvestment { get; set; }
        public decimal? TotalReserved { get; set; }
    }
}
