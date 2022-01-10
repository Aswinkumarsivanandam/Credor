using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Credor.Data.Entities
{
   public class PortfolioKeyHighlight
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("PortfolioOffering")]
        public int OfferingId { get; set; }
        public int? KeyHighlightId { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }
        public bool Visibility { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
