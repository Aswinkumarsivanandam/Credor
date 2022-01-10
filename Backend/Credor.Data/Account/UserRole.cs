using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credor.Data.Entities
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class RoleFeatureMapping
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserRole")]
        public int RoleId { get; set; }
        public string FeatureName { get; set; }
        public bool Active { get; set; }
    }
    public class UserFeatureMapping
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserAccount")]
        public int UserId { get; set; }
        public string FeatureName { get; set; }
        public bool Active { get; set; }
    }
}
