using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{  
    public class PortfolioFundingInstructionsDto
    {        
        public int Id { get; set; }        
        public int OfferingId { get; set; }
        public int AdminUserId { get; set; }
        public int InstructionType { get; set; }
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
