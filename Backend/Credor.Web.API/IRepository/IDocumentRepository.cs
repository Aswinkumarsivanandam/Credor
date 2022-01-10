using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public interface IDocumentRepository
    {
        List<DocumentTypesDto> GetDocumentTypes();
        List<DocumentStatusDto> GetDocumentStatuses();
        List<DocumentDto> GetAllDocuments(int userId);
        Task<bool> AddDocuments(DocumentModelDto documents);
        Task<string> AddDocument(DocumentModelDto documentDto);
        int DeleteDocument(int userId, int documentId);
        List<DocumentDto> GetDocumentUpdates(int userId);
        List<DocumentNameDelimiterDto> GetDocumentNameDelimiters();
        List<DocumentNamePositionDto> GetDocumentNamePositions();
        List<DocumentNameSeparatorDto> GetDocumentNameSeparators();
        Task<bool> AddDocumentsInBatch(List<DocumentBatchModel> documentBatchDto);
        int CreateBatch(DocumentBatchDetailDto documentBatchDetailDto);
        bool DeleteDocumentBatch(int adminuserid, int batchid);
        bool DeleteDocumentInBatchByUserId(int adminuserid, int batchid, int userid);
        bool DeleteDocumentInBatchById(int adminuserid, int batchid, int id);
        DocumentBatchDetailDto GetDocumentBatchDetail(int batchid);
        List<DocumentBatchDetailDto> GetAllDocumentBatches();
        bool UpdateBatch(int adminuserid, int batchid, int status, int totalDocuments = 0);
        List<SubscriptionDocumentDto> GetAllSubscriptionDocuments();
        List<SubscriptionDocumentDto> GetAllSubscriptionDocuments(int offeringId);
    }
}
