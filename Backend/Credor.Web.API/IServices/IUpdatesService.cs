using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public interface IUpdatesService
    {
        public List<PortfolioUpdatesDto> GetAllPortfolioUpdates(int userid);
    }
}
