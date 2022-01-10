using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Credor.Data.Entities
{
    public class PortfolioFund
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("PortfolioOffering")]        
        public int OfferingId { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string Beneficiary { get; set; }
        public string BeneficiaryAddress { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public int AccountTypeId { get; set; }
        public string Reference { get; set; }
        public string OtherInstructions { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
    }
    public class PortfolioFundingInstructions
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("PortfolioOffering")]
        public int OfferingId { get; set; }       
        public string ReceivingBank { get; set; }
        public string BankAddress { get; set; }
        public string Beneficiary { get; set; }
        public string CheckBenificiary { get; set; }
        public string BeneficiaryAddress { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public int AccountType { get; set; }
        public string Reference { get; set; }
        public string OtherInstructions { get; set; }
        public string CheckOtherInstructions { get; set; }
        public string MailingAddress { get; set; }
        public string Memo { get; set; }
        public string Custom { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
