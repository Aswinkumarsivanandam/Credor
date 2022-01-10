using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Credor.Data.Entities
{
    public class PortfolioUpdates
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Investment")]        
        public int InvestmentId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
