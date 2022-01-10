using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class UserProfileDto
    {       
        public int Id { get; set; }        
        public int AdminUserId { get; set; }
        public int UserId { get; set; } 
        public string ProfileDisplayId { get; set; }
        public string ProfileName { get; set; }
        public int Type { get; set; }        
        public int? BankAccountId { get; set; }
        public bool IsOwner { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TrustName { get; set; }
        public string RetirementPlanName { get; set; }
        public string SignorName { get; set; }
        public string InCareOf { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }       
        public int StateOrProvinceId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; } 
        public string TaxId { get; set; }       
        public int? DistributionTypeId { get; set; }
        public bool? IsDisregardedEntity { get; set; }
        public bool? IsIRALLC { get; set; }
        public string OwnerTaxId { get; set; }
        public string DistributionDetail { get; set; }
        public string CheckInCareOf { get; set; }
        public string CheckAddressLine1 { get; set; }
        public string CheckAddressLine2 { get; set; }
        public string CheckCity { get; set; }
        public int CheckStateId { get; set; }
        public string CheckZip { get; set; }
        public string PaymentMethod { get; set; }
        public string BankName { get; set; }
        public string BankAccountType { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankRoutingNumber { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public List<InvestorDto> Investors { get; set; }
        public List<InvestmentDto> Investments { get; set; }        
    }
}
