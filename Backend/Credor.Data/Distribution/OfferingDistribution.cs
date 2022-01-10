﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Credor.Data.Entities
{
    public class OfferingDistribution
    {
        [Key]
        public int Id { get; set; }  
        [ForeignKey("PortfolioOffering")]
        public int OfferingId { get; set; }
        [ForeignKey("PortfolioDistributionType")]        
        public int Type { get; set; }
        public int? CalculationMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Memo { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
