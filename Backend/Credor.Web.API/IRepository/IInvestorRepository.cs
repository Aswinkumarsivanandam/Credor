using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public interface IInvestorRepository
    {
        UserAccountDto GetUserAccount(int investorId);
        List<UserAccountDto> GetAllInvestorAccounts();
        int UpdateInvestorAccount(UserAccountDto userAccountDto);
        bool AddInvestorNotes(UserNotesDto notesDto);
        bool UpdateInvestorNotes(UserNotesDto notesDto);
        bool DeleteInvestorNotes(int adminuserId, int investorId);
        List<UserNotesDto> GetInvestorNotes(int investorId);
        bool AddInvestorTags(TagDto tag);
        bool UpdateInvestorTags(TagDto tag);
        bool DeleteInvestorTags(int adminUserId, int tagId);
        List<TagDto> GetInvestorTags(int adminUserId);
        bool ResetPassword(ResetPasswordDto passwordDto);
        bool AccountVerification(int adminUserId, int investorId,bool isverify);        
        bool UpdateAccreditation(int adminUserId, int investorId, bool isverify);        
        Task<bool> UploadDocuments(DocumentModelDto documents);
        InvestorSummaryDto GetHeaderElements(int userId);
        InvestorsSummaryDto GetInvestorsHeaderElements();
        List<ReservationDataDto> GetReservations(int investorId);
        List<InvestmentDataDto> GetInvestments(int investorId);
        List<UserProfileDto> GetAllUserProfile(int userId);
        bool AddReservation(ReservationDataDto reservationDataDto);
        bool UpdateReservation(ReservationDataDto reservationDataDto);
        bool DeleteReservation(int adminUserId, int reservationId);
        bool AddInvestment(InvestmentDataDto investmentDataDto);
        bool UpdateInvestment(InvestmentDataDto investmentDataDto);
        bool DeleteInvestment(int adminUserId, int reservationId);
        AccountStatementDto GetAccountStatement(int investorid);
        List<PortfolioOfferingDto> GetReservationList();
        List<PortfolioOfferingDto> GetOfferingList();
    }
}

