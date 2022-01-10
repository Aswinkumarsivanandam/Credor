using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class PortfolioKeyHighlightDto
    {       
        public int Id { get; set; }        
        public int OfferingId { get; set; }
        public int? KeyHighLightId { get; set; }
        public string Name { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }
        public bool Visibility { get; set; }
    }
    public class UpdatePortfolioKeyHightlightDto
    {
        public int AdminUserId { get; set; }
        public int OfferingId { get; set; }
        public bool Status { get; set; }
        public List<PortfolioKeyHighlightDto> PortfolioKeyHighlights { get; set; }
    }
}
