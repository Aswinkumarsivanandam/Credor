using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Credor.Client.Entities
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? OfferingId { get; set; }
        public int? BatchId { get; set; }
        public string OfferingName { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string FilePath { get; set; }
        public string Size { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }
        public bool? IsPrivate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
    }

    public class SubscriptionDocumentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }       
        public string InvestorName { get; set; }
        public int ProfileType { get; set; }
        public string ProfileTypeName { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string FilePath { get; set; }
        public string Size { get; set; }
        public int Status { get; set; }
        public string SignStatus { get; set; }
        public string SignType { get; set; }
        public string CompletedSign { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
    }
    public class DocumentModelDto
    {
        public int AdminUserId { get; set; }
        public int UserId { get; set; }     
        public int OfferingId { get; set; }
        public int Type { get; set; }
        public bool? IsPrivate { get; set; }
        public IFormFileCollection Files { get; set; }
        public IFormFileCollection WelcomeDocuments { get; set; }
        public IFormFileCollection OfferingDocuments { get; set; }
    }
    public class PortfolioDocumentsResultDto
    {
        public int OfferingId { get; set; }
        public bool Status { get; set; }
        public IList<DocumentDto> OfferingDocuments { get; set; }
    }
}
