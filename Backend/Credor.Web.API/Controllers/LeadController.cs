using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Data.Entities;
using Credor.Client.Entities;

namespace Credor.Web.API.Controllers
{
    [Route("Lead")]
    [Produces("application/json")]
    [ApiController]
    public class LeadController : ControllerBase
    {           
        private readonly ILeadService _leadService;
        public IConfiguration _configuration { get; }

        public LeadController(ILeadService leadService, IConfiguration configuration)
        {
            _leadService = leadService;
            _configuration = configuration;

        }
        [HttpGet]
        [Route("getuseraccount/{id}")]
        public IActionResult GetUserAccount(int id)
        {
            var userAccount = _leadService.GetUserAccount(id);
            return Ok(userAccount);
        }
        [HttpGet]
        [Route("getallleadaccounts")]
        public IActionResult GetAllLeadAccounts()
        {
            var userAccounts = _leadService.GetAllLeadAccounts();
            return Ok(userAccounts);
        }
        [HttpPost]
        [Route("addleadaccount")]
        public IActionResult AddLeadAccount(UserAccountDto userAccount)
        {
            var result = _leadService.AddLeadAccount(userAccount);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateleadaccount")]
        public IActionResult UpdateLeadAccount(UserAccountDto userAccount)
        {
            var result = _leadService.UpdateLeadAccount(userAccount);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deleteleadaccount/{adminuserid}/{leadid}")]
        public IActionResult DeleteLeadAccount(int adminuserid,int leadid)
        {
            var result = _leadService.DeleteLeadAccount(adminuserid,leadid);
            return Ok(result);
        }
        [HttpPut]
        [Route("deleteleads")]
        public IActionResult DeleteLeads(DeleteUserAccountDto deleteUserAccountDto)
        {
            var result = _leadService.DeleteLeads(deleteUserAccountDto);
            return Ok(result);
        }
        [HttpPost]
        [Route("addleadaccounts")]
        public IActionResult AddLeadAccounts(IFormFile bulkInsertFile)
        {
            var result = _leadService.AddLeadAccounts(bulkInsertFile);
            return Ok(result);
        }
        [HttpGet]
        [Route("getleadsummary")]
        public IActionResult GetLeadSummary()
        {
            var result = _leadService.GetLeadSummary();
            return Ok(result);
        }
        [HttpPost]
        [Route("addleadnotes")]
        public IActionResult AddLeadNotes([FromBody] UserNotesDto notes)
        {
            var result = _leadService.AddLeadNotes(notes);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateleadnotes")]
        public IActionResult UpdateLeadNotes([FromBody] UserNotesDto notes)
        {
            var result = _leadService.UpdateLeadNotes(notes);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deleteleadnotes/{adminuserid}/{leadid}")]
        public IActionResult DeleteLeadNotes(int adminuserid, int leadid)
        {
            var result = _leadService.DeleteLeadNotes(adminuserid, leadid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getleadnotes/{leadid}")]
        public IActionResult GetLeadNotes(int leadid)
        {
            var result = _leadService.GetLeadNotes(leadid);
            return Ok(result);
        }
        [HttpGet]
        [Route("resendinvites/{adminuserid}")]
        public async Task<IActionResult> ResendInvites(int adminuserid)
        {
            var result = await _leadService.ResendInvites(adminuserid);
            return Ok(result);
        }
        [HttpPut]
        [Route("resendinvitemultipleleads")]
        public async Task<IActionResult> ResendInviteMultipleLeads([FromBody] ResendInviteDto resendInviteDto)
        {
            var result = await _leadService.ResendInviteMultipleLeads(resendInviteDto);
            return Ok(result);
        }
        [HttpGet]
        [Route("resendinvite/{adminuserid}/{leadid}")]
        public async Task<IActionResult> ResendInvite(int adminuserid,int leadid)
        {
            var result = await _leadService.ResendInvite(adminuserid,leadid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getleadtags/{adminuserid}")]
        public IActionResult GetLeadTags(int adminuserid)
        {
            var result = _leadService.GetLeadTags(adminuserid);
            return Ok(result);
        }
        [HttpPost]
        [Route("addleadtags")]
        public IActionResult AddLeadTags(TagDto tag)
        {
            var result = _leadService.AddLeadTags(tag);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateleadtags")]
        public IActionResult UpdateLeadTags(TagDto tag)
        {
            var result = _leadService.UpdateLeadTags(tag);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deleteleadtags/{adminuserid}/{tagid}")]
        public IActionResult DeleteLeadTags(int adminuserid, int tagid)
        {
            var result = _leadService.DeleteLeadTags(adminuserid, tagid);
            return Ok(result);
        }
    }
}
