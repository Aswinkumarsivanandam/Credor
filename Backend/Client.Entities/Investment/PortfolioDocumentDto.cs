using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class PortfolioDocumentDto
    {       
        public int Id { get; set; }       
        public int? OfferingId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int DocumentType { get; set; }
        public string FilePath { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
