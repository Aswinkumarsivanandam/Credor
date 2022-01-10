using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class DocumentBatchDetailDto
    {
        public int Id { get; set; }
        public int AdminUserId { get; set; }
        public string BatchName { get; set; }
        public int TotalDocuments { get; set; }
        public int DocumentType { get; set; }
        public int NameDelimiter { get; set; }
        public int NamePosition { get; set; }
        public int NameSeparator { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public List<DocumentBatchModel> Documents { get; set; }
        public List<DocumentBatchModel> MatchedDocuments { get; set; }
        public List<DocumentBatchModel> MappedDocuments { get; set; }
    }
    public class DocumentBatchModel
    {
        public int DocumentId { get; set; }
        public int BatchId { get; set; }
        public int AdminUserId { get; set; }
        public int UserId { get; set; }
        public int Type { get; set; }
        public IFormFile Document { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string InvestorName { get; set; }
        public bool IsMatchfound { get; set; }
        public string MatchedBy { get; set; }
        public int? ProfileId { get; set; }
        public string ProfileName { get; set; }
        public List<UserProfileDto> UserProfiles { get; set; }

    }
    public class UploadDocumentModelDto
    {
        public int Id { get; set; }
        public int AdminUserId { get; set; }
        public int UserId { get; set; }
        public int UserProfileId { get; set; }
        public string BatchName { get; set; }
        public int Status { get; set; }
        public int TotalDocuments { get; set; }
        public int DocumentType { get; set; }
        public int NameDelimiter { get; set; }
        public int NamePosition { get; set; }
        public int NameSeparator { get; set; }
        public IFormFileCollection Files { get; set; }
    }
    public class PublishDocumentDto
    {
        public int BatchId { get; set; }
        public int AdminUserId { get; set; }
        public int UserId { get; set; }
        public int UserProfileId { get; set; }
        public int DocumentType { get; set; }
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }
    public class PublishDocumentModelDto
    {         
        public int AdminUserId { get; set; }
        public int BatchId { get; set; }
        public int DocumentType { get; set; }
        //public List<MatchedDocuments> MatchedDocuments { get; set; }
        public IFormCollection MatchedDocuments { get; set; }
        public List<MatchedDocument> MatchedDocs { get; set; }
        public IFormFileCollection Files { get; set; }
    }
    public class MatchedDocument
    {
        public string UserId { get; set; }
        public string ProfileId { get; set; }
        public string FileName { get; set; }
    }

    public class UpdateBatchDto
    {
        public int AdminUserId { get; set; }
        public int BatchId { get; set; }
        public int TotalDocuments { get; set; }
        public int Status { get; set; }
    }
}
