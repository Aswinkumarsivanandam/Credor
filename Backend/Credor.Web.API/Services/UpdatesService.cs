using Credor.Web.API.Common.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class UpdatesService :IUpdatesService
    {
        private readonly IUpdatesRepository _updatesRespository;
        private readonly UnitOfWork _unitOfWork;
        public UpdatesService(IUpdatesRepository updatesRespository,
                             IConfiguration configuration)
        {
            _updatesRespository = updatesRespository;
            _unitOfWork = new UnitOfWork(configuration);
        }
        public List<PortfolioUpdatesDto> GetAllPortfolioUpdates(int userid)
        {
            return _updatesRespository.GetAllPortfolioUpdates(userid);
        }
    }
}
