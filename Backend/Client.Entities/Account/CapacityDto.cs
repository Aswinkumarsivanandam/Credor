using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class CapacityDto
    {
        public int Id { get; set; }
        public string CapacityRange { get; set; }
        public bool Active { get; set; }
    }
}
