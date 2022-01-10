using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class PortfolioGalleryDto
    {
        public int Id { get; set; }    
        public int AdminUserId { get; set; }
        public int OfferingId { get; set; }
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public bool IsDefaultImage { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class AddPortfolioGalleryDto
    {       
        public int AdminUserId { get; set; }
        public int OfferingId { get; set; }        
        public IFormFileCollection Images { get; set; }        
    }
    public class AddPortfolioGalleryResultDto
    {       
        public int OfferingId { get; set; }
        public bool Status { get; set; }
        public IList<PortfolioGalleryDto> Gallery { get; set; }
    }
    public class UpdatePortfolioGalleryResultDto
    {
        public int OfferingId { get; set; }
        public bool Status { get; set; }
        public IList<PortfolioGalleryDto> Gallery { get; set; }
    }
}
