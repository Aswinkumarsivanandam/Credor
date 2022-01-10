using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Credor.Data.Entities
{
    public class PortfolioOffering
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; } 
        public string EntityName { get; set; }
        public decimal Size { get; set; }  
        public decimal MinimumInvestment { get; set; }
        public int Type { get; set; }
        public string PublicLandingPageUrl { get; set; }
	    public bool? IsPrivate { get; set; }
	    public bool? ShowPercentageRaised { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }    
        public bool IsReservation { get; set; }
        public int Visibility { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
    }
}
