using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
   public class InvestorDto
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
        public bool IsNotificationEnabled { get; set; }
        public bool IsOwner { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
