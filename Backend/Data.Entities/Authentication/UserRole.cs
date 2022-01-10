using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credor.Data.Entities.Authentication
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }              
        public string Name { get; set; }
        public string Description { get; set; }
        
    }
}
