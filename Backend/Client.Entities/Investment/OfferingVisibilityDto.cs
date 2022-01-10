using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class OfferingVisibilityDto
    {        
        public int Id { get; set; }
        public string AccessTo { get; set; }
        public bool Active { get; set; }
    }   
}
