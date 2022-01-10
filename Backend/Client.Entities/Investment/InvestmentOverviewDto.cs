using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class InvestmentOverviewDto
    {
        public string OfferingName { get; set; }
        public DateTime InvestedDate { get; set; }
        public decimal InvestmentAmount { get; set; }
        public decimal FundedAmount { get; set; }
        public decimal Distributions { get; set; }
        public decimal EM { get; set; }
        public string Status { get; set; }

    }
}
