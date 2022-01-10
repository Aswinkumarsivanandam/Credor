using AutoMapper;
using Credor.Web.API.Common.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class UpdatesRepository : IUpdatesRepository
    {
        private readonly UnitOfWork _unitOfWork;       
        private readonly IMapper _mapper;
        public UpdatesRepository(IMapper mapper,
                                 IConfiguration configuration)
        {
            _unitOfWork = new UnitOfWork(configuration);
            _mapper = mapper;
        }
        public List<PortfolioUpdatesDto> GetAllPortfolioUpdates(int userid)
        {
            List<PortfolioUpdatesDto> updates = new List<PortfolioUpdatesDto>();
            var contextData = _unitOfWork.PortfolioUpdatesRepository.Context;
            try
            {
                updates = (from update in contextData.PortfolioUpdates
                           join investment in contextData.Investment on update.InvestmentId equals investment.Id
                           join offering in contextData.PortfolioOffering on investment.OfferingId equals offering.Id
                           where investment.UserId == userid
                                select new PortfolioUpdatesDto
                                {
                                    Id = update.Id, 
                                    OfferingId = investment.OfferingId,
                                    Name = offering.Name,
                                    Subject = update.Subject,
                                    Content = update.Content,
                                    ImageUrl = update.ImageUrl,
                                    Active = update.Active,
                                    Status = update.Status,
                                    CreatedOn = update.CreatedOn,
                                    CreatedBy = update.CreatedBy,
                                    ModifiedOn = update.ModifiedOn,
                                    ModifiedBy = update.ModifiedBy
                                }).ToList();
            }
            catch(Exception e)
            {
                var exception = e.ToString();
                updates = null;
            }
            finally
            {
                contextData = null;
            }
            return updates;
        }
    }
}
