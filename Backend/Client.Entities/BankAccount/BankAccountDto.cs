using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class BankAccountDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string BankName { get; set; }
        public int AccountType { get; set; }
        public string AccountTypeName { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
    }
}
