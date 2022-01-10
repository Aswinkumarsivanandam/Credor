using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Microsoft.AspNetCore.Http;

namespace Credor.Web.API
{
    public interface ILeadService
    {
        UserAccountDto GetUserAccount(int leadId);
        List<UserAccountDto> GetAllLeadAccounts();
        int DeleteLeadAccount(int adminuserId, int leadId);
        int DeleteLeads(DeleteUserAccountDto deleteUserAccountDto);
        int UpdateLeadAccount(UserAccountDto userAccountDto);
        bool AddLeadAccount(UserAccountDto userAccountDto);
        List<UserAccountDto> AddLeadAccounts(IFormFile bulkInsertFile);
        LeadSummary GetLeadSummary();
        bool AddLeadNotes(UserNotesDto notesDto);
        bool UpdateLeadNotes(UserNotesDto notesDto);
        bool DeleteLeadNotes(int adminuserId, int leadid);
        List<UserNotesDto> GetLeadNotes(int leadid);
        Task<bool> ResendInvites(int adminuserid);
        Task<bool> ResendInvite(int adminuserid,int leadid);
        Task<bool> ResendInviteMultipleLeads(ResendInviteDto resendInviteDto);
        bool AddLeadTags(TagDto tag);
        bool UpdateLeadTags(TagDto tag);
        bool DeleteLeadTags(int adminUserId, int tagId);
        List<TagDto> GetLeadTags(int adminUserId);
    }
}
