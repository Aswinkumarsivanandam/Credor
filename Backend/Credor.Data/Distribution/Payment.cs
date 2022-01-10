using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Credor.Data.Entities
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Investment")]
        public int InvestmentId { get; set; }
        public decimal Amount { get; set; } 
        [ForeignKey("PaymentType")]
        public int Type { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
