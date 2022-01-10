using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Credor.Data.Entities;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API.Common
{
    public partial class ApplicationDbContext : DbContext
    {

        private readonly IConfiguration _configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public virtual DbSet<UserAccount> UserAccount { get; set; }
        public virtual DbSet<PortfolioOffering> PortfolioOffering { get; set; }        
        public virtual DbSet<PortfolioFund> PortfolioFund { get; set; }
        public virtual DbSet<PortfolioFundingInstructions> PortfolioFundingInstructions { get; set; }
        public virtual DbSet<PortfolioLocation> PortfolioLocation { get; set; } 
        public virtual DbSet<PortfolioKeyHighlight> PortfolioKeyHighlight { get; set; }
        public virtual DbSet<PortfolioSummary> PortfolioSummary { get; set; }
        public virtual DbSet<PortfolioGallery> PortfolioGallery { get; set; }
        public virtual DbSet<KeyHighlight> KeyHighlight { get; set; }
        public virtual DbSet<OfferingVisibility> OfferingVisibility { get; set; }
        public virtual DbSet<CredorEmailTemplate> CredorEmailTemplate { get; set; }        
        public virtual DbSet<Investment> Investment { get; set; }
        public virtual DbSet<DistributionType> DistributionType { get; set; }
        public virtual DbSet<UserProfileType> UserProfileType { get; set; }
        public virtual DbSet<StateOrProvince> StateOrProvince { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<Investor> Investor { get; set; }
        public virtual DbSet<BankAccountType> BankAccountType { get; set; }
        public virtual DbSet<BankAccount> BankAccount { get; set; }
        public virtual DbSet<PortfolioUpdates> PortfolioUpdates { get; set; }
        public virtual DbSet<InvestmentStatus> InvestmentStatus { get; set; }
        public virtual DbSet<Capacity> Capacity { get; set; }
        public virtual DbSet<DocumentTypes> DocumentTypes { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<DocumentStatus> DocumentStatus { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<UserNotes> UserNotes { get; set; }
        public virtual DbSet<UserOTP> UserOTP { get; set; }
        public virtual DbSet<RoleFeatureMapping> RoleFeatureMapping { get; set; }
        public virtual DbSet<UserFeatureMapping> UserFeatureMapping { get; set; }
        public virtual DbSet<InvestmentSummary> InvestmentSummary { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<TagDetail> TagDetail { get; set; }
        public virtual DbSet<DocumentNameDelimiter> DocumentNameDelimiter { get; set; }
        public virtual DbSet<DocumentNamePosition> DocumentNamePosition { get; set; }
        public virtual DbSet<DocumentNameSeparator> DocumentNameSeparator { get; set; }
        public virtual DbSet<DocumentBatchDetail> DocumentBatchDetail { get; set; }
        public virtual DbSet<OfferingDistribution> OfferingDistribution { get; set; }
        public virtual DbSet<PortfolioDistributionType> PortfolioDistributionType { get; set; }
        public virtual DbSet<Distributions> Distributions { get; set; }
        public virtual DbSet<OfferingCapTable> OfferingCapTable { get; set; }
        public virtual DbSet<PortfolioOfferingVisibility> PortfolioOfferingVisibility { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbConnectionString = _configuration["ConnectionStrings:Database"];
            //optionsBuilder.UseSqlServer(@"Server=kb2data.database.windows.net,1433;Database=Kuba_Staging;User ID=Kb2dbuser;Password=75jfYW_c;Connection Timeout=3600");
            optionsBuilder.UseSqlServer(dbConnectionString);
        }

    }
}
