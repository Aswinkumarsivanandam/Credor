using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Credor.Data.Entities
{
    public class Distributions
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("OfferingDistribution")]
        public int OfferingDistributionId { get; set; }        
        [ForeignKey("Investment")]
        public int? InvestmentId { get; set; }
        [ForeignKey("UserAccount")]
        public int? InvestorId { get; set; }
        public int Type { get; set; }
        public int DistributionMethod { get; set; }
        public string Memo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal PercentageFunded { get; set; }
        public decimal PercentageOwnership { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }        
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
