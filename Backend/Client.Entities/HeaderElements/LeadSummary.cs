using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class LeadSummary
    {      
        public int TotalLeads { get; set; }
        public decimal AccreditedLeads { get; set; }
        public decimal VerifiedLeads  { get; set; }
        public decimal UnverifiedLeads  { get; set; }       
    }
}
