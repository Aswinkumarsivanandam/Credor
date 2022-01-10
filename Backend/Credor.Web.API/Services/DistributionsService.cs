using Credor.Web.API.Common.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class DistributionsService : IDistributionsService
    {
        private readonly IDistributionsRepository _distributionsRepository;

        private readonly UnitOfWork _unitOfWork;
        public DistributionsService(IDistributionsRepository distributionsRepository,
                                     IConfiguration configuration)
        {
            _distributionsRepository = distributionsRepository;
            _unitOfWork = new UnitOfWork(configuration);
        }
        public List<DistributionsDto> GetAllDistributions(int userId)
        {
            return 
               _distributionsRepository.GetAllDistributions(userId);
        }
    }
}
