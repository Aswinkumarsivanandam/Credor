using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Web.API.Common.UnitOfWork;
using AutoMapper;
using Credor.Client.Entities;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class DistributionsRepository : IDistributionsRepository
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DistributionsRepository(IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = new UnitOfWork(configuration);
            _mapper = mapper;
        }       
        public List<DistributionsDto> GetAllDistributions(int userId)
        {
            List<DistributionsDto> distributions = new List<DistributionsDto>();
            var contextData = _unitOfWork.PaymentRepository.Context;
            try
            {                
                distributions = (from payment in contextData.Payment 
                                 join investment in contextData.Investment on payment.InvestmentId equals investment.Id
                                 join userAccount in contextData.UserAccount on investment.UserId equals userAccount.Id
                                 join userProfile in contextData.UserProfile on investment.UserProfileId equals userProfile.Id
                                 join offering in contextData.PortfolioOffering on investment.OfferingId equals offering.Id
                                 join distributionType in contextData.DistributionType on userProfile.DistributionTypeId equals distributionType.Id
                                 join profileType in contextData.UserProfileType on userProfile.Type equals profileType.Id
                                 where investment.UserId == userId 
                                        && payment.Type == 1 //Credit
                                      select new DistributionsDto
                                      {
                                          Id = investment.Id,
                                          UserId = investment.UserId,
                                          UserProfileId = investment.UserProfileId,
                                          OfferingId = investment.OfferingId,
                                          PaymentId = payment.Id,
                                          OfferingName = offering.Name,                                         
                                          AmountInvested = investment.Amount,
                                          AmountReceived = payment.Amount,
                                          ReceivedDate = payment.CreatedOn,
                                          Type = "Retained Earnings",
                                          Memo = payment.Comment,
                                          DistributionMethod = distributionType.Name,
                                          UserProfileType = profileType.Name,
                                          UserProfile = (from profile in contextData.UserProfile                                                         
                                                         where profile.Id == investment.UserProfileId
                                                         select new UserProfileDto
                                                         {
                                                             Id = profile.Id,
                                                             Type = profile.Type,                                                            
                                                             Name = profile.Name,
                                                             FirstName = profile.FirstName,
                                                             LastName = profile.LastName,
                                                             RetirementPlanName = profile.RetirementPlanName,
                                                             TrustName = profile.TrustName
                                                         }).FirstOrDefault()

                                      }
                                      ).ToList();
            }
            catch(Exception e)
            {
                e.ToString();
                return distributions;
            }
            finally
            {
                contextData = null;                
            }
            return distributions;
        }
    }
}
