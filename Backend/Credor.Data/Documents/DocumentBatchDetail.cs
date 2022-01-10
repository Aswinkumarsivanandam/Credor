using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Credor.Data.Entities
{
    public class DocumentBatchDetail
    {
        [Key]
        public int Id { get; set; }
        public string BatchName { get; set; }
        public int TotalDocuments { get; set; }
        [ForeignKey("DocumentTypes")]
        public int DocumentType { get; set; }
        [ForeignKey("DocumentNameDelimiter")]
        public int NameDelimiter { get; set; }
        [ForeignKey("DocumentNamePosition")]
        public int NamePosition { get; set; }
        [ForeignKey("DocumentNameSeperator")]
        public int NameSeparator { get; set; }
        [ForeignKey("DocumentStatus")]
        public int Status { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
