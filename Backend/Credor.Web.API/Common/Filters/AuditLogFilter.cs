using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using Credor.Web.API.Common.Audit;

namespace Credor.Web.API.Common.Filters
{
    public class AuditLogFilter: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string controller = context.Controller.ToString();
            string Action = context.ActionDescriptor.DisplayName.ToString();

            //AuditConfiguration.AddAudit(controller+Action);
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
