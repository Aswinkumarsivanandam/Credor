using System;
using System.Collections.Generic;
using System.Text;
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
            public string EmailId { get; set; }
            public string Title { get; set; }
            public string SecondaryEmail { get; set; }
            public bool? ReceiveEmailNotifications { get; set; }
            public string PhoneNumber { get; set; }
            public string DateOfBirth { get; set; }
	        public string ProfileImageUrl { get; set; }
            public int Residency { get; set; }
            public string Country { get; set; }
            public string Password { get; set; }
            public byte[] PasswordSalt { get; set; }
            public DateTime? PasswordChangedOn { get; set; }
            public string OldPassword { get; set; }
            [ForeignKey("Capacity")]
            public int? Capacity { get; set; }
            public bool? IsAccreditedInvestor { get; set; }
            public string HeardFrom { get; set; }
            public bool IsTOCApproved { get; set; }
            public int Status { get; set; }
            public bool Active { get; set; }
            public bool IsEmailVerified { get; set; }
            public bool IsPhoneVerified { get; set; }
            public bool IsTwoFactorAuthEnabled { get; set; }
            public string OneTimePassword { get; set; }
            public DateTime? LastLogin { get; set; }
            public DateTime CreatedOn { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ApprovedOn { get; set; }
            public string ApprovedBy { get; set; }
            public bool? VerifyAccount { get; set; }           
            public bool? CompanyNewsLetterUpdates { get; set; }
            public bool? NewInvestmentAnnouncements { get; set; }           
        }
    }

