using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class UserPermissionDto
    {       
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string EmailId { get; set; }
        public string Title { get; set; }
        public int RoleId { get; set; }
        public int Permission { get; set; }
        public bool IsNotificationEnabled { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
