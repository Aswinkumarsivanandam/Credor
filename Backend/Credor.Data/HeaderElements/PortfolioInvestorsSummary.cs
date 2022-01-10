using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Data.Entities
{
    public class PortfolioInvestorsSummary
    {
        public int Id { get; set; }   
        public int OfferingId { get; set; }
        public decimal OfferingSize { get; set; }
        public decimal Committed { get; set; }
        public decimal Remaining { get; set; }
        public decimal TotalApproved { get; set; }
        public decimal TotalPending { get; set; }
        public int Approved { get; set; }
        public int Pending { get; set;  }
        public int Waitlist { get; set; }
        public decimal AverageApproved { get; set; }
        public int NonAccredited { get; set; }

    }
}
