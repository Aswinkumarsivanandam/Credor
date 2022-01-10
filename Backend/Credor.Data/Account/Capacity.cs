using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credor.Data.Entities
{
    public class Capacity
    {
        [Key]
        public int Id { get; set; }
        public string CapacityRange { get; set; }
        public bool Active { get; set; }
    }
}
