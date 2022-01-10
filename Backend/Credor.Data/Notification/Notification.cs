using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Data.Entities
{
    public class Notifications
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }
        public int? Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

    }
}
