using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Data.Entities
{
    public class UserOTP
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OTP { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
