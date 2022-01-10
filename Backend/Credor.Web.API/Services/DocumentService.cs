using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Data.Entities;
using Credor.Web.API;
using Credor.Web.API.Common.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class DocumentService :IDocumentService
    {
        private readonly IDocumentRepository _documentRespository;
        private readonly IProfileRespository _profileRespository;
        private readonly UnitOfWork _unitOfWork;
        public DocumentService(IDocumentRepository documentRespository,
                                IProfileRespository profileRespository,
                                 IConfiguration configuration)
        {
            _documentRespository = documentRespository;
            _profileRespository = profileRespository;
            _unitOfWork = new UnitOfWork(configuration);
        }
        public List<DocumentTypesDto> GetDocumentTypes()
        {
            return _documentRespository.GetDocumentTypes();
        }
        public List<DocumentStatusDto> GetDocumentStatuses()
        {
            return _documentRespository.GetDocumentStatuses();
        }
        public List<DocumentDto> GetAllDocuments(int userid)
        {
            return _documentRespository.GetAllDocuments(userid);
        }
        public Task<bool> AddDocuments(DocumentModelDto documents)
        {
            return _documentRespository.AddDocuments(documents);
        }
        public int DeleteDocument(int userid, int documentId)
        {
            return _documentRespository.DeleteDocument(userid, documentId);
        }
        public List<DocumentDto> GetDocumentUpdates(int userId)
        {
            return _documentRespository.GetDocumentUpdates(userId);
        }
        public List<DocumentNameDelimiterDto> GetDocumentNameDelimiters()
        {
            return _documentRespository.GetDocumentNameDelimiters();
        }
        public List<DocumentNamePositionDto> GetDocumentNamePositions()
        {
            return _documentRespository.GetDocumentNamePositions();
        }
        public List<DocumentNameSeparatorDto> GetDocumentNameSeparators()
        {
            return _documentRespository.GetDocumentNameSeparators();
        }
        public List<DocumentBatchModel> bulkdocumentupload(UploadDocumentModelDto batchDetailDto)
        {
            try
            {
                if (batchDetailDto.Id == 0)
                {
                    DocumentBatchDetailDto documentBatchDetailDto = new DocumentBatchDetailDto();
                    documentBatchDetailDto.AdminUserId = batchDetailDto.AdminUserId;
                    documentBatchDetailDto.BatchName = batchDetailDto.BatchName;
                    documentBatchDetailDto.NameDelimiter = batchDetailDto.NameDelimiter;
                    documentBatchDetailDto.NamePosition = batchDetailDto.NamePosition;
                    documentBatchDetailDto.NameSeparator = batchDetailDto.NameSeparator;
                    documentBatchDetailDto.DocumentType = batchDetailDto.DocumentType;
                    batchDetailDto.Id = _documentRespository.CreateBatch(documentBatchDetailDto);
                }
                string nameDelimiter = _unitOfWork.DocumentNameDelimiterRepository.GetByID(batchDetailDto.NameDelimiter).Value;
                string nameSeparator = _unitOfWork.DocumentNameSeparatorRepository.GetByID(batchDetailDto.NameSeparator).Value;
                List<DocumentBatchModel> mappedDocuments = new List<DocumentBatchModel>();
                
                if (batchDetailDto.Files != null && batchDetailDto.Status == 1)
                {
                    foreach (var document in batchDetailDto.Files)
                    {
                        string documentName = document.FileName;
                        string fullName = "";
                        string firstName = "";
                        string lastName = "";
                        string EmailId = "";                        
                        UserAccount userAccount = null;
                        if (batchDetailDto.NamePosition == 1)//Start of the file name
                        {
                            var remainingText = documentName.Substring(0, documentName.IndexOf(nameDelimiter));                           
                            int startindex = remainingText.IndexOf('(');
                            int Endindex = remainingText.IndexOf(')');

                            EmailId = remainingText.Substring(startindex + 1, Endindex - startindex - 1);                            
                            fullName = remainingText.Substring(EmailId.Length + 1, remainingText.Length - EmailId.Length - 1).Remove(0,1);                            
                        }
                        else if (batchDetailDto.NamePosition == 2)//End of the file name
                        {
                            int namelength = documentName.Length;
                            string remainingname = documentName.Substring(0, documentName.IndexOf(nameDelimiter));                                                       

                            remainingname = documentName.Substring(remainingname.Length + 1, namelength - remainingname.Length - Path.GetExtension(documentName).Length - 1);
                           
                            int startindex = remainingname.IndexOf('(');
                            int Endindex = remainingname.IndexOf(')');

                            EmailId = remainingname.Substring(startindex + 1, Endindex - startindex - 1);
                            
                            fullName = remainingname.Substring(EmailId.Length + 1, remainingname.Length - EmailId.Length - 1).Remove(0, 1); ;
                        }
                        if (!String.IsNullOrEmpty(EmailId))
                        {
                            DocumentBatchModel documentDetail = new DocumentBatchModel();
                            string matchedBy = "Investor Not Matched";
                            userAccount = _unitOfWork.UserAccountRepository.Get(x => x.EmailId == EmailId && x.Active == true);
                            if (userAccount != null)
                            {
                                if (!String.IsNullOrEmpty(fullName))
                                {
                                    if (fullName.Contains(nameSeparator))
                                    {
                                        firstName = fullName.Substring(0, fullName.IndexOf(nameSeparator));
                                        lastName = fullName.Substring(firstName.Length + 1, fullName.Length - firstName.Length - 1);
                                    }
                                    else
                                        firstName = fullName;
                                    UserProfile matchedProfile = new UserProfile();                                    
                                    if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                                    {
                                        matchedProfile = _unitOfWork.UserProfileRepository.Get(x => x.UserId == userAccount.Id && 
                                                                                (x.FirstName != null && x.LastName != null) &&
                                                                                (x.FirstName.ToLower() == firstName.ToLower())
                                                                                && x.LastName.ToLower() == lastName.ToLower()                                                                                
                                                                                );
                                        matchedBy = matchedProfile != null ? "Matchd By Full Name" : matchedBy;
                                    }
                                    else if (!string.IsNullOrEmpty(firstName))
                                    {
                                        matchedProfile = _unitOfWork.UserProfileRepository.Get(x => (x.UserId == userAccount.Id 
                                                                                                && x.FirstName != null) 
                                                                                                && (x.FirstName.ToLower() == firstName.ToLower())                                                                             
                                                                                );
                                        matchedBy = matchedProfile != null ? "Matchd By First Name" : matchedBy;
                                    }
                                    if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                                    {
                                        matchedProfile = _unitOfWork.UserProfileRepository.Get(x =>
                                                                    (x.UserId == userAccount.Id) && (x.Name != null) && (x.Name.Contains(firstName)
                                                                    || x.Name.Contains(lastName)
                                                                    || x.Name == fullName.Replace("_"," ")));
                                        matchedProfile = _unitOfWork.UserProfileRepository.Get(x =>
                                                                    (x.UserId == userAccount.Id) && (x.RetirementPlanName != null) && (x.RetirementPlanName.Contains(firstName)
                                                                    || x.RetirementPlanName.Contains(lastName)
                                                                    || x.RetirementPlanName == fullName));
                                        matchedProfile = _unitOfWork.UserProfileRepository.Get(x =>
                                                                (x.UserId == userAccount.Id) && (x.TrustName != null) && (x.TrustName.Contains(firstName)
                                                                || x.TrustName.Contains(lastName)
                                                                || x.TrustName == fullName));
                                        matchedBy = matchedProfile != null ? "Matchd By Search" : matchedBy;
                                    }

                                    if (matchedProfile != null)
                                    {
                                        documentDetail.BatchId = batchDetailDto.Id;
                                        documentDetail.UserId = userAccount.Id; // 0 If no account found
                                        documentDetail.IsMatchfound = true;
                                        documentDetail.MatchedBy = matchedBy;
                                        documentDetail.DocumentName = document.FileName;
                                        documentDetail.InvestorName = userAccount.FirstName + " " + userAccount.LastName;
                                        documentDetail.ProfileId = matchedProfile.Id;
                                        documentDetail.ProfileName = matchedProfile.Name + (matchedProfile.FirstName +" " +
                                            matchedProfile.LastName) + matchedProfile.RetirementPlanName + matchedProfile.TrustName;
                                        documentDetail.UserProfiles = _profileRespository.GetAllUserProfile(userAccount.Id);
                                        mappedDocuments.Add(documentDetail);
                                    }
                                    else
                                    {
                                        var ownerProfile = _unitOfWork.UserProfileRepository.Get(x => x.UserId == userAccount.Id && x.IsOwner == true);
                                        documentDetail.BatchId = batchDetailDto.Id;
                                        documentDetail.UserId = userAccount.Id; // 0 If no account found
                                        documentDetail.IsMatchfound = true;
                                        documentDetail.MatchedBy = "Profile Not Matched";
                                        documentDetail.DocumentName = document.FileName;
                                        documentDetail.InvestorName = userAccount.FirstName + " " + userAccount.LastName;
                                        documentDetail.ProfileId = ownerProfile.Id;
                                        documentDetail.ProfileName = ownerProfile.Name + (ownerProfile.FirstName + " " +
                                                    ownerProfile.LastName) + ownerProfile.RetirementPlanName + ownerProfile.TrustName;
                                        documentDetail.UserProfiles = _profileRespository.GetAllUserProfile(userAccount.Id);
                                        mappedDocuments.Add(documentDetail);
                                    }
                                }
                            }
                            else
                            {
                                documentDetail.BatchId = batchDetailDto.Id;
                                documentDetail.UserId = 0; //  If no account found
                                documentDetail.IsMatchfound = false;
                                documentDetail.MatchedBy = matchedBy;
                                documentDetail.DocumentName = document.FileName;
                                documentDetail.InvestorName = null;
                                documentDetail.ProfileId = null;
                                documentDetail.ProfileName = null;
                                documentDetail.UserProfiles = null;
                                mappedDocuments.Add(documentDetail);
                            }
                        }                        
                    }
                    var result = _documentRespository.UpdateBatch(batchDetailDto.AdminUserId, batchDetailDto.Id, 2);
                    if (result)
                        return mappedDocuments;
                }                                                        
                return null; // failure
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }
        public bool UpdateBatch(int adminuserid, int batchid, int status, int totalDocuments = 0)
        {
            return _documentRespository.UpdateBatch(adminuserid, batchid, status, totalDocuments);
        }

        public bool UpdateBatch(UpdateBatchDto batch)
        {
            return _documentRespository.UpdateBatch(batch.AdminUserId, batch.BatchId, batch.Status, batch.TotalDocuments);
        }
        public async Task<int> PublishMatchedDocument(PublishDocumentDto document)
        {
            if (document!= null)
            {
                try
                {                                                                           
                    DocumentBatchModel documentDetail = new DocumentBatchModel();
                    documentDetail.AdminUserId = document.AdminUserId;
                    documentDetail.UserId = document.UserId;
                    documentDetail.BatchId = document.BatchId;
                    documentDetail.ProfileId = document.UserProfileId;                        
                    documentDetail.Document = document.File;
                                                    
                    documentDetail.Type = document.DocumentType;
                    List<DocumentBatchModel> finalDocuments = new List<DocumentBatchModel>();
                    finalDocuments.Add(documentDetail);

                    var result = await _documentRespository.AddDocumentsInBatch(finalDocuments);
                    if (result)
                    {
                       // _documentRespository.UpdateBatch(adminUserId, batchId, 3, finalDocuments.Count);
                        return document.BatchId;
                    }
                    else
                        return 0;
                }
                catch (Exception e)
                {
                    e.ToString();
                    return 0;
                }
            }
            else
                return 0;
        }
        public async Task<DocumentBatchDetailDto> PublishMatchedDocuments(PublishDocumentModelDto documents)
        {
            if (documents.Files != null && documents.MatchedDocs != null && documents.Files.Count > 0 && documents.MatchedDocs.Count>0)
            {
                try
                {                                      
                    List<DocumentBatchModel> finalDocuments = new List<DocumentBatchModel>();
                    foreach (var document in documents.Files)
                    {                                                                                     
                        DocumentBatchModel documentDetail = new DocumentBatchModel();
                        documentDetail.AdminUserId = documents.AdminUserId;
                        documentDetail.BatchId = documents.BatchId;

                        foreach (var matchedDocument in documents.MatchedDocs)
                        {
                            if (matchedDocument.FileName == document.FileName)
                            {
                                documentDetail.UserId = Convert.ToInt32(matchedDocument.UserId);
                                documentDetail.ProfileId = Convert.ToInt32(matchedDocument.ProfileId);
                                documentDetail.Document = document;
                                documentDetail.Type = documents.DocumentType;
                                finalDocuments.Add(documentDetail);
                            }                            
                        }
                    }
                    var result = await _documentRespository.AddDocumentsInBatch(finalDocuments);
                    if (result)
                    {
                        _documentRespository.UpdateBatch(documents.AdminUserId, documents.BatchId, 3, finalDocuments.Count);
                        var uploadedDocs = _documentRespository.GetDocumentBatchDetail(documents.BatchId);
                        return uploadedDocs;
                    }
                    else
                        return null;
                }
                catch(Exception e)
                {
                    e.ToString();
                    return null;
                }
            }
            else
                return null;
        }
        public bool DeleteDocumentBatch(int adminuserid, int batchid)
        {
            return _documentRespository.DeleteDocumentBatch(adminuserid, batchid);
        }
        public bool DeleteDocumentInBatchByUserId(int adminuserid, int batchid, int userid)
        {
            return _documentRespository.DeleteDocumentInBatchByUserId(adminuserid,batchid,userid);
        }
        public bool DeleteDocumentInBatchById(int adminuserid, int batchid, int documentId)
        {
            return _documentRespository.DeleteDocumentInBatchById(adminuserid, batchid, documentId);
        }
        public DocumentBatchDetailDto GetDocumentBatchDetail(int batchid)
        {
            return _documentRespository.GetDocumentBatchDetail(batchid);
        }
        public List<DocumentBatchDetailDto> GetAllDocumentBatches()
        {
            return _documentRespository.GetAllDocumentBatches();
        }
        public List<SubscriptionDocumentDto> GetAllSubscriptionDocuments()
        {
            return _documentRespository.GetAllSubscriptionDocuments();
        }
        public List<SubscriptionDocumentDto> GetAllSubscriptionDocuments(int offeringId)
        {
            return _documentRespository.GetAllSubscriptionDocuments(offeringId);
        }
    }    
}
