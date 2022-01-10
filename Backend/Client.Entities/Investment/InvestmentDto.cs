using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class InvestmentDto
    {       
        public int Id { get; set; }       
        public int UserId { get; set; }  
        public int UserProfileId { get; set; }
        public int OfferingId { get; set; }
        public decimal Amount { get; set; }  
        public DateTime? FundedDate { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public bool IsReservation { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IseSignCompleted { get; set; }
        public DateTime? DocumenteSignedDate { get; set; }
        public string eSignedDocumentPath { get; set; }
        public DateTime? WireTransferDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
    }
    public class MyInvestmentDto
    {
        public int Id { get; set; }       
        public int UserId { get; set; }
        public int UserProfileId { get; set; }
        public string ProfileDisplayId { get; set; }
        public int OfferingId { get; set; }        
        public string OfferingName{ get; set; }
        public string OfferingPictureUrl { get; set; }
        public string OfferingEntityName { get; set; }        
        public decimal Amount { get; set; }
        public decimal TotalEarnings { get; set; }
        public DateTime? FundedDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IseSignCompleted { get; set; }
        public DateTime? WireTransferDate { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public bool IsReservation { get; set; }
        public bool IsConverted { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public UserProfileDto UserProfile { get; set; }
        public List<PortfolioKeyHighlightDto> KeyHighlights { get; set; }
        public List<PortfolioGalleryDto> Galleries { get; set; }
        public List<PortfolioDocumentDto> Documents { get; set; }
        public List<PortfolioLocationDto> Locations { get; set; }
        public List<PortfolioSummaryDto> Summary { get; set; }
        public PortfolioFundingInstructionsDto Funds { get; set; }
        public List<PortfolioUpdatesDto> Updates { get; set; }
    }
    public class PortfolioInvestmentDataDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public int ProfileTypeId { get; set; }
        public string InvestorName { get; set; }
        public string ProfileName { get; set; }
        public string ProfileTypeName { get; set; }
        public bool IsAccredited { get; set; }
        public bool IsVerified { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public bool IsNotes { get; set; }
        public string Notes { get; set; }
        public int OfferingId { get; set; }
        public string OfferingName { get; set; }
        public DateTime? FundsReceivedDate { get; set; }
        public DateTime? DocumenteSignedDate { get; set; }
        public string eSignedDocumentPath { get; set; }
        public IFormFile eSignedDocument { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class PortfolioReservationDataDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OfferingId { get; set; }
        public int ProfileId { get; set; }
        public int ProfileTypeId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileTypeName { get; set; }
        public string InvestorName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }       
        public bool IsAccredited { get; set; }        
        public decimal Amount { get; set; }
        public bool IsNotes { get; set; }
        public string Notes { get; set; }
        public bool IsConverted { get; set; }
        public DateTime? ConvertedOn { get; set; }
        public int ConfidenceLevel { get; set; } 
        public DateTime? ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
