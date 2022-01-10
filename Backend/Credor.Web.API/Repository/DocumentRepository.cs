using Credor.Web.API.Common.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Credor.Data.Entities;
using AutoMapper;
using Credor.Web.API.Shared;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using Credor.Web.API.Common;

namespace Credor.Web.API
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly UnitOfWork _unitOfWork;
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        public IConfiguration _configuration { get; }
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public DocumentRepository(IMapper mapper,
                                  IConfiguration configuration,
                                  INotificationRepository notificationRepository,
                                  IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = new UnitOfWork(configuration);
            _mapper = mapper;
            _configuration = configuration;
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }
        public List<DocumentTypesDto> GetDocumentTypes()
        {
            List<DocumentTypesDto> documentTypes = new List<DocumentTypesDto>();
            var contextData = _unitOfWork.DocumentTypesRepository.Context;
            try
            {
                documentTypes = (from type in contextData.DocumentTypes
                                 where type.Visibility == true
                                 select new DocumentTypesDto
                                 {
                                     Id = type.Id,
                                     Name = type.Name,
                                     Active = type.Active
                                 }).ToList();
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                documentTypes = null;
            }
            finally
            {
                contextData = null;
            }
            return documentTypes;
        }
        public List<DocumentStatusDto> GetDocumentStatuses()
        {
            List<DocumentStatusDto> documentStatuses = new List<DocumentStatusDto>();
            var contextData = _unitOfWork.DocumentStatusRepository.Context;
            try
            {
                documentStatuses = (from type in contextData.DocumentStatus
                                    select new DocumentStatusDto
                                    {
                                        Id = type.Id,
                                        Name = type.Name,
                                        Active = type.Active,
                                    }).ToList();
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                documentStatuses = null;
            }
            finally
            {
                contextData = null;
            }
            return documentStatuses;

        }
        public List<SubscriptionDocumentDto> GetAllSubscriptionDocuments()
        {
            List<SubscriptionDocumentDto> subscriptionDocuments = new List<SubscriptionDocumentDto>();
            var contextData = _unitOfWork.DocumentRepository.Context;
            try
            {
                subscriptionDocuments = (from document in contextData.Document
                                         join user in contextData.UserAccount on document.UserId equals user.Id
                                         join profile in contextData.UserProfile on document.ProfileId equals profile.Id
                                         join profileType in contextData.UserProfileType on profile.Type equals profileType.Id
                                         where document.Type == 2 && document.Active == true
                                         select new SubscriptionDocumentDto
                                         {
                                             Id = document.Id,
                                             UserId = user.Id,
                                             InvestorName = user.FirstName + " " + user.LastName,
                                             SignStatus = "Completed",
                                             SignType = "eSign",
                                             CompletedSign = " ",
                                             ProfileType = profile.Type,
                                             ProfileTypeName = profileType.Name,
                                             FileName = document.Name,
                                             Extension = document.Extension,
                                             FilePath = document.FilePath,
                                             Status = document.Status,
                                             Size = document.Size,
                                             Active = document.Active,
                                             CreatedOn = document.CreatedOn,
                                             CreatedBy = document.CreatedBy
                                         }).ToList();
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                subscriptionDocuments = null;
            }
            finally
            {
                contextData = null;
            }
            return subscriptionDocuments;
        }
        public List<SubscriptionDocumentDto> GetAllSubscriptionDocuments(int offeringId)
        {
            List<SubscriptionDocumentDto> subscriptionDocuments = new List<SubscriptionDocumentDto>();
            var contextData = _unitOfWork.DocumentRepository.Context;
            try
            {
                subscriptionDocuments = (from document in contextData.Document
                                         join investment in contextData.Investment on document.InvestmentId equals investment.Id
                                         join user in contextData.UserAccount on document.UserId equals user.Id
                                         join profile in contextData.UserProfile on document.ProfileId equals profile.Id
                                         join profileType in contextData.UserProfileType on profile.Type equals profileType.Id
                                         where (document.OfferingId == offeringId || investment.OfferingId == offeringId) && document.Type == 2 && document.Active == true
                                         select new SubscriptionDocumentDto
                                         {
                                             Id = document.Id,
                                             UserId = user.Id,
                                             InvestorName = user.FirstName + " " + user.LastName,
                                             SignStatus = investment.IseSignCompleted ? "Completed": "Not Completed",
                                             SignType = "eSign",
                                             CompletedSign = " ",
                                             ProfileType = profile.Type,
                                             ProfileTypeName = profileType.Name,
                                             FileName = document.Name,
                                             Extension = document.Extension,
                                             FilePath = document.FilePath,
                                             Status = document.Status,
                                             Size = document.Size,
                                             Active = document.Active,
                                             CreatedOn = document.CreatedOn,
                                             CreatedBy = document.CreatedBy
                                         }).ToList();
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                subscriptionDocuments = null;
            }
            finally
            {
                contextData = null;
            }
            return subscriptionDocuments;
        }
        public List<DocumentDto> GetAllDocuments(int userId)
        {
            List<DocumentDto> documents = new List<DocumentDto>();
            List<DocumentDto> offeringDocuments = new List<DocumentDto>();
            List<DocumentDto> taxDocuments = new List<DocumentDto>();
            var contextData = _unitOfWork.DocumentRepository.Context;
            try
            {
                documents = (from document in contextData.Document
                             where document.UserId == userId && document.Type != 1 && document.Type != 4 && document.Active == true //Exclude Tax & Offering Documents
                             select new DocumentDto
                             {
                                 Id = document.Id,
                                 Type = document.Type,
                                 Name = document.Name,
                                 Extension = document.Extension,
                                 FilePath = document.FilePath,
                                 Status = document.Status,
                                 Size = document.Size,
                                 Active = document.Active,
                                 CreatedOn = document.CreatedOn,
                                 CreatedBy = document.CreatedBy
                             }).ToList();
                //where Offering Documents of the Investments the Investor has Either Invested in Currently (Active Offering)
                //or made a Soft Commit (Reservations) against can be viewed and displayed by the Investor 
                offeringDocuments = (from investment in contextData.Investment
                                     join offering in contextData.PortfolioOffering on investment.OfferingId equals offering.Id
                                     join document in contextData.Document on offering.Id equals document.OfferingId
                                     where investment.UserId == userId && offering.Active == true
                                     select new DocumentDto
                                     {
                                         OfferingId = document.OfferingId,
                                         OfferingName = offering.Name,
                                         Type = document.Type,
                                         Name = document.Name,
                                         Extension = document.Extension,
                                         FilePath = document.FilePath,
                                         Status = document.Status,
                                         Size = document.Size,
                                         Active = document.Active,
                                         CreatedOn = document.CreatedOn,
                                         CreatedBy = document.CreatedBy
                                     }).Distinct().ToList();
                documents.AddRange(offeringDocuments);
                //Any Tax related Documents that have been filed by TechTown Equity with regards to the Distributions
                //(Payments) made to the Investor
                taxDocuments = (from payment in contextData.Payment
                                join investment in contextData.Investment on payment.InvestmentId equals investment.Id
                                join offering in contextData.PortfolioOffering on investment.OfferingId equals offering.Id
                                join document in contextData.Document on offering.Id equals document.OfferingId
                                where investment.UserId == userId & payment.Type == 1// Credit
                                select new DocumentDto
                                {
                                    OfferingId = document.OfferingId,
                                    OfferingName = offering.Name,
                                    Type = document.Type,
                                    Name = document.Name,
                                    Extension = document.Extension,
                                    FilePath = document.FilePath,
                                    Status = document.Status,
                                    Size = document.Size,
                                    Active = document.Active,
                                    CreatedOn = document.CreatedOn,
                                    CreatedBy = document.CreatedBy
                                }).Distinct().ToList();
                documents.AddRange(taxDocuments);
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                documents = null;
            }
            finally
            {
                contextData = null;
            }
            return documents;
        }
        public async Task<bool> AddDocuments(DocumentModelDto documentDto)
        {
            var contextData = _unitOfWork.DocumentRepository.Context;

            try
            {
                List<Document> documents = new List<Document>();
                using (var transaction = _unitOfWork.DocumentRepository.Context.Database.BeginTransaction())
                {
                    if (documentDto.Files != null)
                    {
                        foreach (var file in documentDto.Files)
                        {
                            Helper _helper = new Helper(_configuration);
                            var blobFilePath = (await _helper.DocumentSaveAndUpload(file, documentDto.UserId, documentDto.Type)).ToString();

                            Document document = new Document();
                            document.UserId = documentDto.UserId;
                            document.FilePath = blobFilePath;
                            document.Type = documentDto.Type;
                            document.Name = file.FileName;
                            var extn = Path.GetExtension(file.FileName);
                            document.Extension = extn.Replace(".", "");
                            document.Size = (file.Length / 1024).ToString();
                            document.Status = 1;//Pending
                            document.Active = true;
                            if (documentDto.AdminUserId == 0)
                                document.CreatedBy = documentDto.UserId.ToString();
                            else
                                document.CreatedBy = documentDto.AdminUserId.ToString();
                            document.CreatedOn = DateTime.Now;
                            documents.Add(document);
                        }
                    }
                    contextData.AddRange(documents);
                    contextData.SaveChanges();
                    transaction.Commit();
                    string message = "You uploaded document successfully";
                    var notification = _notificationRepository.AddNotification(documentDto.UserId, "Document Added", message);
                    if (notification != null)
                    {
                        await _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return false;//Failure 
            }
            finally
            {
                contextData = null;
            }
            return true;
        }
        public int CreateBatch(DocumentBatchDetailDto documentBatchDetailDto)
        {
            int batchId = 0;
            try
            {
                using (var transaction = _unitOfWork.DocumentRepository.Context.Database.BeginTransaction())
                {
                    DocumentBatchDetail documentBatchDetailData = new DocumentBatchDetail();
                    documentBatchDetailData.BatchName = documentBatchDetailDto.BatchName;
                    documentBatchDetailData.DocumentType = documentBatchDetailDto.DocumentType;
                    documentBatchDetailData.NameDelimiter = documentBatchDetailDto.NameDelimiter;
                    documentBatchDetailData.NameSeparator = documentBatchDetailDto.NameSeparator;
                    documentBatchDetailData.NamePosition = documentBatchDetailDto.NamePosition;
                    documentBatchDetailData.TotalDocuments = documentBatchDetailDto.TotalDocuments;
                    documentBatchDetailData.Status = 1; // Uploading
                    documentBatchDetailData.Active = true;
                    documentBatchDetailData.CreatedBy = documentBatchDetailDto.AdminUserId.ToString();
                    documentBatchDetailData.CreatedOn = DateTime.Now;
                    _unitOfWork.DocumentBatchDetailRepository.Insert(documentBatchDetailData);
                    _unitOfWork.Save();
                    transaction.Commit();
                    batchId = documentBatchDetailData.Id;
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return batchId;//Failure 
            }
            return batchId;
        }
        public async Task<bool> AddDocumentsInBatch(List<DocumentBatchModel> documentBatchDto)
        {
            var contextData = _unitOfWork.DocumentRepository.Context;

            try
            {
                List<Document> documents = new List<Document>();
                using (var transaction = _unitOfWork.DocumentRepository.Context.Database.BeginTransaction())
                {
                    if (documentBatchDto != null)
                    {
                        foreach (var documentDto in documentBatchDto)
                        {
                            Helper _helper = new Helper(_configuration);

                            var blobFilePath = (await _helper.DocumentSaveAndUpload(documentDto.Document, documentDto.UserId, documentDto.Type)).ToString();
                            int profileId = 0;
                            var profile = _unitOfWork.UserProfileRepository.Get(x => x.UserId == documentDto.UserId && x.IsOwner == true);
                            if (profile != null)
                                profileId = profile.Id;                            
                            Document documentData = new Document();
                            documentData.BatchId = documentDto.BatchId;
                            documentData.UserId = documentDto.UserId;
                            documentData.ProfileId = documentDto.ProfileId == 0 ? profileId : documentDto.ProfileId;
                            documentData.FilePath = blobFilePath;
                            documentData.Type = documentDto.Type;
                            documentData.Name = documentDto.Document.FileName;
                            var extn = Path.GetExtension(documentDto.Document.FileName);
                            documentData.Extension = extn.Replace(".", "");
                            documentData.Size = (documentDto.Document.Length / 1024).ToString();
                            documentData.Status = 1;//Pending
                            documentData.Active = true;
                            documentData.CreatedBy = documentDto.AdminUserId.ToString();
                            documentData.CreatedOn = DateTime.Now;
                            documents.Add(documentData);
                        }
                    }
                    contextData.AddRange(documents);
                    contextData.SaveChanges();
                    transaction.Commit();

                    foreach (var document in documentBatchDto)
                    {
                        string message = GetDocumentTypeName(document.Type) + "document uploaded by Admin";
                        var notification = _notificationRepository.AddNotification(document.UserId, "Document uploaded", message);
                        if (notification != null)
                        {
                            await _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return false;//Failure 
            }
            finally
            {
                contextData = null;
            }
            return true;
        }

        private string GetDocumentTypeName(int documentType)
        {
            switch (documentType)
            {
                case 1:
                    return "Tax";
                case 2:
                    return "Subscriptions";
                case 3:
                    return "Accreditation";
                case 4:
                    return "Offering Documents";
                case 5:
                    return "Miscellaneous";
                case 6:
                    return "Updates";
                case 7:
                    return "UserProfileImage";
                default:
                    return "";
            }
        }
        public async Task<string> AddDocument(DocumentModelDto documentDto)
        {
            var contextData = _unitOfWork.DocumentRepository.Context;
            string blobFilePath = null;
            try
            {
                List<Document> documents = new List<Document>();
                using (var transaction = _unitOfWork.DocumentRepository.Context.Database.BeginTransaction())
                {
                    if (documentDto.Files != null && documentDto.Files.Count == 1)
                    {
                        var file = documentDto.Files.First();
                        Helper _helper = new Helper(_configuration);
                        blobFilePath = (await _helper.DocumentSaveAndUpload(file, documentDto.UserId, documentDto.Type)).ToString();

                        Document document = new Document();
                        document.UserId = documentDto.UserId;
                        document.FilePath = blobFilePath;
                        document.Type = documentDto.Type;
                        document.Name = file.FileName;
                        var extn = Path.GetExtension(file.FileName);
                        document.Extension = extn.Replace(".", "");
                        document.Size = (file.Length / 1024).ToString();
                        document.Status = 1;//Pending
                        document.Active = true;
                        if (documentDto.AdminUserId == 0)
                            document.CreatedBy = documentDto.UserId.ToString();
                        else
                            document.CreatedBy = documentDto.AdminUserId.ToString();
                        document.CreatedOn = DateTime.Now;
                        documents.Add(document);
                    }
                    contextData.AddRange(documents);
                    contextData.SaveChanges();
                    transaction.Commit();
                    string message = "You uploaded document successfully";
                    if (documentDto.Type == 8)
                        message = "You uploaded eSigned document successfully";
                    var notification = _notificationRepository.AddNotification(documentDto.UserId, "Document Added", message);
                    if (notification != null)
                    {
                        await _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                    }
                }
                return blobFilePath;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure 
            }
            finally
            {
                contextData = null;
            }
        }
        public int DeleteDocument(int userId, int documentId)
        {
            var documentData = _unitOfWork.DocumentRepository.Get(x => x.Id == documentId);
            if (documentData.Id != 0)
            {
                try
                {
                    using (var transaction = _unitOfWork.DocumentRepository.Context.Database.BeginTransaction())
                    {
                        Document document = documentData;
                        document.Active = false;
                        document.ModifiedBy = userId.ToString();
                        _unitOfWork.DocumentRepository.Update(document);
                        _unitOfWork.Save();
                        transaction.Commit();
                        string message = "You deleted document successfully";
                        var notification = _notificationRepository.AddNotification(userId, "Document Deleted", message);
                        if (notification != null)
                        {
                            _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                        }
                    }
                    return 1;
                }
                catch
                {
                    return 0;
                }
                finally
                {
                    documentData = null;
                }
            }
            return 0;
        }
        public List<DocumentDto> GetDocumentUpdates(int userId)
        {
            List<DocumentDto> documents = new List<DocumentDto>();
            var contextData = _unitOfWork.DocumentRepository.Context;
            try
            {
                documents = (from document in contextData.Document
                             where document.UserId == userId && document.Type == 2 && document.Type == 3 && document.Type == 5  // Subscriptions/Accreditation/Miscellaneous
                             select new DocumentDto
                             {
                                 Id = document.Id,
                                 Type = document.Type,
                                 Name = document.Name,
                                 Extension = document.Extension,
                                 FilePath = document.FilePath,
                                 Status = document.Status,
                                 Size = document.Size,
                                 Active = document.Active,
                                 CreatedOn = document.CreatedOn,
                                 CreatedBy = document.CreatedBy,
                                 ModifiedOn = document.ModifiedOn,
                                 ModifiedBy = document.ModifiedBy,
                                 ApprovedOn = document.ApprovedOn,
                                 ApprovedBy = document.ApprovedBy
                             }).ToList();
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                documents = null;
            }
            finally
            {
                contextData = null;
            }
            return documents;

        }
        public List<DocumentNameDelimiterDto> GetDocumentNameDelimiters()
        {
            List<DocumentNameDelimiterDto> documentDelimiters = new List<DocumentNameDelimiterDto>();
            var contextData = _unitOfWork.DocumentNameDelimiterRepository.Context;
            try
            {
                documentDelimiters = (from type in contextData.DocumentNameDelimiter
                                      select new DocumentNameDelimiterDto
                                      {
                                          Id = type.Id,
                                          Name = type.Name,
                                          Active = type.Active,
                                      }).ToList();
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                documentDelimiters = null;
            }
            finally
            {
                contextData = null;
            }
            return documentDelimiters;
        }
        public List<DocumentNamePositionDto> GetDocumentNamePositions()
        {
            List<DocumentNamePositionDto> documentNamePositions = new List<DocumentNamePositionDto>();
            var contextData = _unitOfWork.DocumentNamePositionRepository.Context;
            try
            {
                documentNamePositions = (from type in contextData.DocumentNamePosition
                                         select new DocumentNamePositionDto
                                         {
                                             Id = type.Id,
                                             Name = type.Name,
                                             Active = type.Active,
                                         }).ToList();
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                documentNamePositions = null;
            }
            finally
            {
                contextData = null;
            }
            return documentNamePositions;
        }
        public List<DocumentNameSeparatorDto> GetDocumentNameSeparators()
        {
            List<DocumentNameSeparatorDto> documentNameSeparators = new List<DocumentNameSeparatorDto>();
            var contextData = _unitOfWork.DocumentNameSeparatorRepository.Context;
            try
            {
                documentNameSeparators = (from type in contextData.DocumentNameSeparator
                                          select new DocumentNameSeparatorDto
                                          {
                                              Id = type.Id,
                                              Name = type.Name,
                                              Active = type.Active,
                                          }).ToList();
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                documentNameSeparators = null;
            }
            finally
            {
                contextData = null;
            }
            return documentNameSeparators;
        }
        public bool DeleteDocumentBatch(int adminuserid, int batchid)
        {
            var contextData = _unitOfWork.DocumentBatchDetailRepository.Context;
            try
            {
                using (var batchTransaction = contextData.Database.BeginTransaction())
                {
                    var documentBatchDetail = _unitOfWork.DocumentBatchDetailRepository.GetByID(batchid);
                    documentBatchDetail.Active = false;
                    documentBatchDetail.ModifiedBy = adminuserid.ToString();
                    _unitOfWork.DocumentBatchDetailRepository.Update(documentBatchDetail);
                    contextData.SaveChanges();
                    batchTransaction.Commit();

                    using (var docoumnetTransaction = contextData.Database.BeginTransaction())
                    {
                        try
                        {
                            var documents = _unitOfWork.DocumentRepository.GetMany(x => x.BatchId == batchid && x.Active == true);
                            List<Document> documentList = new List<Document>();
                            foreach (var document in documents)
                            {
                                document.Active = false;
                                document.ModifiedBy = adminuserid.ToString();
                                documentList.Add(document);
                            }
                            _unitOfWork.DocumentRepository.UpdateList(documentList);
                            _unitOfWork.Save();
                            docoumnetTransaction.Commit();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                            batchTransaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }
            finally
            {
                contextData = null;
            }
            return true;
        }
        public bool DeleteDocumentInBatch(int adminuserid, int batchid, int userid)
        {
            var contextData = _unitOfWork.DocumentBatchDetailRepository.Context;
            try
            {
                using (var docoumnetTransaction = contextData.Database.BeginTransaction())
                {
                    var document = _unitOfWork.DocumentRepository.Get(x => x.BatchId == batchid && x.UserId == userid && x.Active == true);
                    document.Active = false;
                    document.ModifiedBy = adminuserid.ToString();
                    _unitOfWork.DocumentRepository.Update(document);
                    _unitOfWork.Save();
                    docoumnetTransaction.Commit();
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }
            finally
            {
                contextData = null;
            }
            return true;
        }
        public bool DeleteDocumentInBatchByUserId(int adminuserid, int batchid, int userid)
        {
            var contextData = _unitOfWork.DocumentBatchDetailRepository.Context;
            try
            {
                using (var docoumnetTransaction = contextData.Database.BeginTransaction())
                {
                    var document = _unitOfWork.DocumentRepository.Get(x => x.BatchId == batchid && x.UserId == userid && x.Active == true);
                    document.Active = false;
                    document.ModifiedBy = adminuserid.ToString();
                    _unitOfWork.DocumentRepository.Update(document);
                    _unitOfWork.Save();
                    docoumnetTransaction.Commit();
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }
            finally
            {
                contextData = null;
            }
            return true;
        }
        public bool DeleteDocumentInBatchById(int adminuserid, int batchid, int documentid)
        {
            var contextData = _unitOfWork.DocumentBatchDetailRepository.Context;
            try
            {
                using (var docoumnetTransaction = contextData.Database.BeginTransaction())
                {
                    var document = _unitOfWork.DocumentRepository.Get(x => x.BatchId == batchid && x.Id == documentid && x.Active == true);
                    document.Active = false;
                    document.ModifiedBy = adminuserid.ToString();
                    _unitOfWork.DocumentRepository.Update(document);
                    _unitOfWork.Save();
                    docoumnetTransaction.Commit();
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }
            finally
            {
                contextData = null;
            }
            return true;
        }
        public bool UpdateBatch(int adminuserid, int batchid, int status, int totalDocuments = 0)
        {
            try
            {
                using (var batchTransaction = _unitOfWork.DocumentBatchDetailRepository.Context.Database.BeginTransaction())
                {
                    var batch = _unitOfWork.DocumentBatchDetailRepository.Get(x => x.Id == batchid && x.Active == true);
                    batch.Status = status;
                    batch.TotalDocuments = totalDocuments;
                    batch.ModifiedBy = adminuserid.ToString();
                    _unitOfWork.DocumentBatchDetailRepository.Update(batch);
                    _unitOfWork.Save();
                    batchTransaction.Commit();
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }
            return true;
        }
        public DocumentBatchDetailDto GetDocumentBatchDetail(int batchId)
        {
            DocumentBatchDetailDto documentBatchDetail = new DocumentBatchDetailDto();
            var contextData = _unitOfWork.DocumentBatchDetailRepository.Context;
            try
            {
                documentBatchDetail = (from batch in contextData.DocumentBatchDetail
                                       where batch.Id == batchId
                                       && batch.Active == true
                                       select new DocumentBatchDetailDto
                                       {
                                           Id = batch.Id,
                                           BatchName = batch.BatchName,
                                           Status = batch.Status,
                                           TotalDocuments = batch.TotalDocuments,
                                           DocumentType = batch.DocumentType,
                                           NameDelimiter = batch.NameDelimiter,
                                           NamePosition = batch.NamePosition,
                                           NameSeparator = batch.NameSeparator,
                                           CreatedOn = batch.CreatedOn,
                                           CreatedBy = batch.CreatedBy,
                                           Documents = (from document in contextData.Document
                                                        join user in contextData.UserAccount
                                                            on document.UserId equals user.Id
                                                        join profile in contextData.UserProfile
                                                          on document.ProfileId equals profile.Id into pro
                                                        from profile in pro.DefaultIfEmpty()
                                                        where document.BatchId == batch.Id
                                                            && document.Active == true
                                                        select new DocumentBatchModel
                                                        {
                                                            DocumentId = document.Id,
                                                            DocumentName = document.Name,
                                                            Type = document.Type,
                                                            BatchId = Convert.ToInt32(document.BatchId),
                                                            UserId = Convert.ToInt32(document.UserId),
                                                            ProfileId = document.ProfileId != null ? Convert.ToInt32(document.ProfileId) : 0,
                                                            DocumentPath = document.FilePath,
                                                            ProfileName = document.ProfileId != null ?
                                                            (profile.Name + profile.FirstName + " " + profile.LastName + profile.RetirementPlanName + profile.TrustName) : "",
                                                            InvestorName = user.FirstName + " " + user.LastName
                                                        }
                                                        ).ToList()
                                       }).FirstOrDefault();
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                documentBatchDetail = null;
            }
            finally
            {
                contextData = null;
            }
            return documentBatchDetail;
        }
        public List<DocumentBatchDetailDto> GetAllDocumentBatches()
        {
            List<DocumentBatchDetailDto> documentBatches = new List<DocumentBatchDetailDto>();
            var contextData = _unitOfWork.DocumentBatchDetailRepository.Context;
            try
            {
                documentBatches = (from batch in contextData.DocumentBatchDetail
                                   where batch.Active == true && (batch.Status == 3 || batch.Status == 1 || batch.Status == 2)
                                   select new DocumentBatchDetailDto
                                   {
                                       Id = batch.Id,
                                       BatchName = batch.BatchName,
                                       Status = batch.Status,
                                       TotalDocuments = batch.TotalDocuments,
                                       DocumentType = batch.DocumentType,
                                       CreatedOn = batch.CreatedOn,
                                       CreatedBy = batch.CreatedBy,
                                       Documents = null
                                   }).OrderByDescending(x => x.CreatedOn).ToList();
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                documentBatches = null;
            }
            finally
            {
                contextData = null;
            }
            return documentBatches;
        }

    }
}
