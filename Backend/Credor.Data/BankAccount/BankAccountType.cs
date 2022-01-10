using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Credor.Data.Entities
{
    public class BankAccountType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
    }
}
