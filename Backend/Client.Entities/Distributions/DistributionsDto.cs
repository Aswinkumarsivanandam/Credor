using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class DistributionsDto
    {
        public int Id { get; set; }        
        public int UserId { get; set; }
        public string Type { get; set; }
        public string Memo { get; set; }
        public string DistributionMethod { get; set; }
        public int UserProfileId { get; set; } 
        public string UserProfileType { get; set; }
        public int OfferingId { get; set; }
        public int PaymentId { get; set; }
        public string OfferingName { get; set; }
        public UserProfileDto UserProfile { get; set; }
        public Decimal AmountInvested { get; set; }
        public Decimal AmountReceived { get; set; }
        public DateTime ReceivedDate { get; set; }                    
    }

    public class OfferingDistributionDto
    {
        public int Id { get; set; }
        public int AdminUserId { get; set; }
        public int OfferingId { get; set; }
        public int InvesterId { get; set; }
        public int Type { get; set; }
        public int? CalculationMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Memo { get; set; }
        public List<DistributionsDataDto> Distributions { get; set; }
        public bool IsNotify { get; set; }
        public decimal TotalDistributions { get; set; }
    }

    public class DistributionsDataDto
    {
        public int Id { get; set; }    
        public int OfferingDistributionId { get; set; }
        public int? InvestmentId { get; set; }
        public int? UserId { get; set; }
        public int OfferingId { get; set; }
        public int ProfileId { get; set; }
        public string InvestorName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ProfileType { get; set; }
        public string ProfileName { get; set; }
        public int PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PaymentDate { get; set; }          
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string Memo { get; set; }
        public decimal PercentageFunded { get; set; }
        public decimal PercentageOwnership { get; set; }
        public int Status { get; set; }
    }

    public class CapTableDataDto
    {
        public int OfferingId { get; set; }
        public decimal TotalFundedAmount { get; set; }
        public decimal TotalPercentagaeFunded { get; set; }
        public decimal TotalPercentagaeOwnership { get; set; }
        public List<CapTableInvestmentDto> CapTableInvestments { get; set; }
    }
    public class CapTableInvestmentDto
    {        
        public int Id { get; set; }
        public int InvestmentId { get; set; }
        public int OfferingId { get; set; }
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public string InvesterName { get; set; }
        public string ProfileName { get; set; }
        public string ProfileTypeName { get; set; }
        public int ProfileType { get; set; }
        public DateTime FundedDate { get; set; }
        public decimal FundedAmount { get; set; }
        public decimal FundedPercentage { get; set; }
        public decimal OwnershipPercentage { get; set; }   
        public int? PaymentMethod { get; set; }
    }
    public class UpdateCapTableDto
    {
        public int Id { get; set; }
        public int OfferingId { get; set; }
        public decimal FundedPercentage { get; set; }
        public int AdminUserId { get; set; }

    }
}
