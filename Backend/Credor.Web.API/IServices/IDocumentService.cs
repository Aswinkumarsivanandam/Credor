using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API
{
   public interface IDocumentService
    {
        List<DocumentTypesDto> GetDocumentTypes();
        List<DocumentStatusDto> GetDocumentStatuses();
        List<DocumentDto> GetAllDocuments(int userId);
        Task<bool> AddDocuments(DocumentModelDto documents);
        int DeleteDocument(int userId, int documentId);
        List<DocumentDto> GetDocumentUpdates(int userId);
        List<DocumentNameDelimiterDto> GetDocumentNameDelimiters();
        List<DocumentNamePositionDto> GetDocumentNamePositions();
        List<DocumentNameSeparatorDto> GetDocumentNameSeparators();
        List<DocumentBatchModel> bulkdocumentupload(UploadDocumentModelDto documents);
        Task<DocumentBatchDetailDto> PublishMatchedDocuments(PublishDocumentModelDto documents);
        Task<int> PublishMatchedDocument(PublishDocumentDto document);
        bool DeleteDocumentBatch(int adminuserid, int batchid);
        bool DeleteDocumentInBatchByUserId(int adminuserid, int batchid, int userid);
        bool DeleteDocumentInBatchById(int adminuserid, int batchid, int documentid);
        DocumentBatchDetailDto GetDocumentBatchDetail(int batchid);
        List<DocumentBatchDetailDto> GetAllDocumentBatches();
        bool UpdateBatch(int adminuserid, int batchid, int status, int totalDocuments = 0);
        bool UpdateBatch(UpdateBatchDto batchDto);
        List<SubscriptionDocumentDto> GetAllSubscriptionDocuments();
        List<SubscriptionDocumentDto> GetAllSubscriptionDocuments(int offeringId);
    }
}
