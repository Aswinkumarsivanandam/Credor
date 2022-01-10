using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class InvestorSummaryDto
    {       
        public int UserId { get; set; }     
        public decimal? TotalInvested { get; set; }
        public int? TotalInvestments { get; set; }
        public int? PendingInvestments { get; set; }
        public decimal? TotalReserved { get; set; }        
    }
}
