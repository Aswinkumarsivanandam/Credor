using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API.Controllers
{
    [Route("Documents")]      
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentsService;       

        public DocumentsController(IDocumentService documentService)
        {          
            _documentsService = documentService;
        }
        [HttpGet]
        [Route("getdocumenttypes")]
        public IActionResult GetDocumentTypes()
        {
            var updates = _documentsService.GetDocumentTypes();
            return Ok(updates);
        }
        [HttpGet]
        [Route("getalldocuments/{userid}")]
        public IActionResult GetAllDocuments(int userid)
        {
            var updates = _documentsService.GetAllDocuments(userid);
            return Ok(updates);
        }
        [HttpPost]
        [Route("uploaddocuments")]      
        public async Task<IActionResult> UploadDocuments([FromForm] DocumentModelDto documents)
        {            
            var result = await _documentsService.AddDocuments(documents);
            if (result)
                return Ok(result);
            else
                return Ok();
        }
        [HttpPost, DisableRequestSizeLimit]
        [Route("uploaddocumentstest")]
        public async Task<IActionResult> UploadDocumentstest(IFormFile file1, IFormFile file2)
        {
            List<IFormFile> filelist = new List<IFormFile>();
            DocumentModelDto documents = new DocumentModelDto();            
            documents.UserId = 17;
            documents.Type = 2;
            //documents.Files = filelist;
            //documents.Files.Add(file1);
            //documents.Files.Add(file2);           
            var result = await _documentsService.AddDocuments(documents);
            if (result)
                return Ok(result);
            else
                return Ok();
        }
        [HttpDelete]
        [Route("deletedocument/{userid}/{documentid}")]
        public IActionResult DeleteDocument(int userid,int documentid)
        {
            var updates = _documentsService.DeleteDocument(userid,documentid);
            return Ok(updates);
        }
        [HttpGet]
        [Route("getdocumentstatuses)")]
        public IActionResult GetDocumentStatuses()
        {
            var updates = _documentsService.GetDocumentStatuses();
            return Ok(updates);
        }
        [HttpGet]
        [Route("getdocumentupdates/{userid}")]
        public IActionResult GetDocumentUpdates(int userid)
        {
            var updates = _documentsService.GetDocumentUpdates(userid);
            return Ok(updates);
        }
        [HttpGet]
        [Route("getallsubscriptiondocuments")]
        public IActionResult GetAllSubscriptionDocuments()
        {
            var updates = _documentsService.GetAllSubscriptionDocuments();
            return Ok(updates);
        }

        [HttpGet]
        [Route("getallsubscriptiondocuments/{offeringid}")]
        public IActionResult GetAllSubscriptionDocuments(int offeringid)
        {
            var updates = _documentsService.GetAllSubscriptionDocuments(offeringid);
            return Ok(updates);
        }

    }
}
