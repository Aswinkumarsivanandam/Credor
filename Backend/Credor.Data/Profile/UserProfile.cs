using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Credor.Data.Entities
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string DisplayId { get; set; }
        [ForeignKey("UserAccount")]
        public int UserId { get; set; }
        [ForeignKey("UserProfileType")]
        public int Type { get; set; }
        [ForeignKey("BankAccount")]
        public int? BankAccountId { get; set; }
        public bool IsOwner { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TrustName { get; set; }
        public string RetirementPlanName { get; set; }
        public string SignorName { get; set; }
        public string InCareOf { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        [ForeignKey("StateOrProvince")]
        public int StateOrProvinceId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string TaxId { get; set; }
        [ForeignKey("DistributionType")]
        public int? DistributionTypeId { get; set; }
        public bool? IsDisregardedEntity { get; set; }
        public bool? IsIRALLC { get; set; }
        public string OwnerTaxId { get; set; }
        public string DistributionDetail { get; set; }
        public string CheckInCareOf { get; set; }
        public string CheckAddressLine1 { get; set; }
        public string CheckAddressLine2 { get; set; }
        public string CheckCity { get;set; }
	    public int CheckStateId { get; set; }
	    public string CheckZip { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public IList<Investor> Investors { get; } = new List<Investor>();
    }
}
