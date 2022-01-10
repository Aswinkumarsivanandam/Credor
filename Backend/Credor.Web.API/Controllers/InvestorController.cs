using Credor.Client.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Credor.Web.API.Controllers
{
    [Route("Investor")]
    [Produces("application/json")]
    [ApiController]    
    public class InvestorController : ControllerBase
    {
        private readonly IInvestorService _investorService;
        private readonly INotificationService _notificationService;
        private readonly IProfileService _profileService;
        private readonly IDistributionsService _distributionsService;
        private readonly IDocumentService _documentService;
        private readonly IAccountService _accountService;
        public IConfiguration _configuration { get; }

        public InvestorController(IInvestorService investorService,
                                   INotificationService notificationService,
                                   IProfileService profileService,
                                   IDistributionsService distributionsService,
                                   IDocumentService documentService,
                                   IAccountService accountService,
                                   IConfiguration configuration)
        {
            _investorService = investorService;
            _notificationService = notificationService;
            _profileService = profileService;
            _distributionsService = distributionsService;
            _documentService = documentService;
            _accountService = accountService;
            _configuration = configuration;

        }
        [HttpGet]
        [Route("getuseraccount/{id}")]
        public IActionResult GetUserAccount(int id)
        {
            var userAccount = _investorService.GetUserAccount(id);
            return Ok(userAccount);
        }
        [HttpGet]
        [Route("getallinvestoraccounts")]
        public IActionResult GetAllInvestorAccounts()
        {
            var userAccounts = _investorService.GetAllInvestorAccounts();
            return Ok(userAccounts);
        }
        [HttpPut]
        [Route("updateinvestoraccount")]
        public IActionResult UpdateInvestorAccount(UserAccountDto userAccount)
        {
            var result = _investorService.UpdateInvestorAccount(userAccount);
            return Ok(result);
        }
        [HttpPost]
        [Route("addinvestornotes")]
        public IActionResult AddInvestorNotes([FromBody] UserNotesDto notes)
        {
            var result = _investorService.AddInvestorNotes(notes);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateinvestornotes")]
        public IActionResult UpdateInvestorNotes([FromBody] UserNotesDto notes)
        {
            var result = _investorService.UpdateInvestorNotes(notes);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deleteinvestornotes/{adminuserid}/{investorid}")]
        public IActionResult DeleteInvestorNotes(int adminuserid, int investorid)
        {
            var result = _investorService.DeleteInvestorNotes(adminuserid, investorid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getinvestornotes/{investorid}")]
        public IActionResult GetInvestorNotes(int investorid)
        {
            var result = _investorService.GetInvestorNotes(investorid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getinvestortags/{adminuserid}")]
        public IActionResult GetInvestorTags(int adminuserid)
        {
            var result = _investorService.GetInvestorTags(adminuserid);
            return Ok(result);
        }
        [HttpPost]
        [Route("addinvestortags")]
        public IActionResult AddInvestorTags(TagDto tag)
        {
            var result = _investorService.AddInvestorTags(tag);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateinvestortags")]
        public IActionResult UpdateInvestorTags(TagDto tag)
        {
            var result = _investorService.UpdateInvestorTags(tag);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deleteinvestortags/{adminuserid}/{tagid}")]
        public IActionResult DeleteInvestorTags(int adminuserid, int tagid)
        {
            var result = _investorService.DeleteInvestorTags(adminuserid, tagid);
            return Ok(result);
        }
        [HttpPut]
        [Route("resetpassword")]
        public IActionResult ResetPassword(ResetPasswordDto passwordDto)
        {
            var result = _investorService.ResetPassword(passwordDto);
            return Ok(result);
        }
        [HttpPut]
        [Route("sendresetpasswordlink")]
        public async Task<IActionResult> SendResetPasswordLink(ResetPasswordDto passwordDto)
        {
            var result = await _investorService.SendResetPasswordLink(passwordDto);
            return Ok(result);
        }
        [HttpGet]
        [Route("accountverification/{adminuserid}/{investorid}/{isverify}")]
        public IActionResult VerifyAccount(int adminuserid, int investorid,bool isverify)
        {
            var result = _investorService.AccountVerification(adminuserid, investorid, isverify);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("updateaccreditation/{adminuserid}/{investorid}/{isverify}")]
        public IActionResult VerifyAccreditation(int adminuserid, int investorid,bool isverify)
        {
            var result = _investorService.UpdateAccreditation(adminuserid, investorid, isverify);
            return Ok(result);
        }
        [HttpGet]
        [Route("getalldocuments/{userid}")]
        public IActionResult GetAllDocuments(int userid)
        {
            var updates = _documentService.GetAllDocuments(userid);
            return Ok(updates);
        }

        [HttpPost]
        [Route("uploaddocuments")]
        public async Task<IActionResult> UploadDocuments([FromForm] DocumentModelDto documents)
        {
            var result = await _documentService.AddDocuments(documents);
            if (result)
                return Ok(result);
            else
                return Ok();
        }
        [HttpGet]
        [Route("getinvestorsummary/{investorid}")]
        public IActionResult GetHeaderElements(int investorid)
        {
            var list = _investorService.GetHeaderElements(investorid);
            return Ok(list);
        }
        [HttpGet]
        [Route("getinvestorssummary")]
        public IActionResult GetInvestorsHeaderElements()
        {
            var list = _investorService.GetInvestorsHeaderElements();
            return Ok(list);
        }
        [HttpGet]
        [Route("getreservationlist")]
        public IActionResult GetReservationList()
        {
            var reservations = _investorService.GetReservationList();
            return Ok(reservations);
        }
        [HttpGet]
        [Route("getofferinglist")]
        public IActionResult GetOfferingList()
        {
            var reservations = _investorService.GetOfferingList();
            return Ok(reservations);
        }
        [HttpGet]
        [Route("getreservations/{investorid}")]
        public IActionResult GetReservations(int investorid)
        {
            var reservations = _investorService.GetReservations(investorid);
            return Ok(reservations);
        }
        [HttpGet]
        [Route("getinvestments/{investorid}")]
        public IActionResult GetInvestments(int investorid)
        {
            var investments = _investorService.GetInvestments(investorid);
            return Ok(investments);
        }
        [HttpGet]
        [Route("getalluserprofile/{investorid}")]
        public IActionResult GetAllUserProfile(int investorid)
        {
            var profiles = _investorService.GetAllUserProfile(investorid);
            return Ok(profiles);
        }
        [HttpGet]
        [Route("getdistributions/{investorid}")]
        public IActionResult GetDistributions(int investorid)
        {
            var distributions = _distributionsService.GetAllDistributions(investorid);
            return Ok(distributions);
        }
        [HttpGet]
        [Route("getusernotifications/{userid}")]
        public IActionResult GetUserNotifications(int userid)
        {
            var notifications = _notificationService.GetUserNotifications(userid);
            return Ok(notifications);
        }
        [HttpPost]
        [Route("addreservation")]
        public IActionResult AddReservation(ReservationDataDto reservationDataDto)
        {
            var status = _investorService.AddReservation(reservationDataDto);
            return Ok(status);
        }
        [HttpPut]
        [Route("updatereservation")]
        public IActionResult UpdateReservation(ReservationDataDto reservationDataDto)
        {
            var status = _investorService.UpdateReservation(reservationDataDto);
            return Ok(status);
        }
        [HttpDelete]
        [Route("deletereservation/{adminuserid}/{reservationid}")]
        public IActionResult DeleteReservation(int adminuserid, int reservationid)
        {
            var status = _investorService.DeleteReservation(adminuserid, reservationid);
            return Ok(status);
        }
        [HttpPost]
        [Route("addinvestment")]
        public IActionResult AddInvestment(InvestmentDataDto investmentDataDto)
        {
            var status = _investorService.AddInvestment(investmentDataDto);
            return Ok(status);
        }
        [HttpPut]
        [Route("updateinvestment")]
        public IActionResult UpdateInvestment(InvestmentDataDto investmentDataDto)
        {
            var status = _investorService.UpdateInvestment(investmentDataDto);
            return Ok(status);
        }
        [HttpDelete]
        [Route("deleteinvestment/{adminuserid}/{reservationid}")]
        public IActionResult DeleteInvestment(int adminuserid, int reservationid)
        {
            var status = _investorService.DeleteInvestment(adminuserid, reservationid);
            return Ok(status);
        }
        [HttpPost]
        [Route("addprofile")]
        public IActionResult AddProfile(UserProfileDto userProfileDto)
        {
            var status = _profileService.CreateUserProfile(userProfileDto);
            return Ok(status);
        }
        [HttpPut]
        [Route("updateprofile")]
        public IActionResult UpdateProfile(UserProfileDto userProfileDto)
        {
            var status = _profileService.UpdateUserProfile(userProfileDto);
            return Ok(status);
        }
        [HttpDelete]
        [Route("deleteuserprofile/{adminuserid}/{userid}/{userprofileid}")]
        public IActionResult DeleteUserProfile(int userid, int userprofileid, int adminuserid)
        {
            var result = _profileService.DeleteUserProfile(userid, userprofileid, adminuserid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getuserprofile/{userid}/{userprofileid}")]
        public IActionResult GetUserProfile(int userid, int userprofileid)
        {
            var result = _profileService.GetUserProfile(userid, userprofileid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getaccessgrantedtoothers/{userid}")]
        public IActionResult GetAccessGrantedToOthers(int userid)
        {
            var result = _accountService.GetAccessGrantedToOthers(userid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getdocumentnamedelimiters")]
        public IActionResult GetDocumentNameDelimiters()
        {
            var result = _documentService.GetDocumentNameDelimiters();
            return Ok(result);
        }
        [HttpGet]
        [Route("getdocumentnamepositions")]
        public IActionResult GetDocumentNamePositions()
        {
            var result = _documentService.GetDocumentNamePositions();
            return Ok(result);
        }
        [HttpGet]
        [Route("getdocumentnameseparators")]
        public IActionResult GetDocumentNameSeparators()
        {
            var result = _documentService.GetDocumentNameSeparators();
            return Ok(result);
        }
        [HttpDelete]
        [Route("deletedocumentbatch/{adminuserid}/{batchid}")]
        public IActionResult DeleteDocumentbatch(int adminuserid, int batchid)
        {
            var result =  _documentService.DeleteDocumentBatch(adminuserid,batchid);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deletedocumentinbatchbyuserid/{adminuserid}/{batchid}/{userid}")]
        public IActionResult DeleteDocumentInBatchByUserId(int adminuserid, int batchid, int userid)
        {
            var result = _documentService.DeleteDocumentInBatchByUserId(adminuserid, batchid,userid);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deletedocumentinbatchbyid/{adminuserid}/{batchid}/{documentid}")]
        public IActionResult DeleteDocumentInBatchById(int adminuserid, int batchid, int documentid)
        {
            var result = _documentService.DeleteDocumentInBatchById(adminuserid, batchid, documentid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getdocumentbatchdetail/{batchid}")]
        public IActionResult GetDocumentBatchDetail(int batchid)
        {
            var result = _documentService.GetDocumentBatchDetail(batchid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getalldocumentbatches")]
        public IActionResult GetAllDocumentBatches()
        {
            var result = _documentService.GetAllDocumentBatches();
            return Ok(result);
        }
        [HttpPut]
        [Route("updatebatch")]
        public IActionResult UpdateBatch(UpdateBatchDto batchDto)
        {
            var status = _documentService.UpdateBatch(batchDto);
            return Ok(status);
        }
        [HttpGet]
        [Route("getaccountstatement/{investorid}")]
        public IActionResult GetAccountStatement(int investorid)
        {           
            return Ok();
        }
        [HttpPost]
        [Route("publishmatcheddocuments")]
        public async Task<IActionResult> PublishMatchedDocuments([FromForm] PublishDocumentModelDto documents)
        {

            try
            {
                var canParse = Request.Form.TryGetValue("MatchedDocuments", out var model);
                if (canParse)
                {
                    var strMatchedDocuments = model.ToList();
                    List<MatchedDocument> matchedDocuments = new List<MatchedDocument>();
                    foreach (var doc in strMatchedDocuments)
                    {
                        var data = JsonConvert.DeserializeObject<MatchedDocument>(doc);
                        matchedDocuments.Add(data);
                    }
                    documents.MatchedDocs = matchedDocuments;
                }                
                var result = await _documentService.PublishMatchedDocuments(documents);
                return Ok(result);
            }
            catch(Exception e)
            {
                e.ToString();
                return Ok();
            }
        }

        [HttpPost]
        [Route("publishmatcheddocument")]
        public async Task<IActionResult> PublishMatchedDocument([FromForm] PublishDocumentDto document)
        {
            var result = await _documentService.PublishMatchedDocument(document);
            return Ok(result);
        }

        [HttpPost]
        [Route("bulkdocumentupload")]
        public IActionResult bulkdocumentupload([FromForm] UploadDocumentModelDto documents)
        {
            var result = _documentService.bulkdocumentupload(documents);            
            return Ok(result);
        }
        /*[HttpPost]
        [Route("bulkdocumentuploadtest")]
        public async Task<IActionResult> bulkdocumentuploadtest(IFormFile file1, IFormFile file2, IFormFile file3)
        {
            List<IFormFile> filelist = new List<IFormFile>();
            DocumentBatchDetailDto documents = new DocumentBatchDetailDto();
            documents.Id = 1;
            documents.AdminUserId = 34;
            documents.DocumentType = 1;
            documents.BatchName = "Bulk upload 1";
            documents.NameDelimiter = 3;
            documents.NamePosition = 2;
            documents.NameSeparator = 1;
            //documents.Documents = filelist;
            //documents.Documents.Add(file1);
            //documents.Documents.Add(file2);
            //documents.Documents.Add(file3);

            List<DocumentBatchModel> matchedDocuments = new List<DocumentBatchModel>();
            DocumentBatchModel document1 = new DocumentBatchModel();
            document1.AdminUserId = 34;
            document1.BatchId = 1;
            document1.ProfileId = 40;
            document1.UserId = 40;
            document1.Document = file1;
            matchedDocuments.Add(document1);
            DocumentBatchModel document2 = new DocumentBatchModel();
            document2.AdminUserId = 34;
            document2.BatchId = 1;
            document2.ProfileId = 43;
            document2.UserId = 51;
            document2.Document = file2;
            matchedDocuments.Add(document2);
            DocumentBatchModel document3 = new DocumentBatchModel();
            document3.AdminUserId = 34;
            document3.BatchId = 1;
            document3.ProfileId = 1;
            document3.UserId = 8;
            document3.Document = file3;
            matchedDocuments.Add(document3);

            documents.MatchedDocuments = matchedDocuments;
                                   
            var result = await _documentService.bulkdocumentupload(documents);
            return Ok();
        }
        */
    }
}
