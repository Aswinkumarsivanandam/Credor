using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class PortfolioLocationDto
    {        
        public int Id { get; set; } 
        public int AdminUserId { get; set; }
        public int OfferingId { get; set; }
        public string Location { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
    }
}
