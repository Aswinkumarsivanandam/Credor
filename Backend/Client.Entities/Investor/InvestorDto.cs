using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{   
    public class ReservationDataDto
    {
        public int Id { get; set; }
        public int AdminUserId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public int ProfileType { get; set; }
        public string ProfileTypeName { get; set; }
        public int ReservationId { get; set; }
        public string ReservationName { get; set; }
        public int ConfidenceLevel { get; set; }
        public UserProfileDto UserProfile { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class InvestmentDataDto
    {
        public int Id { get; set; }
        public int AdminUserId { get; set; }
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileTypeName { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public UserProfileDto UserProfile { get; set; }
        public int OfferingId { get; set; }
        public string OfferingName { get; set; }
        public DateTime? FundsReceivedDate { get; set; }
        public DateTime? DocumenteSignedDate{get;set;}
        public string eSignedDocumentPath { get; set; }
        public IFormFileCollection eSignedDocument { get; set; }
        public DateTime CreatedOn { get; set; }       
    }    

    public class InvestmentNotesDto
    {
        public int AdminUserId { get; set; }
        public int InvestmentId { get; set; }
        public string Notes { get; set; }
    }
    public class ReservationNotesDto
    {
        public int AdminUserId { get; set; }
        public int ReservationId { get; set; }
        public string Notes { get; set; }
    }
}
