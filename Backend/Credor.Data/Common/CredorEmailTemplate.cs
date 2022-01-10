using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Credor.Data.Entities
{
   public class CredorEmailTemplate
    {        
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public string BodyContent { get; set; }
        public int Status { get; set; }
        public string TemplateName { get; set; }        
    }
}
