using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
   public class PortfolioSummaryDto
    {       
        public int Id { get; set; } 
        public int AdminUserId { get; set; }
        public int OfferingId { get; set; }
        public string Summary { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
    }
}
