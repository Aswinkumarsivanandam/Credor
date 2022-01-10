using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class RoleFeatureMappingDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FeatureName { get; set; }
        public bool Active { get; set; }
    }
    public class UserFeatureMappingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FeatureName { get; set; }
        public bool Active { get; set; }
    }
}