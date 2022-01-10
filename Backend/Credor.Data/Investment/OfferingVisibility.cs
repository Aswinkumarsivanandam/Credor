using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Credor.Data.Entities
{
    public class OfferingVisibility
    {
        [Key]
        public int Id { get; set; }
        public string AccessTo { get; set; }
        public bool Active { get; set; }
    }
    public class PortfolioOfferingVisibility
    {
        public int Id { get; set; }       
        [ForeignKey("PortfolioOffering")]
        public int OfferingId { get; set; }
        public int OfferingVisibilityId { get; set; }        
        [ForeignKey("PortfolioOffering")]
        public int OfferingGroupId { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
