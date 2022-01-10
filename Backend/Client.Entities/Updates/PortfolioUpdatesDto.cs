using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class PortfolioUpdatesDto
    {
        public int Id { get; set; }
        public int OfferingId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set;}
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }        
    }
}
