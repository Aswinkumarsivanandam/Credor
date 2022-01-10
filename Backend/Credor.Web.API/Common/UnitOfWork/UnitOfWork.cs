using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Web.API.Common;
using Credor.Web.API.Common.GenericRepository;
using Credor.Data.Entities;
using System.Data.Common;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API.Common.UnitOfWork
{
    public partial class UnitOfWork : IDisposable
    {
        private readonly ApplicationDbContext _context;        
        private GenericRepository<UserAccount> _userAccountRepository;
        private GenericRepository<PortfolioOffering> _portfolioOfferingRepository;
        private GenericRepository<OfferingVisibility> _offeringVisibilityRepository;
        private GenericRepository<CredorEmailTemplate> _credorEmailTemplateRepository;
        private GenericRepository<Investment> _investmentRepository;
        private GenericRepository<UserProfileType> _userProfileTypeRepository;
        private GenericRepository<DistributionType> _distributionTypeRepository;
        private GenericRepository<StateOrProvince> _stateOrProvinceRepository;
        private GenericRepository<UserProfile> _userProfileRepository;
        private GenericRepository<Investor> _investorRepository;
        private GenericRepository<BankAccountType> _bankAccountTypeRepository;
        private GenericRepository<BankAccount> _bankAccountRepository;
        private GenericRepository<PortfolioUpdates> _portfolioUpdatesRepository;
        private GenericRepository<PortfolioGallery> _portfolioGalleryRepository;
        private GenericRepository<PortfolioSummary> _portfolioSummaryRepository;
        private GenericRepository<PortfolioLocation> _portfolioLocationRepository;
        private GenericRepository<KeyHighlight> _keyHighlightRepository;
        private GenericRepository<PortfolioKeyHighlight> _portfolioKeyHighlightRepository;
        private GenericRepository<PortfolioFundingInstructions> _portfolioFundingInstructionsReposoitory;
        private GenericRepository<InvestmentStatus> _investmentStatusRepository;
        private GenericRepository<Capacity> _capacityRepository;
        private GenericRepository<DocumentTypes> _documentsTypesRepository;
        private GenericRepository<Payment> _paymentRepository;
        private GenericRepository<Document> _documentRepository;
        private GenericRepository<DocumentStatus> _documentStatusRepository;
        private GenericRepository<UserPermission> _userPermissionRepository;
        private GenericRepository<Notifications> _notificationRepository;
        private GenericRepository<UserNotes> _userNotesRepository;
        private GenericRepository<UserOTP> _userOTPRepository;
        private GenericRepository<UserFeatureMapping> _userFeatureMappingRepository;
        private GenericRepository<RoleFeatureMapping> _roleFeatureMappingRepository;
        private GenericRepository<Tag> _tagRepository;
        private GenericRepository<TagDetail> _tagDetailRepository;
        private GenericRepository<DocumentBatchDetail> _documentBatchDetailRepository;
        private GenericRepository<DocumentNameDelimiter> _documentNameDelimiterRepository;
        private GenericRepository<DocumentNamePosition> _documentNamePositionRepository;
        private GenericRepository<DocumentNameSeparator> _documentNameSeparatorRepository;
        private GenericRepository<OfferingDistribution> _offeringDistributionRepository;
        private GenericRepository<PortfolioDistributionType> _portfolioDistributionTypeRepository;
        private GenericRepository<Distributions> _distributionsRepository;
        private GenericRepository<OfferingCapTable> _offeringCapTableRepository;
        public UnitOfWork(IConfiguration configuration)
        {           
            _context = new ApplicationDbContext(configuration);
        }       

        public GenericRepository<UserAccount> UserAccountRepository
        {
            get
            {
                if (this._userAccountRepository == null)
                    this._userAccountRepository = new GenericRepository<UserAccount>(_context);
                return _userAccountRepository;
            }
        }
        public GenericRepository<PortfolioOffering> PortfolioOfferingRepository
        {
            get
            {
                if (this._portfolioOfferingRepository == null)
                    this._portfolioOfferingRepository = new GenericRepository<PortfolioOffering>(_context);
                return _portfolioOfferingRepository;
            }
        }
        public GenericRepository<PortfolioGallery> PortfolioGalleryRepository
        {
            get
            {
                if (this._portfolioGalleryRepository == null)
                    this._portfolioGalleryRepository = new GenericRepository<PortfolioGallery>(_context);
                return _portfolioGalleryRepository;
            }
        }
        public GenericRepository<PortfolioSummary> PortfolioSummaryRepository
        {
            get
            {
                if (this._portfolioSummaryRepository == null)
                    this._portfolioSummaryRepository = new GenericRepository<PortfolioSummary>(_context);
                return _portfolioSummaryRepository;
            }
        }
        public GenericRepository<PortfolioLocation> PortfolioLocationRepository
        {
            get
            {
                if (this._portfolioLocationRepository == null)
                    this._portfolioLocationRepository = new GenericRepository<PortfolioLocation>(_context);
                return _portfolioLocationRepository;
            }
        }
        public GenericRepository<KeyHighlight> KeyHighlightRepository
        {
            get
            {
                if (this._keyHighlightRepository == null)
                    this._keyHighlightRepository = new GenericRepository<KeyHighlight>(_context);
                return _keyHighlightRepository;
            }
        }
        public GenericRepository<PortfolioKeyHighlight> PortfolioKeyHighlightRepository
        {
            get
            {
                if (this._portfolioKeyHighlightRepository == null)
                    this._portfolioKeyHighlightRepository = new GenericRepository<PortfolioKeyHighlight>(_context);
                return _portfolioKeyHighlightRepository;
            }
        }
        public GenericRepository<PortfolioFundingInstructions> PortfolioFundingInstructionsRepository
        {
            get
            {
                if (this._portfolioFundingInstructionsReposoitory == null)
                    this._portfolioFundingInstructionsReposoitory = new GenericRepository<PortfolioFundingInstructions>(_context);
                return _portfolioFundingInstructionsReposoitory;
            }
        }
        public GenericRepository<OfferingVisibility> OfferingVisibilityRepository
        {
            get
            {
                if (this._offeringVisibilityRepository == null)
                    this._offeringVisibilityRepository = new GenericRepository<OfferingVisibility>(_context);
                return _offeringVisibilityRepository;
            }
        }
        public GenericRepository<CredorEmailTemplate> CredorEmailTemplateRepository
        {
            get
            {
                if (this._credorEmailTemplateRepository == null)
                    this._credorEmailTemplateRepository = new GenericRepository<CredorEmailTemplate>(_context);
                return _credorEmailTemplateRepository;
            }
        }
        public GenericRepository<Investment> InvestmentRepository
        {
            get
            {
                if (this._investmentRepository == null)
                    this._investmentRepository = new GenericRepository<Investment>(_context);
                return _investmentRepository;
            }
        }
        public GenericRepository<UserProfileType> UserProfileTypeRepository
        {
            get
            {
                if (this._userProfileTypeRepository == null)
                    this._userProfileTypeRepository = new GenericRepository<UserProfileType>(_context);
                return _userProfileTypeRepository;
            }
        }
        public GenericRepository<DistributionType> DistributionTypeRepository
        {
            get
            {
                if (this._distributionTypeRepository == null)
                    this._distributionTypeRepository = new GenericRepository<DistributionType>(_context);
                return _distributionTypeRepository;
            }
        }
        public GenericRepository<StateOrProvince> StateOrProvinceRepository
        {
            get
            {
                if (this._stateOrProvinceRepository == null)
                    this._stateOrProvinceRepository = new GenericRepository<StateOrProvince>(_context);
                return _stateOrProvinceRepository;
            }
        }
        public GenericRepository<UserProfile> UserProfileRepository
         {
             get
             {
                 if (this._userProfileRepository == null)
                     this._userProfileRepository = new GenericRepository<UserProfile>(_context);
                 return _userProfileRepository;
             }
         }
        public GenericRepository<Investor> InvestorRepository
        {
            get
            {
                if (this._investorRepository == null)
                    this._investorRepository = new GenericRepository<Investor>(_context);
                return _investorRepository;
            }
        }
        public GenericRepository<BankAccountType> BankAccountTypeRepository
        {
            get
            {
                if (this._bankAccountTypeRepository == null)
                    this._bankAccountTypeRepository = new GenericRepository<BankAccountType>(_context);
                return _bankAccountTypeRepository;
            }
        }
        public GenericRepository<BankAccount> BankAccountRepository
        {
            get
            {
                if (this._bankAccountRepository == null)
                    this._bankAccountRepository = new GenericRepository<BankAccount>(_context);
                return _bankAccountRepository;
            }
        }
        public GenericRepository<PortfolioUpdates> PortfolioUpdatesRepository
        {
            get
            {
                if (this._portfolioUpdatesRepository == null)
                    this._portfolioUpdatesRepository = new GenericRepository<PortfolioUpdates>(_context);
                return _portfolioUpdatesRepository;
            }
        }
        public GenericRepository<InvestmentStatus> InvestmentStatusRepository
        {
            get
            {
                if (this._investmentStatusRepository == null)
                    this._investmentStatusRepository = new GenericRepository<InvestmentStatus>(_context);
                return _investmentStatusRepository;
            }
        }
        public GenericRepository<Capacity> CapacityRepository
        {
            get
            {
                if (this._capacityRepository == null)
                    this._capacityRepository = new GenericRepository<Capacity>(_context);
                return _capacityRepository;
            }
        }
        public GenericRepository<DocumentTypes> DocumentTypesRepository
        {
            get
            {
                if (this._documentsTypesRepository == null)
                    this._documentsTypesRepository = new GenericRepository<DocumentTypes>(_context);
                return _documentsTypesRepository;
            }
        }
        public GenericRepository<Payment> PaymentRepository
        {
            get
            {
                if (this._paymentRepository == null)
                    this._paymentRepository = new GenericRepository<Payment>(_context);
                return _paymentRepository;
            }
        }
        public GenericRepository<Document> DocumentRepository
        {
            get
            {
                if (this._documentRepository == null)
                    this._documentRepository = new GenericRepository<Document>(_context);
                return _documentRepository;
            }
        }
        public GenericRepository<DocumentStatus> DocumentStatusRepository
        {
            get
            {
                if (this._documentStatusRepository == null)
                    this._documentStatusRepository = new GenericRepository<DocumentStatus>(_context);
                return _documentStatusRepository;
            }
        }
        public GenericRepository<UserPermission> UserPermissionRepository
        {
            get
            {
                if (this._userPermissionRepository == null)
                    this._userPermissionRepository = new GenericRepository<UserPermission>(_context);
                return _userPermissionRepository;
            }
        }
        public GenericRepository<Notifications> NotificationRepository
        {
            get
            {
                if (this._notificationRepository == null)
                    this._notificationRepository = new GenericRepository<Notifications>(_context);
                return _notificationRepository;
            }
        }
        public GenericRepository<UserNotes> UserNotesRepository
        {
            get
            {
                if (this._userNotesRepository == null)
                    this._userNotesRepository = new GenericRepository<UserNotes>(_context);
                return _userNotesRepository;
            }
        }
        public GenericRepository<UserOTP> UserOTPRepository
        {
            get
            {
                if (this._userOTPRepository == null)
                    this._userOTPRepository = new GenericRepository<UserOTP>(_context);
                return _userOTPRepository;
            }
        }
        public GenericRepository<RoleFeatureMapping> RoleFeatureMappingRepository
        {
            get
            {
                if (this._roleFeatureMappingRepository == null)
                    this._roleFeatureMappingRepository = new GenericRepository<RoleFeatureMapping>(_context);
                return _roleFeatureMappingRepository;
            }
        }
        public GenericRepository<UserFeatureMapping> UserFeatureMappingRepository
        {
            get
            {
                if (this._userFeatureMappingRepository == null)
                    this._userFeatureMappingRepository = new GenericRepository<UserFeatureMapping>(_context);
                return _userFeatureMappingRepository;
            }
        }
        public GenericRepository<Tag> TagRepository
        {
            get
            {
                if (this._tagRepository == null)
                    this._tagRepository = new GenericRepository<Tag>(_context);
                return _tagRepository;
            }
        }
        public GenericRepository<TagDetail> TagDetailRepository
        {
            get
            {
                if (this._tagDetailRepository == null)
                    this._tagDetailRepository = new GenericRepository<TagDetail>(_context);
                return _tagDetailRepository;
            }
        }
        public GenericRepository<DocumentBatchDetail> DocumentBatchDetailRepository
        {
            get
            {
                if (this._documentBatchDetailRepository == null)
                    this._documentBatchDetailRepository = new GenericRepository<DocumentBatchDetail>(_context);
                return _documentBatchDetailRepository;
            }
        }
        public GenericRepository<DocumentNameDelimiter> DocumentNameDelimiterRepository
        {
            get
            {
                if (this._documentNameDelimiterRepository == null)
                    this._documentNameDelimiterRepository = new GenericRepository<DocumentNameDelimiter>(_context);
                return _documentNameDelimiterRepository;
            }
        }
        public GenericRepository<DocumentNamePosition> DocumentNamePositionRepository
        {
            get
            {
                if (this._documentNamePositionRepository == null)
                    this._documentNamePositionRepository = new GenericRepository<DocumentNamePosition>(_context);
                return _documentNamePositionRepository;
            }
        }
        public GenericRepository<DocumentNameSeparator> DocumentNameSeparatorRepository
        {
            get
            {
                if (this._documentNameSeparatorRepository == null)
                    this._documentNameSeparatorRepository = new GenericRepository<DocumentNameSeparator>(_context);
                return _documentNameSeparatorRepository;
            }
        }
        public GenericRepository<OfferingDistribution> OfferingDistributionRepository
        {
            get
            {
                if (this._offeringDistributionRepository == null)
                    this._offeringDistributionRepository = new GenericRepository<OfferingDistribution>(_context);
                return _offeringDistributionRepository;
            }
        }
        public GenericRepository<PortfolioDistributionType> PortfolioDistributionTypeRepository
        {
            get
            {
                if (this._portfolioDistributionTypeRepository == null)
                    this._portfolioDistributionTypeRepository = new GenericRepository<PortfolioDistributionType>(_context);
                return _portfolioDistributionTypeRepository;
            }
        }
        public GenericRepository<Distributions> DistributionsRepository
        {
            get
            {
                if (this._distributionsRepository == null)
                    this._distributionsRepository = new GenericRepository<Distributions>(_context);
                return _distributionsRepository;
            }
        }
        public GenericRepository<OfferingCapTable> OfferingCapTableRepository
        {
            get
            {
                if (this._offeringCapTableRepository == null)
                    this._offeringCapTableRepository = new GenericRepository<OfferingCapTable>(_context);
                return _offeringCapTableRepository;
            }
        }

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbException e)
            {
                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;        
        #endregion
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _userAccountRepository = null;

                }
            }
            this.disposed = true;
        }
        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
