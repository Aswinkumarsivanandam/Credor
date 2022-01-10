using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credor.Data.Entities
{
    public class UserAccount
    {       
        [Key]
        public int Id { get; set; }        
        [ForeignKey("UserRole")]
        public int RoleId { get; set; }      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Username { get; set; }
        public string EmailID { get; set; }
        public string PhoneNumber { get; set; }
        public int Residency { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        [ForeignKey("Capacity")]
        public int Capacity { get; set; }
        public bool IsAccreditedInvestor { get; set; }
        public string HeardFrom { get; set; }
        public bool TandCAcceptance { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public int OneTimePassword { get; set; }   
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }           
    }
}
