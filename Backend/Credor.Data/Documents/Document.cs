using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credor.Data.Entities
{
    public class Document
    {
        [Key]
        public int Id { get; set; }        
        [ForeignKey("UserAccount")]
        public int? UserId { get; set; }       
        public int? ProfileId { get; set; }
        [ForeignKey("PortfolioOffering")]
        public int? OfferingId { get; set; }
        [ForeignKey("DocumentBatchDetail")]
        public int? BatchId { get; set; }
        public int? InvestmentId { get; set; }
        [ForeignKey("DocumentTypes")]
        public int Type { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Extension { get; set; }
        public bool? IsPrivate { get; set; }
        public string Size { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
    }
}
