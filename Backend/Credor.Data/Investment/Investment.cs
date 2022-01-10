using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Credor.Data.Entities
{
    public class Investment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserAccount")]
        public int UserId { get; set; }
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }
        [ForeignKey("PortfolioOffering")]
        public int OfferingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? FundedDate { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public bool IsReservation { get; set; }
        public bool IsConverted { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IseSignCompleted { get; set; }
        public DateTime? DocumenteSignedDate { get; set; }
        public string eSignedDocumentPath { get; set; }
        public DateTime? WireTransferDate { get; set; }
        public int ConfidenceLevel { get; set; }
        public string Notes { get; set; }       
        public DateTime? ConvertedOn { get; set; }      
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
    }
}
