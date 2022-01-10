using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Core.ConfigurationApi;

namespace Credor.Web.API.Common.Audit
{
    public static class AuditConfiguration
    {
        
       private static string auditdata { get; set; }

        public static void AddAudit(string requestData)
        {
          //  auditdata = requestData;           
                auditdata = auditdata + requestData;            
            
        }
        public static void ConfigureAudit(IServiceCollection serviceCollection)
        {
            // This is explained below
        }
    }
}
