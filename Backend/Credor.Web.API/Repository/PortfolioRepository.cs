using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Credor.Client.Entities;
using Credor.Data.Entities;
using Credor.Web.API.Common.UnitOfWork;
using Credor.Web.API.Shared;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly UnitOfWork _unitOfWork;
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public PortfolioRepository(IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = new UnitOfWork(configuration);
            _mapper = mapper;
            _configuration = configuration;
        }

        public int AddPortfolioOffering(PortfolioOfferingDto portfolioOfferingDto)
        {
            try
            {
                using (var transaction = _unitOfWork.PortfolioOfferingRepository.Context.Database.BeginTransaction())
                {

                    PortfolioOffering offering = new PortfolioOffering();
                    offering.IsReservation = false;
                    offering.Name = portfolioOfferingDto.Name;
                    offering.EntityName = portfolioOfferingDto.EntityName;
                    offering.Status = portfolioOfferingDto.Status;
                    offering.Active = true;
                    offering.Type = portfolioOfferingDto.Type;
                    offering.Visibility = portfolioOfferingDto.Visibility;
                    offering.Size = portfolioOfferingDto.Size;
                    offering.MinimumInvestment = portfolioOfferingDto.MinimumInvestment;
                    offering.CreatedOn = DateTime.Now;
                    offering.PictureUrl = "https://kuba2storage.blob.core.windows.net/credor/GalleryImages/5/download-(1).jpg"; // Need to get default image from UI team
                    offering.CreatedBy = portfolioOfferingDto.AdminUserId.ToString();

                    _unitOfWork.PortfolioOfferingRepository.Insert(offering);
                    _unitOfWork.Save();
                    transaction.Commit();
                    var offeringId = offering.Id;
                    var result = AddDefaultPortfolioKeyHighlights(offeringId,portfolioOfferingDto.AdminUserId);
                    if (result)
                        return offeringId;
                    else
                        return 0;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        public int AddPortfolioReservation(PortfolioOfferingDto portfolioReservationDto)
        {
            try
            {
                using (var transaction = _unitOfWork.PortfolioOfferingRepository.Context.Database.BeginTransaction())
                {

                    PortfolioOffering reservation = new PortfolioOffering();
                    reservation.IsReservation = true;
                    reservation.Name = portfolioReservationDto.Name;
                    reservation.EntityName = portfolioReservationDto.EntityName;
                    reservation.Status = portfolioReservationDto.Status;
                    reservation.Active = true;                   
                    reservation.Visibility = portfolioReservationDto.Visibility;
                    reservation.Size = portfolioReservationDto.Size;
                    reservation.MinimumInvestment = 0;
                    
                    reservation.PictureUrl = "https://kuba2storage.blob.core.windows.net/credor/GalleryImages/5/download-(1).jpg"; // Need to get default image from UI team
                    reservation.CreatedOn = DateTime.Now;
                    reservation.CreatedBy = portfolioReservationDto.AdminUserId.ToString();

                    _unitOfWork.PortfolioOfferingRepository.Insert(reservation);
                    _unitOfWork.Save();
                    transaction.Commit();
                    var reservationId = reservation.Id;

                    var result = AddDefaultPortfolioKeyHighlights(reservationId,portfolioReservationDto.AdminUserId);
                    if (result)
                        return reservationId;
                    else
                        return 0;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        public bool DeletePortfolioOffering(int adminUserId, int offeringId)
        {
            try
            {
                var offering = _unitOfWork.PortfolioOfferingRepository.GetByID(offeringId);
                if (offering != null)
                {
                    using (var transaction = _unitOfWork.PortfolioOfferingRepository.Context.Database.BeginTransaction())
                    {
                        PortfolioOffering offeringData = offering;
                        offeringData.Active = false;
                        offeringData.ModifiedOn = DateTime.Now;
                        offeringData.ModifiedBy = adminUserId.ToString();

                        _unitOfWork.PortfolioOfferingRepository.Update(offeringData);
                        _unitOfWork.Save();
                        transaction.Commit();

                        return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public bool DeletePortfolioReservation(int adminUserId , int reservationId)
        {
            try
            {
                var reservation = _unitOfWork.PortfolioOfferingRepository.GetByID(reservationId);
                if (reservation != null)
                {
                    using (var transaction = _unitOfWork.PortfolioOfferingRepository.Context.Database.BeginTransaction())
                    {
                        PortfolioOffering reservationData = reservation;
                        reservationData.Active = false;
                        reservationData.ModifiedOn = DateTime.Now;
                        reservationData.ModifiedBy = adminUserId.ToString();

                        _unitOfWork.PortfolioOfferingRepository.Update(reservationData);
                        _unitOfWork.Save();
                        transaction.Commit();

                        return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }      
        public PortfolioOfferingDto GetPortfolioOffering(int offeringId)
        {
            try
            {
                var contextData = _unitOfWork.PortfolioOfferingRepository.Context;

                var offeringData = (from offering in contextData.PortfolioOffering
                                    where offering.Id == offeringId && offering.IsReservation == false && offering.Active == true
                                    select new PortfolioOfferingDto
                                    {
                                        Id = offering.Id,
                                        Name = offering.Name,
                                        PictureUrl = offering.PictureUrl,
                                        EntityName = offering.EntityName,
                                        Active = offering.Active,
                                        Status = offering.Status,
                                        Size = offering.Size,
                                        IsReservation = offering.IsReservation,
                                        Visibility = offering.Visibility,
                                        CreatedOn = offering.CreatedOn,
                                        MinimumInvestment = offering.MinimumInvestment,
                                        KeyHighlights = (from PKH in contextData.PortfolioKeyHighlight
                                                         where PKH.OfferingId == offering.Id
                                                         && PKH.Active == true
                                                         join KH in contextData.KeyHighlight on PKH.KeyHighlightId equals KH.Id into keyhighlights
                                                         from KH in keyhighlights.DefaultIfEmpty()
                                                         select new PortfolioKeyHighlightDto
                                                         {
                                                             Id = PKH.Id,
                                                             OfferingId = PKH.OfferingId,
                                                             KeyHighLightId = PKH.KeyHighlightId,
                                                             Name = KH.Name,
                                                             Field = PKH.Field,
                                                             Value = PKH.Value,
                                                             Active = PKH.Active,
                                                             Visibility = PKH.Visibility
                                                         }).ToList(),
                                        Summary = (from summary in contextData.PortfolioSummary
                                                   where summary.OfferingId == offering.Id
                                                   select new PortfolioSummaryDto
                                                   {
                                                       Id = summary.Id,
                                                       OfferingId = summary.OfferingId,
                                                       Summary = summary.Summary,
                                                       Status = summary.Status,
                                                       Active = summary.Active
                                                   }).ToList(),
                                        Documents = (from document in contextData.Document
                                                     where document.OfferingId == offering.Id
                                                     select new PortfolioDocumentDto
                                                     {
                                                         Id = document.Id,
                                                         OfferingId = document.OfferingId,
                                                         Name = document.Name,
                                                         FilePath = document.FilePath,
                                                         Type = document.Extension,
                                                         Status = document.Status,
                                                         DocumentType = document.Type,
                                                         Active = document.Active,
                                                         CreatedOn = document.CreatedOn
                                                     }).ToList(),
                                        Locations = (from loc in contextData.PortfolioLocation
                                                     where loc.OfferingId == offering.Id
                                                     select new PortfolioLocationDto
                                                     {
                                                         Id = loc.Id,
                                                         OfferingId = loc.OfferingId,
                                                         Location = loc.Location,
                                                         Status = loc.Status,
                                                         Active = loc.Active,
                                                         Latitude = loc.Latitude,
                                                         Longitude = loc.Longitude
                                                     }).ToList(),
                                        Galleries = (from gallery in contextData.PortfolioGallery
                                                     where gallery.OfferingId == offering.Id
                                                     select new PortfolioGalleryDto
                                                     {
                                                         Id = gallery.Id,
                                                         OfferingId = gallery.OfferingId,
                                                         ImageUrl = gallery.ImageUrl,
                                                         Status = gallery.Status,
                                                         Active = gallery.Active,
                                                         IsDefaultImage = gallery.IsDefaultImage
                                                     }).OrderByDescending(x => x.IsDefaultImage).ToList(),
                                        Funds = (from fund in contextData.PortfolioFundingInstructions
                                                 where fund.OfferingId == offering.Id
                                                  && fund.Active == true
                                                 select new PortfolioFundingInstructionsDto
                                                 {
                                                     Id = fund.Id,
                                                     OfferingId = fund.OfferingId,
                                                     ReceivingBank = fund.ReceivingBank,
                                                     BankAddress = fund.BankAddress,
                                                     Beneficiary = fund.Beneficiary,
                                                     BeneficiaryAddress = fund.BeneficiaryAddress,
                                                     AccountType = fund.AccountType,
                                                     AccountNumber = fund.AccountNumber,
                                                     RoutingNumber = fund.RoutingNumber,
                                                     MailingAddress = fund.MailingAddress,
                                                     Reference = fund.Reference,
                                                     Memo = fund.Memo,
                                                     Custom = fund.Custom,
                                                     OtherInstructions = fund.OtherInstructions,
                                                     CheckBenificiary = fund.CheckBenificiary,
                                                     CheckOtherInstructions = fund.CheckOtherInstructions,
                                                     Status = fund.Status,
                                                     Active = fund.Active
                                                 }).FirstOrDefault()
                                    }).FirstOrDefault();
                contextData = null;
                return offeringData;

            }
            catch (Exception e)
            {
                throw e;
            }           
        }
        public List<PortfolioOfferingDto> GetPortfolioOfferings()
        {
            List<PortfolioOfferingDto> offerings = new List<PortfolioOfferingDto>();
            var contextData = _unitOfWork.PortfolioOfferingRepository.Context;
            try
            {
                offerings = (from offering in contextData.PortfolioOffering
                             where offering.IsReservation == false
                             && offering.Active == true
                             select new PortfolioOfferingDto
                            {
                               Id = offering.Id,
                               Name = offering.Name,
                               PictureUrl = offering.PictureUrl,
                               Active = offering.Active,
                               EntityName = offering.EntityName,
                               CreatedOn = offering.CreatedOn,
                               Type = offering.Type,
                               Visibility = offering.Visibility,
                               Status = offering.Status,
                               Size = offering.Size,
                               MinimumInvestment = offering.MinimumInvestment                               
                             }).OrderByDescending(x => x.CreatedOn).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
                offerings = null;
            }
            finally
            {
                contextData = null;
            }
            return offerings;
        }
        public PortfolioOfferingDto GetPortfolioReservation(int reservationId)
        {
            try
            {
                var contextData = _unitOfWork.PortfolioOfferingRepository.Context;

                var reservationData = (from reservation in contextData.PortfolioOffering
                                       where reservation.Id == reservationId && reservation.IsReservation == true && reservation.Active == true
                                       select new PortfolioOfferingDto
                                       {
                                           Id = reservation.Id,
                                           Name = reservation.Name,
                                           PictureUrl = reservation.PictureUrl,
                                           EntityName = reservation.EntityName,
                                           Active = reservation.Active,
                                           Status = reservation.Status,
                                           Size = reservation.Size,                                          
                                           IsReservation = reservation.IsReservation,
                                           Visibility = reservation.Visibility,
                                           CreatedOn = reservation.CreatedOn,                                          
                                           KeyHighlights = (from PKH in contextData.PortfolioKeyHighlight
                                                            where PKH.OfferingId == reservation.Id
                                                            && PKH.Active == true
                                                            join KH in contextData.KeyHighlight on PKH.KeyHighlightId equals KH.Id into keyhighlights
                                                            from KH in keyhighlights.DefaultIfEmpty()
                                                            select new PortfolioKeyHighlightDto
                                                            {
                                                                Id = PKH.Id,
                                                                OfferingId = PKH.OfferingId,
                                                                KeyHighLightId = PKH.KeyHighlightId,
                                                                Name = KH.Name,
                                                                Field = PKH.Field,
                                                                Value = PKH.Value,
                                                                Active = PKH.Active,
                                                                Visibility = PKH.Visibility
                                                            }).ToList(),
                                           Summary = (from summary in contextData.PortfolioSummary
                                                      where summary.OfferingId == reservation.Id
                                                      select new PortfolioSummaryDto
                                                      {
                                                          Id = summary.Id,
                                                          OfferingId = summary.OfferingId,
                                                          Summary = summary.Summary,
                                                          Status = summary.Status,
                                                          Active = summary.Active
                                                      }).ToList(),
                                           Documents = (from document in contextData.Document
                                                        join documentType in contextData.DocumentTypes on document.Type equals documentType.Id
                                                        where document.OfferingId == reservation.Id
                                                        select new PortfolioDocumentDto
                                                        {
                                                            Id = document.Id,
                                                            OfferingId = document.OfferingId,
                                                            Name = document.Name,
                                                            Type = documentType.Name,
                                                            DocumentType = document.Type,
                                                            FilePath = document.FilePath,
                                                            Status = document.Status,
                                                            Active = document.Active,
                                                            CreatedOn = document.CreatedOn
                                                        }).ToList(),
                                           Locations = (from loc in contextData.PortfolioLocation
                                                        where loc.OfferingId == reservation.Id
                                                        select new PortfolioLocationDto
                                                        {
                                                            Id = loc.Id,
                                                            OfferingId = loc.OfferingId,
                                                            Location = loc.Location,
                                                            Status = loc.Status,
                                                            Active = loc.Active,
                                                            Latitude = loc.Latitude,
                                                            Longitude = loc.Longitude
                                                        }).ToList(),
                                           Galleries = (from gallery in contextData.PortfolioGallery
                                                        where gallery.OfferingId == reservation.Id
                                                        select new PortfolioGalleryDto
                                                        {
                                                            Id = gallery.Id,
                                                            OfferingId = gallery.OfferingId,
                                                            ImageUrl = gallery.ImageUrl,
                                                            Status = gallery.Status,
                                                            Active = gallery.Active,
                                                            IsDefaultImage = gallery.IsDefaultImage
                                                        }).OrderByDescending(x => x.IsDefaultImage).ToList()                                          
                                       }).FirstOrDefault();
                contextData = null;
                return reservationData;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<PortfolioOfferingDto> GetPortfolioReservations()
        {
            List<PortfolioOfferingDto> reservations = new List<PortfolioOfferingDto>();
            var contextData = _unitOfWork.PortfolioOfferingRepository.Context;
            try
            {
                reservations = (from offering in contextData.PortfolioOffering
                             where offering.IsReservation == true
                             && offering.Active == true
                             select new PortfolioOfferingDto
                             {
                                 Id = offering.Id,
                                 Name = offering.Name,
                                 PictureUrl = offering.PictureUrl,
                                 EntityName = offering.EntityName,
                                 Active = offering.Active,
                                 CreatedOn = offering.CreatedOn,
                                 Type = offering.Type,
                                 Visibility = offering.Visibility,
                                 Status = offering.Status,
                                 Size = offering.Size                                
                             }).OrderByDescending(x => x.CreatedOn).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
                reservations = null;
            }
            finally
            {
                contextData = null;
            }
            return reservations;
        }
        public int UpdatePortfolioOffering(PortfolioOfferingDto portfolioOffering)
        {
            try
            {
                var offering = _unitOfWork.PortfolioOfferingRepository.GetByID(portfolioOffering.Id);
                if (offering != null)
                {
                    using (var transaction = _unitOfWork.PortfolioOfferingRepository.Context.Database.BeginTransaction())
                    {
                        PortfolioOffering offeringData = offering;
                        offeringData.Name = portfolioOffering.Name;
                        offeringData.EntityName = portfolioOffering.EntityName;
                        // offeringData.Active = portfolioOffering.Active;
                        if (offeringData.Size != portfolioOffering.Size)
                            offeringData.Size = portfolioOffering.Size;
                        if (offeringData.MinimumInvestment != portfolioOffering.MinimumInvestment)
                            offeringData.MinimumInvestment = portfolioOffering.MinimumInvestment;
                        offeringData.MinimumInvestment = portfolioOffering.MinimumInvestment;
                        offeringData.Status = portfolioOffering.Status;
                        offeringData.Type = portfolioOffering.Type;
                        offeringData.Visibility = portfolioOffering.Visibility;
                        offeringData.ModifiedOn = DateTime.Now;
                        offeringData.ModifiedBy = portfolioOffering.AdminUserId.ToString();

                        _unitOfWork.PortfolioOfferingRepository.Update(offeringData);
                        _unitOfWork.Save();
                        transaction.Commit();
                        var offeringId = offeringData.Id;

                        return offeringId;
                    }
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public int UpdatePortfolioReservation(PortfolioOfferingDto portfolioReservation)
        {
            try
            {
                var reservation = _unitOfWork.PortfolioOfferingRepository.GetByID(portfolioReservation.Id);
                if (reservation != null)
                {
                    using (var transaction = _unitOfWork.PortfolioOfferingRepository.Context.Database.BeginTransaction())
                    {
                        PortfolioOffering reservationData = reservation;
                        reservationData.Name = portfolioReservation.Name;
                        reservationData.EntityName = portfolioReservation.EntityName;
                       // reservationData.Active = portfolioReservation.Active;
                       if(reservation.Size != portfolioReservation.Size)
                            reservationData.Size = portfolioReservation.Size;                     
                        reservationData.Status = portfolioReservation.Status;
                        reservationData.Type = portfolioReservation.Type;
                        reservationData.Visibility = portfolioReservation.Visibility;
                        reservationData.ModifiedOn = DateTime.Now;
                        reservationData.ModifiedBy = portfolioReservation.AdminUserId.ToString();

                        _unitOfWork.PortfolioOfferingRepository.Update(reservationData);
                        _unitOfWork.Save();
                        transaction.Commit();
                        var reservationId = reservationData.Id;

                        return reservationId;
                    }
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public List<PortfolioOfferingDto> GetArchivedPortfolioOfferings()
        {
            List<PortfolioOfferingDto> offerings = new List<PortfolioOfferingDto>();
            var contextData = _unitOfWork.PortfolioOfferingRepository.Context;
            try
            {
                offerings = (from offering in contextData.PortfolioOffering
                             where offering.IsReservation == false
                             && offering.Active == false
                             select new PortfolioOfferingDto
                             {
                                 Id = offering.Id,
                                 Name = offering.Name,
                                 PictureUrl = offering.PictureUrl,
                                 Active = offering.Active,
                                 EntityName = offering.EntityName,
                                 CreatedOn = offering.CreatedOn,
                                 Type = offering.Type,
                                 IsReservation = offering.IsReservation,
                                 Visibility = offering.Visibility,
                                 Status = offering.Status,
                                 Size = offering.Size,
                                 MinimumInvestment = offering.MinimumInvestment,
                                 ModifiedOn = offering.ModifiedOn
                             }).OrderByDescending(x => x.ModifiedOn).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
                offerings = null;
            }
            finally
            {
                contextData = null;
            }
            return offerings;
        }
        public List<PortfolioOfferingDto> GetArchivedPortfolioOfferingAndReservations()
        {
            List<PortfolioOfferingDto> reservations = new List<PortfolioOfferingDto>();
            var contextData = _unitOfWork.PortfolioOfferingRepository.Context;
            try
            {
                reservations = (from reservation in contextData.PortfolioOffering
                             where reservation.Active == false
                             select new PortfolioOfferingDto
                             {
                                 Id = reservation.Id,
                                 Name = reservation.Name,
                                 PictureUrl = reservation.PictureUrl,
                                 Active = reservation.Active,
                                 EntityName = reservation.EntityName,
                                 CreatedOn = reservation.CreatedOn,
                                 Type = reservation.Type,
                                 Visibility = reservation.Visibility,
                                 Status = reservation.Status,
                                 Size = reservation.Size,
                                 ModifiedOn = reservation.ModifiedOn
                             }).OrderByDescending(x => x.ModifiedOn).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
                reservations = null;
            }
            finally
            {
                contextData = null;
            }
            return reservations;
        }
        public bool RestorePortfolioOffering(int AdminUserId , int offeringId)
        {
            try
            {
                var offering = _unitOfWork.PortfolioOfferingRepository.GetByID(offeringId);
                if (offering != null)
                {
                    using (var transaction = _unitOfWork.PortfolioOfferingRepository.Context.Database.BeginTransaction())
                    {
                        PortfolioOffering offeringData = offering;
                        
                        offeringData.Active = true;
                        
                        offeringData.ModifiedOn = DateTime.Now;
                        offeringData.ModifiedBy = AdminUserId.ToString();

                        _unitOfWork.PortfolioOfferingRepository.Update(offeringData);
                        _unitOfWork.Save();
                        transaction.Commit();

                        return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public bool RestorePortfolioReservation(int AdminUserId,int reservationId)
        {
            try
            {
                var reservation = _unitOfWork.PortfolioOfferingRepository.GetByID(reservationId);
                if (reservation != null)
                {
                    using (var transaction = _unitOfWork.PortfolioOfferingRepository.Context.Database.BeginTransaction())
                    {
                        PortfolioOffering reservationData = reservation;
                        
                        reservationData.Active = true;
                       
                        reservationData.ModifiedOn = DateTime.Now;
                        reservationData.ModifiedBy = AdminUserId.ToString();

                        _unitOfWork.PortfolioOfferingRepository.Update(reservationData);
                        _unitOfWork.Save();
                        transaction.Commit();

                        return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public async Task<AddPortfolioGalleryResultDto> AddPortfolioGalleryImages(AddPortfolioGalleryDto portfolioGalleryDto)
        {
            var contextData = _unitOfWork.PortfolioGalleryRepository.Context;
            try
            {
                List<PortfolioGallery> galleries = new List<PortfolioGallery>();
                using (var transaction = _unitOfWork.PortfolioGalleryRepository.Context.Database.BeginTransaction())
                {
                    if (portfolioGalleryDto.Images != null)
                    {
                        foreach (var image in portfolioGalleryDto.Images)
                        {
                            Helper _helper = new Helper(_configuration);
                            var blobFilePath = (await _helper.DocumentSaveAndUpload(image, portfolioGalleryDto.OfferingId, 9)).ToString();

                            PortfolioGallery gallery = new PortfolioGallery();
                            gallery.OfferingId = portfolioGalleryDto.OfferingId;
                            gallery.ImageUrl = blobFilePath;
                            gallery.Status = 1;
                            gallery.Active = true;
                            gallery.CreatedBy = portfolioGalleryDto.AdminUserId.ToString();
                            gallery.CreatedOn = DateTime.Now;
                            galleries.Add(gallery);
                        }
                    }
                    contextData.AddRange(galleries);
                    contextData.SaveChanges();
                    transaction.Commit();
                }
                    var portfolioGallery = (from gallery in contextData.PortfolioGallery
                                            where gallery.OfferingId == portfolioGalleryDto.OfferingId
                                            && gallery.Active == true
                                            select new PortfolioGalleryDto
                                            {
                                                Id = gallery.Id,
                                                OfferingId = gallery.OfferingId,
                                                ImageUrl = gallery.ImageUrl,
                                                Active = gallery.Active,
                                                Status = gallery.Status,
                                                IsDefaultImage = gallery.IsDefaultImage
                                            }).ToList();
                  

                    AddPortfolioGalleryResultDto result = new AddPortfolioGalleryResultDto();

                    result.OfferingId = portfolioGalleryDto.OfferingId;
                    result.Status = true;
                    result.Gallery = portfolioGallery;

                    if (portfolioGallery != null)
                    {
                        using (var transaction = _unitOfWork.PortfolioGalleryRepository.Context.Database.BeginTransaction())
                        {
                            var offeringContextData = _unitOfWork.PortfolioOfferingRepository.Context;
                            var DefaultImage = portfolioGallery[0];
                            var Offering = _unitOfWork.PortfolioOfferingRepository.GetByID(portfolioGalleryDto.OfferingId);
                            Offering.PictureUrl = DefaultImage.ImageUrl;
                            _unitOfWork.PortfolioOfferingRepository.Update(Offering);
                            offeringContextData.SaveChanges();
                        transaction.Commit();
                        }
                    }
                    return result;                
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure 
            }
            finally
            {
                contextData = null;
            }           
        }
        public UpdatePortfolioGalleryResultDto UpdatePortfolioGalleryImage(PortfolioGalleryDto portfolioGalleryDto)
        {
            try
            {
                var contextData = _unitOfWork.PortfolioGalleryRepository.Context;
                var gallery = _unitOfWork.PortfolioGalleryRepository.GetByID(portfolioGalleryDto.Id);
                if (gallery != null)
                {
                    using (var transaction = _unitOfWork.PortfolioGalleryRepository.Context.Database.BeginTransaction())
                    {
                        if (portfolioGalleryDto.IsDefaultImage == true)
                        {
                            List<PortfolioGallery> galleries = new List<PortfolioGallery>();
                            var galleryImages = _unitOfWork.PortfolioGalleryRepository.GetMany(x => x.OfferingId == gallery.OfferingId);
                            if (galleryImages != null)
                            {
                                foreach (var image in galleryImages)
                                {
                                    image.IsDefaultImage = false;
                                    galleries.Add(image);
                                }
                                _unitOfWork.PortfolioGalleryRepository.UpdateList(galleries);
                                contextData.SaveChanges();
                            }

                            gallery.IsDefaultImage = portfolioGalleryDto.IsDefaultImage;
                            gallery.ModifiedBy = portfolioGalleryDto.AdminUserId.ToString();
                            gallery.ModifiedOn = DateTime.Now;
                            _unitOfWork.PortfolioGalleryRepository.Update(gallery);
                            contextData.SaveChanges();
                            transaction.Commit();

                            using (var offeringTransaction = _unitOfWork.PortfolioOfferingRepository.Context.Database.BeginTransaction())
                            {
                                var offeringContextData = _unitOfWork.PortfolioOfferingRepository.Context;
                                var Offering = _unitOfWork.PortfolioOfferingRepository.GetByID(gallery.OfferingId);
                                Offering.PictureUrl = gallery.ImageUrl;
                                _unitOfWork.PortfolioOfferingRepository.Update(Offering);
                                offeringContextData.SaveChanges();
                                offeringTransaction.Commit();
                                offeringContextData = null;
                            }
                        }
                        else if (portfolioGalleryDto.Active == false)
                        {
                            gallery.IsDefaultImage = portfolioGalleryDto.IsDefaultImage;
                            gallery.Active = portfolioGalleryDto.Active;
                            gallery.ModifiedBy = portfolioGalleryDto.AdminUserId.ToString();
                            gallery.ModifiedOn = DateTime.Now;
                            _unitOfWork.PortfolioGalleryRepository.Update(gallery);
                            contextData.SaveChanges();
                            transaction.Commit();
                        }
                    }                   
                    var portfolioGallery = (from galleryImage in contextData.PortfolioGallery
                                            where galleryImage.OfferingId == gallery.OfferingId
                                            && galleryImage.Active == true
                                            select new PortfolioGalleryDto
                                            {
                                                Id = galleryImage.Id,
                                                OfferingId = galleryImage.OfferingId,
                                                ImageUrl = galleryImage.ImageUrl,
                                                Active = galleryImage.Active,
                                                Status = galleryImage.Status,
                                                IsDefaultImage = galleryImage.IsDefaultImage
                                            }).OrderByDescending(x => x.IsDefaultImage).ToList();

                    UpdatePortfolioGalleryResultDto result = new UpdatePortfolioGalleryResultDto();
                    result.OfferingId = gallery.OfferingId;
                    result.Status = true;
                    result.Gallery = portfolioGallery;

                    contextData = null;
                    return result;
                }
                return null;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure
            }            
        }
        public PortfolioSummaryDto AddPortfolioSummary(PortfolioSummaryDto portfolioSummaryDto)
        {
            var contextData = _unitOfWork.PortfolioSummaryRepository.Context;
            try
            {                
                using (var transaction = _unitOfWork.PortfolioSummaryRepository.Context.Database.BeginTransaction())
                {
                    PortfolioSummary portfolioSummary = new PortfolioSummary();
                    portfolioSummary.OfferingId = portfolioSummaryDto.OfferingId;
                    portfolioSummary.Summary = portfolioSummaryDto.Summary;
                    portfolioSummary.Status = 1;
                    portfolioSummary.Active = true;
                    portfolioSummary.CreatedBy = portfolioSummaryDto.AdminUserId.ToString();
                    portfolioSummary.CreatedOn = DateTime.Now;

                    _unitOfWork.PortfolioSummaryRepository.Insert(portfolioSummary);
                    contextData.SaveChanges();
                    transaction.Commit();
                    var portfolioSummaryId = portfolioSummary.Id;

                    var portfolioSummaryData = (from summary in contextData.PortfolioSummary
                                                where summary.OfferingId == portfolioSummaryDto.OfferingId
                                                && summary.Active == true
                                                select new PortfolioSummaryDto
                                                {
                                                    Id = summary.Id,
                                                    OfferingId = summary.OfferingId,
                                                    Summary = summary.Summary,
                                                    Status = summary.Status
                                                }).FirstOrDefault();

                    return portfolioSummaryData;
                }                
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure 
            }
            finally
            {
                contextData = null;
            }
        }
        public PortfolioSummaryDto UpdatePortfolioSummary(PortfolioSummaryDto portfolioSummaryDto)
        {
            var contextData = _unitOfWork.PortfolioSummaryRepository.Context;
            try
            {
                using (var transaction = _unitOfWork.PortfolioSummaryRepository.Context.Database.BeginTransaction())
                {
                    var portfolioSummary = _unitOfWork.PortfolioSummaryRepository.Get(x=>x.OfferingId == portfolioSummaryDto.OfferingId && x.Active == true);

                    if (portfolioSummary != null)
                    {
                        portfolioSummary.Summary = portfolioSummaryDto.Summary;
                        portfolioSummary.ModifiedBy = portfolioSummaryDto.AdminUserId.ToString();
                        portfolioSummary.ModifiedOn = DateTime.Now;

                        _unitOfWork.PortfolioSummaryRepository.Update(portfolioSummary);
                        contextData.SaveChanges();
                        transaction.Commit();

                        var portfolioSummaryData = (from summary in contextData.PortfolioSummary
                                                    where summary.OfferingId == portfolioSummaryDto.OfferingId
                                                    && summary.Active == true
                                                    select new PortfolioSummaryDto
                                                    {
                                                        Id = summary.Id,
                                                        OfferingId = summary.OfferingId,
                                                        Summary = summary.Summary,
                                                        Status = summary.Status
                                                    }).FirstOrDefault();

                        return portfolioSummaryData;
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure 
            }
            finally
            {
                contextData = null;
            }
        }
        public async Task<PortfolioDocumentsResultDto> UploadOfferingDocuments(DocumentModelDto documentDto)
        {
            var contextData = _unitOfWork.DocumentRepository.Context;

            try
            {
                int documentType;
                Helper _helper = new Helper(_configuration);
                List<Document> documents = new List<Document>();
                using (var transaction = _unitOfWork.DocumentRepository.Context.Database.BeginTransaction())
                {
                    if (documentDto.WelcomeDocuments != null)
                    {
                        documentType = 6;//Welcome Document
                        var file = documentDto.WelcomeDocuments[0];
                        var blobFilePath = (await _helper.DocumentSaveAndUpload(file, documentDto.OfferingId, documentType)).ToString();
                        Document document = new Document();
                        document.OfferingId = documentDto.OfferingId;
                        document.FilePath = blobFilePath;
                        document.Type = documentType;
                        document.Name = file.FileName;
                        document.IsPrivate = documentDto.IsPrivate;
                        var extn = Path.GetExtension(file.FileName);
                        document.Extension = extn.Replace(".", "");
                        document.Size  = (file.Length / 1024).ToString();
                        document.Status = 1;//Pending
                        document.Active = true;
                        document.CreatedBy = documentDto.AdminUserId.ToString();
                        document.CreatedOn = DateTime.Now;
                        _unitOfWork.DocumentRepository.Insert(document);
                        contextData.SaveChanges();
                    }

                    if (documentDto.OfferingDocuments != null)
                    {
                        documentType = 4;//Offering Documents
                        foreach (var file in documentDto.OfferingDocuments)
                        {                           
                            var blobFilePath = (await _helper.DocumentSaveAndUpload(file, documentDto.OfferingId, documentType)).ToString();

                            Document document = new Document();
                            document.OfferingId = documentDto.OfferingId;
                            document.FilePath = blobFilePath;
                            document.Type = documentType;
                            document.Name = file.FileName;
                            var extn = Path.GetExtension(file.FileName);
                            document.Extension = extn.Replace(".", "");
                            document.Size = (file.Length / 1024).ToString();
                            document.Status = 1;//Pending
                            document.Active = true;                           
                            document.CreatedBy = documentDto.AdminUserId.ToString();
                            document.CreatedOn = DateTime.Now;
                            documents.Add(document);
                        }
                    }
                    contextData.AddRange(documents);
                    contextData.SaveChanges();
                    transaction.Commit();

                    PortfolioDocumentsResultDto result = new PortfolioDocumentsResultDto();
                    result.OfferingId = documentDto.OfferingId;
                    result.Status = true;
                    result.OfferingDocuments = (from document in contextData.Document
                                                           where document.OfferingId == documentDto.OfferingId
                                                           && document.Active == true
                                                           select new DocumentDto
                                                           {
                                                               Id = document.Id,
                                                               OfferingId = document.Id,
                                                               Type = document.Type,
                                                               FilePath = document.FilePath,
                                                               Name = document.Name,
                                                               CreatedOn = document.CreatedOn
                                                           }).OrderByDescending(x=>x.CreatedOn).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure 
            }
            finally
            {
                contextData = null;
            }            
        }
        public PortfolioDocumentsResultDto DeleteOfferingDocument(int adminuserid, int offeringId, int documentId)
        {
            var contextData = _unitOfWork.DocumentRepository.Context;
            try
            {
                var documentData = _unitOfWork.DocumentRepository.Get(x => x.Id == documentId && x.OfferingId == offeringId);
                if (documentData.Id != 0)
                {
                    using (var transaction = contextData.Database.BeginTransaction())
                    {
                        Document document = documentData;
                        document.Active = false;
                        document.ModifiedBy = adminuserid.ToString();
                        _unitOfWork.DocumentRepository.Update(document);
                        _unitOfWork.Save();
                        transaction.Commit();
                    }
                    PortfolioDocumentsResultDto result = new PortfolioDocumentsResultDto();
                    result.OfferingId = offeringId;
                    result.Status = true;
                    result.OfferingDocuments = (from document in contextData.Document
                                                           where document.OfferingId == offeringId
                                                           && document.Active == true
                                                           select new DocumentDto
                                                           {
                                                               Id = document.Id,
                                                               OfferingId = document.Id,
                                                               Type = document.Type,
                                                               FilePath = document.FilePath,
                                                               Name = document.Name,
                                                               CreatedOn = document.CreatedOn
                                                           }).OrderByDescending(x => x.CreatedOn).ToList();
                    return result;
                }
                return null;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure 
            }
            finally
            {
                contextData = null;
            }           
        }
        public bool AddDefaultPortfolioKeyHighlights(int id,int adminUserId)
        {
            var contextData = _unitOfWork.PortfolioKeyHighlightRepository.Context;
            try
            {               
                var keyHighlights = _unitOfWork.KeyHighlightRepository.GetMany(x => x.Active == true);                
                if (keyHighlights != null)
                 {
                    using (var transaction = contextData.Database.BeginTransaction())
                    {
                        List<PortfolioKeyHighlight> portfolioKeyHighlights = new List<PortfolioKeyHighlight>();
                        foreach (var keyHighlight in keyHighlights)
                        {

                            PortfolioKeyHighlight portfolioKeyHighlight = new PortfolioKeyHighlight();
                            portfolioKeyHighlight.OfferingId = id;//Offering/Reservation
                            portfolioKeyHighlight.KeyHighlightId = keyHighlight.Id;
                            portfolioKeyHighlight.Active = true;
                            portfolioKeyHighlight.Value = " ";
                            portfolioKeyHighlight.Visibility = true;
                            portfolioKeyHighlight.CreatedBy = adminUserId.ToString();
                            portfolioKeyHighlight.CreatedOn = DateTime.Now;

                            portfolioKeyHighlights.Add(portfolioKeyHighlight);
                        }
                        contextData.AddRange(portfolioKeyHighlights);
                        contextData.SaveChanges();
                        transaction.Commit();

                        keyHighlights = null;
                        portfolioKeyHighlights = null;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                e.ToString();
                return false;//Failure 
            }
            finally
            {
                contextData = null;
            }
        }
        public UpdatePortfolioKeyHightlightDto UpdatePortfolioKeyHightlight(UpdatePortfolioKeyHightlightDto updatePortfolioKeyHightlight)
        {
            var contextData = _unitOfWork.PortfolioKeyHighlightRepository.Context;
            try
            {               
                var keyHighlights = _unitOfWork.KeyHighlightRepository.GetMany(x => x.Active == true);
                if (keyHighlights != null)
                {
                    using (var transaction = contextData.Database.BeginTransaction())
                    {                      
                        foreach (var portfolioKeyHighlightDto in updatePortfolioKeyHightlight.PortfolioKeyHighlights)
                        {
                            if (portfolioKeyHighlightDto.Id == 0)
                            {                               
                                PortfolioKeyHighlight portfolioKeyHighlight = new PortfolioKeyHighlight();
                                portfolioKeyHighlight.OfferingId = portfolioKeyHighlightDto.OfferingId;
                                portfolioKeyHighlight.Field = portfolioKeyHighlightDto.Field;
                                portfolioKeyHighlight.Active = true;
                                portfolioKeyHighlight.Value = portfolioKeyHighlightDto.Value;
                                portfolioKeyHighlight.Visibility = portfolioKeyHighlightDto.Visibility;
                                portfolioKeyHighlight.CreatedBy = updatePortfolioKeyHightlight.AdminUserId.ToString();
                                portfolioKeyHighlight.CreatedOn = DateTime.Now;
                                _unitOfWork.PortfolioKeyHighlightRepository.Insert(portfolioKeyHighlight);
                                contextData.SaveChanges();                                
                            }
                            else
                            {
                                var portfolioKeyHightlightData = _unitOfWork.PortfolioKeyHighlightRepository.GetByID(portfolioKeyHighlightDto.Id);
                                if(portfolioKeyHightlightData != null)
                                {                                    
                                    portfolioKeyHightlightData.Field = portfolioKeyHighlightDto.Field;
                                    portfolioKeyHightlightData.Active = portfolioKeyHighlightDto.Active;
                                    portfolioKeyHightlightData.Value = portfolioKeyHighlightDto.Value;
                                    portfolioKeyHightlightData.Visibility = portfolioKeyHighlightDto.Visibility;
                                    portfolioKeyHightlightData.ModifiedBy = updatePortfolioKeyHightlight.AdminUserId.ToString();
                                    portfolioKeyHightlightData.ModifiedOn = DateTime.Now;
                                    _unitOfWork.PortfolioKeyHighlightRepository.Update(portfolioKeyHightlightData);                                    
                                }
                            }
                        }
                        contextData.SaveChanges();                       
                        transaction.Commit();

                        var keyHightlights = (from PKH in contextData.PortfolioKeyHighlight
                                              where PKH.OfferingId == updatePortfolioKeyHightlight.OfferingId
                                              && PKH.Active == true
                                              join KH in contextData.KeyHighlight on PKH.KeyHighlightId equals KH.Id into keyhighlights
                                              from KH in keyhighlights.DefaultIfEmpty()
                                              select new PortfolioKeyHighlightDto
                                              {
                                                  Id = PKH.Id,
                                                  OfferingId = PKH.OfferingId,
                                                  KeyHighLightId = PKH.KeyHighlightId,
                                                  Name = KH.Name,
                                                  Field = PKH.Field,
                                                  Value = PKH.Value,
                                                  Active = PKH.Active,
                                                  Visibility = PKH.Visibility
                                              }).ToList();

                        UpdatePortfolioKeyHightlightDto result = new UpdatePortfolioKeyHightlightDto();
                        result.OfferingId = updatePortfolioKeyHightlight.OfferingId;                        
                        result.PortfolioKeyHighlights = keyHightlights;
                        result.Status = true;                                                 
                        return result;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure 
            }
            finally
            {
                contextData = null;
            }
        }
        public PortfolioLocationDto AddPortfolioLocation(PortfolioLocationDto portfolioLocationDto)
        {
            var contextData = _unitOfWork.PortfolioLocationRepository.Context;
            try
            {
                using (var transaction = _unitOfWork.PortfolioLocationRepository.Context.Database.BeginTransaction())
                {
                    PortfolioLocation portfolioLocation = new PortfolioLocation();
                    portfolioLocation.OfferingId = portfolioLocationDto.OfferingId;
                    portfolioLocation.Location = portfolioLocationDto.Location;
                    portfolioLocation.Latitude = portfolioLocationDto.Latitude;
                    portfolioLocation.Longitude = portfolioLocationDto.Longitude;
                    portfolioLocation.Status = 1;
                    portfolioLocation.Active = true;
                    portfolioLocation.CreatedBy = portfolioLocationDto.AdminUserId.ToString();
                    portfolioLocation.CreatedOn = DateTime.Now;

                    _unitOfWork.PortfolioLocationRepository.Insert(portfolioLocation);
                    contextData.SaveChanges();
                    transaction.Commit();                    

                    var portfolioLocationData = (from location in contextData.PortfolioLocation
                                                where location.OfferingId == portfolioLocationDto.OfferingId
                                                && location.Active == true
                                                select new PortfolioLocationDto
                                                {
                                                    Id = location.Id,
                                                    OfferingId = location.OfferingId,
                                                    Latitude = location.Latitude,
                                                    Longitude = location.Longitude,
                                                    Location = location.Location,
                                                    Status = location.Status
                                                }).FirstOrDefault();

                    return portfolioLocationData;
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure 
            }
            finally
            {
                contextData = null;
            }
        }
        public PortfolioLocationDto UpdatePortfolioLocation(PortfolioLocationDto portfolioLocationDto)
        {
            var contextData = _unitOfWork.PortfolioLocationRepository.Context;
            try
            {
                using (var transaction = _unitOfWork.PortfolioLocationRepository.Context.Database.BeginTransaction())
                {
                    var portfolioLocation = _unitOfWork.PortfolioLocationRepository.Get(x => x.OfferingId == portfolioLocationDto.OfferingId && x.Active == true);

                    if (portfolioLocation != null)
                    {
                        portfolioLocation.Location = portfolioLocationDto.Location;
                        portfolioLocation.Latitude = portfolioLocationDto.Latitude;
                        portfolioLocation.Longitude = portfolioLocationDto.Longitude;
                        portfolioLocation.ModifiedBy = portfolioLocationDto.AdminUserId.ToString();
                        portfolioLocation.ModifiedOn = DateTime.Now;

                        _unitOfWork.PortfolioLocationRepository.Update(portfolioLocation);
                        contextData.SaveChanges();
                        transaction.Commit();

                        var portfolioLocationData = (from location in contextData.PortfolioLocation
                                                     where location.OfferingId == portfolioLocationDto.OfferingId
                                                     && location.Active == true
                                                     select new PortfolioLocationDto
                                                     {
                                                         Id = location.Id,
                                                         OfferingId = location.OfferingId,
                                                         Latitude = location.Latitude,
                                                         Longitude = location.Longitude,
                                                         Location = location.Location,
                                                         Status = location.Status
                                                     }).FirstOrDefault();

                        return portfolioLocationData;
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure 
            }
            finally
            {
                contextData = null;
            }
        }
        public PortfolioFundingInstructionsDto AddPortfolioFundingInstructions(PortfolioFundingInstructionsDto portfolioFundingInstructionsDto)
        {            
            var contextData = _unitOfWork.PortfolioFundingInstructionsRepository.Context;
            try
            {
                using (var transaction = _unitOfWork.PortfolioFundingInstructionsRepository.Context.Database.BeginTransaction())
                {
                    PortfolioFundingInstructions portfolioFundingInstructions = new PortfolioFundingInstructions();
                    portfolioFundingInstructions.OfferingId = portfolioFundingInstructionsDto.OfferingId;
                    portfolioFundingInstructions.ReceivingBank = portfolioFundingInstructionsDto.ReceivingBank;
                    portfolioFundingInstructions.Beneficiary = portfolioFundingInstructionsDto.Beneficiary;
                    portfolioFundingInstructions.BeneficiaryAddress = portfolioFundingInstructionsDto.BeneficiaryAddress;
                    portfolioFundingInstructions.BankAddress = portfolioFundingInstructionsDto.BankAddress;
                    portfolioFundingInstructions.RoutingNumber = portfolioFundingInstructionsDto.RoutingNumber;
                    portfolioFundingInstructions.AccountNumber = portfolioFundingInstructionsDto.AccountNumber;
                    portfolioFundingInstructions.AccountType = portfolioFundingInstructionsDto.AccountType;
                    portfolioFundingInstructions.Reference = portfolioFundingInstructionsDto.Reference;
                    portfolioFundingInstructions.OtherInstructions = portfolioFundingInstructionsDto.OtherInstructions;
                    portfolioFundingInstructions.MailingAddress = portfolioFundingInstructionsDto.MailingAddress;
                    portfolioFundingInstructions.Beneficiary = portfolioFundingInstructionsDto.Beneficiary;
                    portfolioFundingInstructions.Memo = portfolioFundingInstructionsDto.Memo;
                    portfolioFundingInstructions.Custom = portfolioFundingInstructionsDto.Custom;
                    portfolioFundingInstructions.CheckBenificiary = portfolioFundingInstructionsDto.CheckBenificiary;
                    portfolioFundingInstructions.CheckOtherInstructions = portfolioFundingInstructionsDto.CheckOtherInstructions;
                    portfolioFundingInstructions.Status = 1;
                    portfolioFundingInstructions.Active = true;
                    portfolioFundingInstructions.CreatedBy = portfolioFundingInstructionsDto.AdminUserId.ToString();
                    portfolioFundingInstructions.CreatedOn = DateTime.Now;
                    
                    _unitOfWork.PortfolioFundingInstructionsRepository.Insert(portfolioFundingInstructions);
                    contextData.SaveChanges();
                    transaction.Commit();

                    var portfolioFundingInstruction = (from fundInstruction in contextData.PortfolioFundingInstructions
                                                 where fundInstruction.OfferingId == portfolioFundingInstructionsDto.OfferingId
                                                 && fundInstruction.Active == true
                                                 select new PortfolioFundingInstructionsDto
                                                 {
                                                     Id = fundInstruction.Id,
                                                     OfferingId = fundInstruction.OfferingId,
                                                     ReceivingBank = fundInstruction.ReceivingBank,
                                                     BankAddress = fundInstruction.BankAddress,
                                                     Beneficiary = fundInstruction.Beneficiary,
                                                     BeneficiaryAddress = fundInstruction.BeneficiaryAddress,
                                                     RoutingNumber = fundInstruction.RoutingNumber,
                                                     AccountNumber = fundInstruction.AccountNumber,
                                                     AccountType = fundInstruction.AccountType,
                                                     Reference = fundInstruction.Reference,
                                                     OtherInstructions = fundInstruction.OtherInstructions,
                                                     MailingAddress = fundInstruction.MailingAddress,
                                                     Memo = fundInstruction.Memo,
                                                     Custom = fundInstruction.Custom,
                                                     CheckBenificiary = fundInstruction.CheckBenificiary,
                                                     CheckOtherInstructions = fundInstruction.CheckOtherInstructions,
                                                     Status = fundInstruction.Status
                                                 }).FirstOrDefault();

                    return portfolioFundingInstruction;
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure 
            }
            finally
            {
                contextData = null;
            }
        }
        public PortfolioFundingInstructionsDto UpdatePortfolioFundingInstructions(PortfolioFundingInstructionsDto portfolioFundingInstructionsDto)
        {
            var contextData = _unitOfWork.PortfolioFundingInstructionsRepository.Context;
            try
            {
                var portfolioFundingInstructions = _unitOfWork.PortfolioFundingInstructionsRepository.Get(x => x.OfferingId == portfolioFundingInstructionsDto.OfferingId && x.Active == true);
                if (portfolioFundingInstructions != null)
                {
                    using (var transaction = _unitOfWork.PortfolioFundingInstructionsRepository.Context.Database.BeginTransaction())
                    {
                        portfolioFundingInstructions.OfferingId = portfolioFundingInstructionsDto.OfferingId;
                        portfolioFundingInstructions.OfferingId = portfolioFundingInstructionsDto.OfferingId;
                        portfolioFundingInstructions.ReceivingBank = portfolioFundingInstructionsDto.ReceivingBank;
                        portfolioFundingInstructions.Beneficiary = portfolioFundingInstructionsDto.Beneficiary;
                        portfolioFundingInstructions.BeneficiaryAddress = portfolioFundingInstructionsDto.BeneficiaryAddress;
                        portfolioFundingInstructions.BankAddress = portfolioFundingInstructionsDto.BankAddress;
                        portfolioFundingInstructions.RoutingNumber = portfolioFundingInstructionsDto.RoutingNumber;
                        portfolioFundingInstructions.AccountNumber = portfolioFundingInstructionsDto.AccountNumber;
                        portfolioFundingInstructions.AccountType = portfolioFundingInstructionsDto.AccountType;
                        portfolioFundingInstructions.Reference = portfolioFundingInstructionsDto.Reference;
                        portfolioFundingInstructions.OtherInstructions = portfolioFundingInstructionsDto.OtherInstructions;
                        portfolioFundingInstructions.MailingAddress = portfolioFundingInstructionsDto.MailingAddress;
                        portfolioFundingInstructions.Beneficiary = portfolioFundingInstructionsDto.Beneficiary;
                        portfolioFundingInstructions.CheckBenificiary = portfolioFundingInstructionsDto.CheckBenificiary;
                        portfolioFundingInstructions.CheckOtherInstructions = portfolioFundingInstructionsDto.CheckOtherInstructions;
                        portfolioFundingInstructions.Memo = portfolioFundingInstructionsDto.Memo;
                        portfolioFundingInstructions.Custom = portfolioFundingInstructionsDto.Custom;
                        portfolioFundingInstructions.ModifiedBy = portfolioFundingInstructionsDto.AdminUserId.ToString();
                        portfolioFundingInstructions.ModifiedOn = DateTime.Now;

                        _unitOfWork.PortfolioFundingInstructionsRepository.Update(portfolioFundingInstructions);
                        contextData.SaveChanges();
                        transaction.Commit();

                        var portfolioFundingInstruction = (from fundInstruction in contextData.PortfolioFundingInstructions
                                                           where fundInstruction.OfferingId == portfolioFundingInstructionsDto.OfferingId
                                                           && fundInstruction.Active == true
                                                           select new PortfolioFundingInstructionsDto
                                                           {
                                                               Id = fundInstruction.Id,
                                                               OfferingId = fundInstruction.OfferingId,
                                                               ReceivingBank = fundInstruction.ReceivingBank,
                                                               BankAddress = fundInstruction.BankAddress,
                                                               Beneficiary = fundInstruction.Beneficiary,
                                                               BeneficiaryAddress = fundInstruction.BeneficiaryAddress,
                                                               RoutingNumber = fundInstruction.RoutingNumber,
                                                               AccountNumber = fundInstruction.AccountNumber,
                                                               AccountType = fundInstruction.AccountType,
                                                               Reference = fundInstruction.Reference,
                                                               OtherInstructions = fundInstruction.OtherInstructions,
                                                               MailingAddress = fundInstruction.MailingAddress,
                                                               Memo = fundInstruction.Memo,
                                                               Custom = fundInstruction.Custom,
                                                               Status = fundInstruction.Status,
                                                               CheckBenificiary = fundInstruction.CheckBenificiary,
                                                               CheckOtherInstructions = fundInstruction.CheckOtherInstructions
                                                           }).FirstOrDefault();

                        return portfolioFundingInstruction;
                    }
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;//Failure 
            }
            finally
            {
                contextData = null;
            }
        }
        public PortfolioInvestmentsSummaryDto GetPortfolioInvestorsSummary(int offeringId)
        {
            PortfolioInvestmentsSummaryDto elements = null;
            var contextData = _unitOfWork.InvestmentRepository.Context;
            try
            {
                /* elements = (from summary in contextData.InvestorSummary
                                where summary.UserId == userId
                                select new InvestmentSummaryDto
                                {
                                    UserId = summary.UserId,
                                    TotalInvestments = summary.TotalInvestments == null ? 0 : summary.TotalInvestments,
                                    PendingInvestments = summary.PendingInvestments == null ? 0 : summary.PendingInvestments,
                                    TotalInvested = summary.TotalInvested,
                                    TotalEarnings = summary.TotalEarnings,
                                    TotalReturn = summary.TotalReturn
                                }
                                ).FirstOrDefault();*/
                if (elements == null)
                {
                    elements = new PortfolioInvestmentsSummaryDto();
                    elements.OfferingId = offeringId;
                    elements.OfferingSize = _unitOfWork.PortfolioOfferingRepository.GetByID(offeringId).Size;
                    elements.Committed = (from investment in contextData.Investment
                                          where investment.OfferingId == offeringId
                                          &&
                                           (investment.Status == 3 // Pending Investor Signature and Funding
                                            || investment.Status == 1 // Approved
                                            || investment.Status == 4 // Pending Funding
                                            || investment.Status == 2) // Pending                                       
                                          && investment.Active == true // Active Investment
                                          && investment.IsReservation == false // Investment
                                          select investment.Amount).Sum();
                    elements.Remaining = elements.OfferingSize - elements.Committed;
                    elements.TotalApproved = (from investment in contextData.Investment
                                              where investment.OfferingId == offeringId
                                              && investment.Status == 1 // Approved                                                                                     
                                              && investment.Active == true // Active Investment
                                              && investment.IsReservation == false // Investment
                                              select investment.Amount).Sum(); 
                    elements.TotalPending = (from investment in contextData.Investment
                                             where investment.OfferingId == offeringId
                                             && (investment.Status == 2 // Pending
                                               ||investment.Status == 3 // Pending Investor Signature and Funding                                              
                                               || investment.Status == 4) // Pending Funding
                                             && investment.IsReservation == false // Investment                                                                          
                                             && investment.Active == true // Active Investment
                                             select investment.Amount).Sum(); 
                    elements.Approved = (from investment in contextData.Investment
                                         where investment.OfferingId == offeringId
                                         && investment.Status == 1 // Approved                                                                                     
                                         && investment.Active == true // Active Investment
                                         && investment.IsReservation == false // Investment
                                         select investment.Id).Count();
                    elements.Pending = (from investment in contextData.Investment
                                        where investment.OfferingId == offeringId
                                        && (investment.Status == 2 // Pending
                                          || investment.Status == 3 // Pending Investor Signature and Funding                                              
                                          || investment.Status == 4) // Pending Funding                                        
                                        && investment.Active == true // Active Investment
                                        && investment.IsReservation == false // Investment
                                        select investment.Id).Count();
                    elements.Waitlist = (from investment in contextData.Investment
                                         where investment.OfferingId == offeringId
                                         && investment.Status == 5 // Waitlisted                                                                                     
                                         && investment.Active == true // Active Investment
                                         && investment.IsReservation == false // Investment
                                         select investment.Id).Count();
                    elements.AverageApproved = elements.TotalApproved / elements.Approved;
                    elements.NonAccredited = (from investment in contextData.Investment
                                              join investor in contextData.UserAccount on investment.UserId equals investor.Id
                                              where investment.OfferingId == offeringId
                                              && investor.IsAccreditedInvestor == false                                                                                                                               
                                              && investment.Active == true // Active Investment
                                              && investment.IsReservation == false // Investment
                                              select investment.Id).Count();                                     

                    return elements;
                }
                else
                    return elements;
            }
            catch (Exception e)
            {
                e.ToString();
                return elements;
            }
            finally
            {
                contextData = null;
            }
        }
        public List<PortfolioInvestmentDataDto> GetPortfolioOfferingInvestments(int offeringId)
        {
            try
            {
                var contextData = _unitOfWork.InvestmentRepository.Context;

                var investmentsData = (from investment in contextData.Investment
                                       join userAccount in contextData.UserAccount on investment.UserId equals userAccount.Id
                                       join userprofile in contextData.UserProfile on investment.UserProfileId equals userprofile.Id
                                       join offering in contextData.PortfolioOffering on investment.OfferingId equals offering.Id
                                       join profiletype in contextData.UserProfileType on userprofile.Type equals profiletype.Id
                                       where investment.OfferingId == offeringId
                                            && investment.IsReservation == false
                                            && investment.Active == true
                                       select new PortfolioInvestmentDataDto
                                       {
                                           Id = investment.Id,
                                           OfferingId = offering.Id,
                                           UserId = userAccount.Id,
                                           ProfileId = userprofile.Id,
                                           InvestorName = userAccount.FirstName + " " + userAccount.LastName,
                                           Amount = investment.Amount,
                                           Status = investment.Status,
                                           OfferingName = offering.Name,
                                           ProfileName = userprofile.FirstName + " " + userprofile.LastName + userprofile.Name + userprofile.RetirementPlanName + userprofile.TrustName,
                                           ProfileTypeName = profiletype.Name,
                                           IsAccredited = userAccount.IsAccreditedInvestor != null ? Convert.ToBoolean(userAccount.IsAccreditedInvestor) : false,
                                           IsVerified = userAccount.IsEmailVerified || userAccount.IsPhoneVerified,
                                           FundsReceivedDate = investment.FundedDate,
                                           eSignedDocumentPath = investment.eSignedDocumentPath,
                                           DocumenteSignedDate = investment.DocumenteSignedDate,
                                           Notes = investment.Notes,
                                           CreatedOn = investment.CreatedOn
                                       }).OrderByDescending(x => x.CreatedOn).ToList();
                contextData = null;
                return investmentsData;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }
        public PortfolioReservationsSummaryDto GetPortfolioReservationsSummary(int offeringId)
        {
            PortfolioReservationsSummaryDto elements = null;
            var contextData = _unitOfWork.InvestmentRepository.Context;
            try
            {
                /* elements = (from summary in contextData.InvestorSummary
                                where summary.UserId == userId
                                select new InvestmentSummaryDto
                                {
                                    UserId = summary.UserId,
                                    TotalInvestments = summary.TotalInvestments == null ? 0 : summary.TotalInvestments,
                                    PendingInvestments = summary.PendingInvestments == null ? 0 : summary.PendingInvestments,
                                    TotalInvested = summary.TotalInvested,
                                    TotalEarnings = summary.TotalEarnings,
                                    TotalReturn = summary.TotalReturn
                                }
                                ).FirstOrDefault();*/
                if (elements == null)
                {
                    elements = new PortfolioReservationsSummaryDto();
                    elements.OfferingId = offeringId;                    
                    elements.TotalReserved = (from investment in contextData.Investment
                                          where investment.OfferingId == offeringId                                                                            
                                          && investment.Active == true // Active Investment
                                          && (investment.IsReservation == true || investment.IsConverted == true) // Reservation or reservation converted to investment
                                          select investment.Amount).Sum();                    
                    elements.Converted = (from investment in contextData.Investment
                                              where investment.OfferingId == offeringId                                                                                                                             
                                              && investment.Active == true // Active Investment
                                              && (investment.IsConverted == true)
                                          select investment.Amount).Sum();                  
                    elements.Reservations = (from investment in contextData.Investment
                                         where investment.OfferingId == offeringId                                                                                                                           
                                         && investment.Active == true // Active Investment
                                         && (investment.IsReservation == true || investment.IsConverted == true)
                                         select investment.Id).Count();
                    elements.Remaining = elements.TotalReserved - elements.Converted;
                    return elements;
                }
                else
                    return elements;
            }
            catch (Exception e)
            {
                e.ToString();
                return elements;
            }
            finally
            {
                contextData = null;
            }
        }
        public ReservationsSummaryDto GetReservationsSummary(int reservationId)
        {
            ReservationsSummaryDto elements = null;
            var contextData = _unitOfWork.InvestmentRepository.Context;
            try
            {
                /* elements = (from summary in contextData.InvestorSummary
                                where summary.UserId == userId
                                select new InvestmentSummaryDto
                                {
                                    UserId = summary.UserId,
                                    TotalInvestments = summary.TotalInvestments == null ? 0 : summary.TotalInvestments,
                                    PendingInvestments = summary.PendingInvestments == null ? 0 : summary.PendingInvestments,
                                    TotalInvested = summary.TotalInvested,
                                    TotalEarnings = summary.TotalEarnings,
                                    TotalReturn = summary.TotalReturn
                                }
                                ).FirstOrDefault();*/
                if (elements == null)
                {
                    elements = new ReservationsSummaryDto();
                    elements.ReservationId = reservationId;
                    elements.ReservationSize = _unitOfWork.PortfolioOfferingRepository.GetByID(reservationId).Size;
                    elements.TotalReserved = (from investment in contextData.Investment
                                              where investment.OfferingId == reservationId
                                              && investment.Active == true // Active Investment
                                              && (investment.IsReservation == true) // Reservation or reservation converted to investment
                                              select investment.Amount).Sum();
                    elements.NonAccredited = (from investment in contextData.Investment
                                              join user in contextData.UserAccount on investment.UserId equals user.Id
                                               where investment.OfferingId == reservationId
                                          && investment.Active == true // Active Investment
                                          && (investment.IsConverted == true)
                                          select investment.Id).Count();
                    elements.Reservations = (from investment in contextData.Investment
                                             where investment.OfferingId == reservationId
                                             && investment.Active == true // Active Investment
                                             && (investment.IsReservation == true)
                                             select investment.Id).Count();
                    elements.Remaining = elements.ReservationSize - elements.TotalReserved;
                    return elements;
                }
                else
                    return elements;
            }
            catch (Exception e)
            {
                e.ToString();
                return elements;
            }
            finally
            {
                contextData = null;
            }
        }
        public List<PortfolioReservationDataDto> GetPortfolioOfferingReservations(int offeringId)
        {
            try
            {
                var contextData = _unitOfWork.InvestmentRepository.Context;

                var reservationsData = (from investment in contextData.Investment
                                       join userAccount in contextData.UserAccount on investment.UserId equals userAccount.Id
                                       join userprofile in contextData.UserProfile on investment.UserProfileId equals userprofile.Id
                                       join offering in contextData.PortfolioOffering on investment.OfferingId equals offering.Id
                                       join profiletype in contextData.UserProfileType on userprofile.Type equals profiletype.Id                                       
                                       where investment.OfferingId == offeringId
                                            && (investment.IsReservation == true || investment.IsConverted == true)
                                            && investment.Active == true
                                       select new PortfolioReservationDataDto
                                       {
                                           Id = investment.Id,
                                           UserId = investment.UserId,
                                           ProfileId = investment.UserProfileId,
                                           OfferingId = offering.Id,
                                           InvestorName = userAccount.FirstName + " " + userAccount.LastName,
                                           Amount = investment.Amount,
                                           EmailId = userAccount.EmailId,
                                           PhoneNumber = userAccount.PhoneNumber,
                                           ConfidenceLevel = investment.ConfidenceLevel, 
                                           ProfileTypeId = userprofile.Type,
                                           ProfileName = userprofile.FirstName + " " + userprofile.LastName + userprofile.Name + userprofile.RetirementPlanName + userprofile.TrustName,
                                           ProfileTypeName = profiletype.Name,
                                           IsAccredited = userAccount.IsAccreditedInvestor != null ? Convert.ToBoolean(userAccount.IsAccreditedInvestor) : false,
                                           IsConverted = investment.IsConverted,
                                           ConvertedOn = investment.ConvertedOn,
                                           Notes = investment.Notes,
                                           ModifiedOn = investment.ModifiedOn,
                                           CreatedOn = investment.CreatedOn
                                       }).OrderByDescending(x=>x.CreatedOn).ToList();
                contextData = null;
                return reservationsData;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }
        public List<PortfolioReservationDataDto> GetReservationsList(int reservationId)
        {
            try
            {
                var contextData = _unitOfWork.InvestmentRepository.Context;

                var reservationsData = (from investment in contextData.Investment
                                        join userAccount in contextData.UserAccount on investment.UserId equals userAccount.Id
                                        join userprofile in contextData.UserProfile on investment.UserProfileId equals userprofile.Id
                                        join offering in contextData.PortfolioOffering on investment.OfferingId equals offering.Id
                                        join profiletype in contextData.UserProfileType on userprofile.Type equals profiletype.Id
                                        where investment.OfferingId == reservationId
                                             && (investment.IsReservation == true)
                                             && investment.Active == true
                                        select new PortfolioReservationDataDto
                                        {
                                            Id = investment.Id,
                                            OfferingId = offering.Id,
                                            UserId = investment.UserId,
                                            ProfileId = investment.UserProfileId,
                                            InvestorName = userAccount.FirstName + " " + userAccount.LastName,
                                            Amount = investment.Amount,
                                            EmailId = userAccount.EmailId,
                                            PhoneNumber = userAccount.PhoneNumber,
                                            ConfidenceLevel = investment.ConfidenceLevel,
                                            ProfileTypeId = userprofile.Type,
                                            ProfileName = userprofile.FirstName + " " + userprofile.LastName + userprofile.Name + userprofile.RetirementPlanName + userprofile.TrustName,
                                            ProfileTypeName = profiletype.Name,
                                            IsAccredited = userAccount.IsAccreditedInvestor != null ? Convert.ToBoolean(userAccount.IsAccreditedInvestor) : false,
                                            IsConverted = investment.IsConverted,
                                            ConvertedOn = investment.ConvertedOn,
                                            Notes = investment.Notes,
                                            ModifiedOn = investment.ModifiedOn,
                                            CreatedOn = investment.CreatedOn
                                        }).OrderByDescending(x => x.CreatedOn).ToList();
                contextData = null;
                return reservationsData;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }
        public bool AddInvestmentNotes(InvestmentNotesDto investmentNotesDto)
        {
            try
            {
                using (var transaction = _unitOfWork.InvestmentRepository.Context.Database.BeginTransaction())
                {
                    var investment = _unitOfWork.InvestmentRepository.GetByID(investmentNotesDto.InvestmentId);
                    investment.Notes = investmentNotesDto.Notes;
                    investment.ModifiedBy = investmentNotesDto.AdminUserId.ToString();
                    _unitOfWork.InvestmentRepository.Update(investment);
                    _unitOfWork.Save();
                    transaction.Commit();                                     
                    return true; //Success
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return false;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public bool AddReservationNotes(ReservationNotesDto reservationNotesDto)
        {
            try
            {
                using (var transaction = _unitOfWork.InvestmentRepository.Context.Database.BeginTransaction())
                {
                    var reservation = _unitOfWork.InvestmentRepository.GetByID(reservationNotesDto.ReservationId);
                    reservation.Notes = reservationNotesDto.Notes;
                    reservation.ModifiedBy = reservationNotesDto.AdminUserId.ToString();
                    _unitOfWork.InvestmentRepository.Update(reservation);
                    _unitOfWork.Save();
                    transaction.Commit();
                    return true; //Success
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return false;//Failure
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        public List<InvestmentStatusDto> GetInvestmentStatuses()
        {
            List<InvestmentStatusDto> statuses = new List<InvestmentStatusDto>();
            var contextData = _unitOfWork.InvestmentStatusRepository.Context;
            try
            {
                statuses = (from status in contextData.InvestmentStatus
                            select new InvestmentStatusDto
                            {
                                Id = status.Id,
                                Name = status.Name,
                                Active = status.Active
                            }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
                return statuses;
            }
            finally
            {
                contextData = null;
            }
            return statuses;
        }
        public List<PortfolioDistributionTypeDto> GetPortfolioDistributionTypes()
        {
            List<PortfolioDistributionTypeDto> types = new List<PortfolioDistributionTypeDto>();
            var contextData = _unitOfWork.PortfolioDistributionTypeRepository.Context;
            try
            {
                types = (from type in contextData.PortfolioDistributionType
                            select new PortfolioDistributionTypeDto
                            {
                                Id = type.Id,
                                Name = type.Name,
                                Active = type.Active
                            }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
                return types;
            }
            finally
            {
                contextData = null;
            }
            return types;
        }     
        public bool UpdateCapTable(UpdateCapTableDto capTableDto)
        {
            var contextData = _unitOfWork.OfferingCapTableRepository.Context;
            try
            {
                var capTableInvestment = _unitOfWork.OfferingCapTableRepository.Get(x => x.Id == capTableDto.Id);
                if (capTableInvestment != null)
                {
                    using (var transaction = _unitOfWork.OfferingCapTableRepository.Context.Database.BeginTransaction())
                    {
                        capTableInvestment.PercentageOwnership = capTableDto.FundedPercentage;
                        capTableInvestment.ModifiedBy = capTableDto.AdminUserId.ToString();
                        capTableInvestment.ModifiedOn = DateTime.Now;

                        _unitOfWork.OfferingCapTableRepository.Update(capTableInvestment);
                        contextData.SaveChanges();
                        transaction.Commit();
                    }
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                e.ToString();
                throw;
            }
            finally
            {
                contextData = null;
            }            
        }
        public bool UpdateDistribution(DistributionsDataDto distributionsData)
        {
            return true;
        }
        public CapTableDataDto GetPortfolioOfferingCapTable(int offeringid)
        {           
            var contextData = _unitOfWork.InvestmentRepository.Context;
            CapTableDataDto capTableDataDto = new CapTableDataDto();
            try
            {
                var offeringSize = _unitOfWork.PortfolioOfferingRepository.GetByID(offeringid).Size;
                capTableDataDto.TotalFundedAmount = (from investment in contextData.Investment                                                     
                                                     where investment.OfferingId == offeringid
                                                            && investment.Status == 1 // Approved
                                                     select investment.Amount).Sum();                
                if (capTableDataDto.TotalFundedAmount != 0)
                {
                    capTableDataDto.CapTableInvestments = (from investment in contextData.Investment
                                                           join userAccount in contextData.UserAccount on investment.UserId equals userAccount.Id
                                                           join userProfile in contextData.UserProfile on investment.UserProfileId equals userProfile.Id
                                                           where investment.OfferingId == offeringid
                                                                && investment.Status == 1 // Approved
                                                                && investment.Active == true
                                                           select new CapTableInvestmentDto
                                                           {
                                                               InvestmentId = investment.Id,
                                                               OfferingId = investment.OfferingId,
                                                               FundedPercentage = (investment.Amount / capTableDataDto.TotalFundedAmount) * 100,                                                               
                                                               OwnershipPercentage = (investment.Amount / offeringSize) * 100
                                                           }).ToList();
                    if (capTableDataDto.CapTableInvestments != null && capTableDataDto.CapTableInvestments.Count > 0)
                    {                           
                        using (var transaction = _unitOfWork.OfferingCapTableRepository.Context.Database.BeginTransaction())
                        {
                            List<OfferingCapTable> offeringCapTables = new List<OfferingCapTable>();
                            foreach (var investment in capTableDataDto.CapTableInvestments)
                            {
                                var capTableInvestment = _unitOfWork.OfferingCapTableRepository.Get(x=>x.InvestmentId == investment.InvestmentId);
                                if (capTableInvestment == null)
                                {
                                    OfferingCapTable offeringCapTable = new OfferingCapTable();
                                    offeringCapTable.OfferingId = investment.OfferingId;
                                    offeringCapTable.PercentageFunded = investment.FundedPercentage;
                                    offeringCapTable.PercentageOwnership = investment.OwnershipPercentage;
                                    offeringCapTable.InvestmentId = investment.InvestmentId;
                                    offeringCapTable.Status = 1;
                                    offeringCapTable.Active = true;
                                    offeringCapTable.CreatedBy = "Auto Generated";
                                    offeringCapTable.CreatedOn = DateTime.Now;

                                    offeringCapTables.Add(offeringCapTable);
                                }                                
                            }
                            _unitOfWork.OfferingCapTableRepository.InsertList(offeringCapTables);
                            contextData.SaveChanges();
                            transaction.Commit();
                            capTableDataDto.CapTableInvestments = (from capTable in contextData.OfferingCapTable
                                                                   join investment in contextData.Investment on capTable.InvestmentId equals investment.Id
                                                                   join userAccount in contextData.UserAccount on investment.UserId equals userAccount.Id
                                                                   join userProfile in contextData.UserProfile on investment.UserProfileId equals userProfile.Id
                                                                   join userProfileType in contextData.UserProfileType on userProfile.Type equals userProfileType.Id
                                                                   where capTable.OfferingId == offeringid
                                                                        && capTable.Status == 1 // Approved
                                                                        && capTable.Active == true
                                                                   select new CapTableInvestmentDto
                                                                   {
                                                                       Id = capTable.Id,
                                                                       InvestmentId = investment.Id,
                                                                       OfferingId = investment.OfferingId,
                                                                       UserId = investment.UserId,
                                                                       ProfileId = investment.UserProfileId,
                                                                       ProfileType = userProfile.Type,
                                                                       ProfileTypeName = userProfileType.Name,
                                                                       ProfileName = userProfile.FirstName + " " + userProfile.LastName + userProfile.Name + userProfile.RetirementPlanName + userProfile.TrustName,
                                                                       InvesterName = userAccount.FirstName + " " + userAccount.LastName,
                                                                       FundedAmount = investment.Amount,
                                                                       FundedPercentage = capTable.PercentageFunded,
                                                                       FundedDate = investment.FundedDate == null ? investment.CreatedOn : Convert.ToDateTime(investment.FundedDate),
                                                                       OwnershipPercentage = capTable.PercentageOwnership,
                                                                       PaymentMethod = userProfile.DistributionTypeId
                                                                   }).ToList();
                        }                        
                    }                  
                }                
                capTableDataDto.TotalPercentagaeFunded = (from captable in contextData.OfferingCapTable
                                                          where captable.OfferingId == offeringid
                                                          select captable.PercentageFunded).Sum();
                capTableDataDto.TotalPercentagaeOwnership = (from captable in contextData.OfferingCapTable
                                                          where captable.OfferingId == offeringid
                                                          select captable.PercentageOwnership).Sum();
                return capTableDataDto;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
            finally
            {
                contextData = null;
            }           
        }
        public bool AddOfferingDistributions(OfferingDistributionDto offeringDistributionDto)
        {
            var contextData = _unitOfWork.OfferingDistributionRepository.Context;
            try
            {
                int offeringDistributionId;
                using (var transaction = _unitOfWork.OfferingDistributionRepository.Context.Database.BeginTransaction())
                {                    
                    OfferingDistribution offeringDistribution = new OfferingDistribution();
                    offeringDistribution.OfferingId = offeringDistributionDto.OfferingId;
                    offeringDistribution.Amount= offeringDistributionDto.Amount;
                    offeringDistribution.CalculationMethod = offeringDistributionDto.CalculationMethod;
                    offeringDistribution.StartDate = offeringDistributionDto.StartDate;
                    offeringDistribution.EndDate = offeringDistributionDto.EndDate;
                    offeringDistribution.PaymentDate = offeringDistributionDto.PaymentDate;
                    offeringDistribution.Memo = offeringDistributionDto.Memo;
                    offeringDistribution.Type = offeringDistributionDto.Type; 
                    offeringDistribution.Status = 1;
                    offeringDistribution.Active = true;
                    offeringDistribution.CreatedBy = offeringDistributionDto.AdminUserId.ToString();
                    offeringDistribution.CreatedOn = DateTime.Now;                    

                    _unitOfWork.OfferingDistributionRepository.Insert(offeringDistribution);
                    contextData.SaveChanges();
                    transaction.Commit();
                    offeringDistributionId = offeringDistribution.Id;                                       
                }
                if(offeringDistributionId != 0)
                {
                    if (offeringDistributionDto.Distributions != null && offeringDistributionDto.Distributions.Count > 0)
                    {
                        using (var transaction = _unitOfWork.DistributionsRepository.Context.Database.BeginTransaction())
                        {
                            List<Distributions> distributions = new List<Distributions>();
                            foreach (var distributionDto in offeringDistributionDto.Distributions)
                            {
                                Distributions distributionsData = new Distributions();
                                distributionsData.OfferingDistributionId = offeringDistributionId;                               
                                distributionsData.InvestmentId = distributionDto.InvestmentId;
                                distributionsData.InvestorId = distributionDto.UserId;
                                distributionsData.StartDate = distributionDto.StartDate;
                                distributionsData.EndDate = distributionDto.EndDate;
                                distributionsData.PaymentDate = distributionDto.PaymentDate;
                                distributionsData.PaymentAmount = distributionDto.PaymentAmount;
                                distributionsData.Type = distributionDto.Type;
                                distributionsData.DistributionMethod = distributionDto.PaymentMethod;                                
                                distributionsData.PercentageFunded = distributionDto.PercentageFunded;
                                distributionsData.PercentageOwnership = distributionDto.PercentageOwnership;
                                distributionsData.Status = 1;
                                distributionsData.Active = true;
                                distributionsData.CreatedBy = offeringDistributionDto.AdminUserId.ToString();
                                distributionsData.CreatedOn = DateTime.Now;

                                distributions.Add(distributionsData);
                            }

                            _unitOfWork.DistributionsRepository.InsertList(distributions);
                            contextData.SaveChanges();
                            transaction.Commit();                            
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                e.ToString();
                return false;//Failure 
            }
            finally
            {
                contextData = null;
            }
        }
        public OfferingDistributionDto ConfirmDistributions(int offeringDistributionId, int adminuserid)
        {
            var contextData = _unitOfWork.OfferingDistributionRepository.Context;
            try
            {
                using (var transaction = _unitOfWork.OfferingDistributionRepository.Context.Database.BeginTransaction())
                {
                    var offeringDistributionData = _unitOfWork.OfferingDistributionRepository.Get(x => x.Id == offeringDistributionId && x.Active == true);

                    if (offeringDistributionData != null)
                    {                       
                        var distributions = (from distribution in contextData.Distributions
                                             where distribution.OfferingDistributionId == offeringDistributionId
                                             select distribution.Id).ToList();

                        if (distributions != null && distributions.Count > 0)
                        {
                            using (var distribtionTransaction = _unitOfWork.DistributionsRepository.Context.Database.BeginTransaction())
                            {
                                List<Distributions> distributionsDataList = new List<Distributions>();
                                foreach (int distributionId in distributions)
                                {
                                    var distributionData = _unitOfWork.DistributionsRepository.GetByID(distributionId);
                                    if (distributionData != null)
                                    {                                     
                                        distributionData.Status = 2;//Approved                                        
                                        distributionData.ModifiedBy = adminuserid.ToString();
                                        distributionData.ModifiedOn = DateTime.Now;

                                        distributionsDataList.Add(distributionData);
                                    }

                                    _unitOfWork.DistributionsRepository.UpdateList(distributionsDataList);
                                    contextData.SaveChanges();
                                    distribtionTransaction.Commit();
                                }
                            }

                            var distributionsData = (from offeringDistribution in contextData.OfferingDistribution
                                                 where offeringDistribution.Id == offeringDistributionId
                                                 select new OfferingDistributionDto
                                                 {
                                                     Id = offeringDistribution.Id,
                                                     OfferingId = offeringDistribution.OfferingId,
                                                     StartDate = offeringDistribution.StartDate,
                                                     EndDate = offeringDistribution.EndDate,
                                                     PaymentDate = offeringDistribution.PaymentDate,
                                                     Memo = offeringDistribution.Memo,
                                                     CalculationMethod = offeringDistribution.CalculationMethod,
                                                     Type = offeringDistribution.Type,
                                                     Amount = offeringDistribution.Amount,
                                                     Distributions = (from distribution in contextData.Distributions
                                                                      join investment in contextData.Investment on distribution.InvestmentId equals investment.Id
                                                                      join user in contextData.UserAccount on investment.UserId equals user.Id
                                                                      join profile in contextData.UserProfile on investment.UserProfileId equals profile.Id
                                                                      where distribution.OfferingDistributionId == offeringDistributionId
                                                                      select new DistributionsDataDto
                                                                      {
                                                                          Id = distribution.Id,
                                                                          OfferingDistributionId = distribution.OfferingDistributionId,
                                                                          OfferingId = investment.OfferingId,
                                                                          UserId = investment.UserId,
                                                                          InvestmentId = distribution.InvestmentId,
                                                                          InvestorName = user.FirstName + " " + user.LastName,
                                                                          FirstName = user.FirstName,
                                                                          LastName = user.LastName,
                                                                          ProfileId = investment.UserProfileId,
                                                                          ProfileType = profile.Type,
                                                                          ProfileName = profile.FirstName + " " + profile.LastName + profile.Name + profile.RetirementPlanName + profile.TrustName,
                                                                          PaymentAmount = distribution.PaymentAmount,
                                                                          PaymentMethod = distribution.DistributionMethod,
                                                                          StartDate = distribution.StartDate,
                                                                          EndDate = distribution.EndDate,
                                                                          PaymentDate = distribution.PaymentDate,
                                                                          PercentageFunded = distribution.PercentageFunded,
                                                                          PercentageOwnership = distribution.PercentageOwnership,
                                                                          Type = distribution.Type
                                                                      }).ToList()
                                                 }).FirstOrDefault();
                            return distributionsData;
                        }
                        return null;
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                e.ToString();
                throw;
            }
            finally
            {
                contextData = null;
            }
        }
        public List<OfferingDistributionDto> GetOfferingDistributions(int offeringid)
        {
            var contextData = _unitOfWork.OfferingDistributionRepository.Context;
            try
            {
                var distributions = (from offeringDistribution in contextData.OfferingDistribution
                                     where offeringDistribution.OfferingId == offeringid
                                        && offeringDistribution.Active == true
                                     select new OfferingDistributionDto
                                     { 
                                         Id = offeringDistribution.Id,                                         
                                         OfferingId = offeringDistribution.OfferingId,
                                         StartDate = offeringDistribution.StartDate,
                                         EndDate = offeringDistribution.EndDate,
                                         PaymentDate = offeringDistribution.PaymentDate,
                                         Memo = offeringDistribution.Memo,
                                         CalculationMethod = offeringDistribution.CalculationMethod,
                                         Type = offeringDistribution.Type,
                                         Amount = offeringDistribution.Amount,
                                         Distributions = (from distribution in contextData.Distributions
                                                          join investment in contextData.Investment on distribution.InvestmentId equals investment.Id
                                                          join user in contextData.UserAccount on investment.UserId equals user.Id
                                                          join profile in contextData.UserProfile on investment.UserProfileId equals profile.Id
                                                          where investment.OfferingId == offeringid
                                                          select new DistributionsDataDto
                                                          {
                                                              Id = distribution.Id,
                                                              OfferingDistributionId = distribution.OfferingDistributionId,
                                                              OfferingId = investment.OfferingId,
                                                              UserId = investment.UserId,
                                                              InvestmentId = distribution.InvestmentId,
                                                              InvestorName = user.FirstName + " " + user.LastName,
                                                              ProfileId = investment.UserProfileId,
                                                              ProfileType = profile.Type,
                                                              ProfileName = profile.FirstName + " " + profile.LastName + profile.Name + profile.RetirementPlanName + profile.TrustName,
                                                              PaymentAmount = distribution.PaymentAmount,
                                                              PaymentMethod = distribution.DistributionMethod,
                                                              StartDate = distribution.StartDate,
                                                              EndDate = distribution.EndDate,
                                                              PaymentDate = distribution.PaymentDate,
                                                              PercentageFunded = distribution.PercentageFunded,
                                                              PercentageOwnership = distribution.PercentageOwnership,
                                                              Type = distribution.Type
                                                          }).ToList() 
                                     }).ToList();
                return distributions;
            }
            catch(Exception e)
            {
                e.ToString();
                throw;
            }
            finally
            {
                contextData = null;
            }
        }
        public OfferingDistributionDto UpdateOfferingDistribution(OfferingDistributionDto offeringDistributionDto)
        {
            var contextData = _unitOfWork.OfferingDistributionRepository.Context;
            try
            {
                using (var transaction = _unitOfWork.OfferingDistributionRepository.Context.Database.BeginTransaction())
                {
                    var offeringDistributionData = _unitOfWork.OfferingDistributionRepository.Get(x => x.Id == offeringDistributionDto.Id && x.Active == true);

                    if (offeringDistributionData != null)
                    {
                        offeringDistributionData.Amount = offeringDistributionDto.Amount;
                        offeringDistributionData.CalculationMethod = offeringDistributionDto.CalculationMethod;
                        offeringDistributionData.Type = offeringDistributionDto.Type;
                        offeringDistributionData.StartDate = offeringDistributionDto.StartDate;
                        offeringDistributionData.EndDate = offeringDistributionDto.EndDate;
                        offeringDistributionData.PaymentDate = offeringDistributionDto.PaymentDate;
                        offeringDistributionData.Memo = offeringDistributionDto.Memo;
                        offeringDistributionData.ModifiedBy = offeringDistributionDto.AdminUserId.ToString();
                        offeringDistributionData.ModifiedOn = DateTime.Now;

                        _unitOfWork.OfferingDistributionRepository.Update(offeringDistributionData);
                        contextData.SaveChanges();
                        transaction.Commit();

                        if (offeringDistributionDto.Distributions != null && offeringDistributionDto.Distributions.Count > 0)
                        {
                            using (var distribtionTransaction = _unitOfWork.DistributionsRepository.Context.Database.BeginTransaction())
                            {
                                List<Distributions> distributionsDataList = new List<Distributions>();
                                foreach (var distributionDto in offeringDistributionDto.Distributions)
                                {
                                    var distributionsData = _unitOfWork.DistributionsRepository.GetByID(distributionDto.Id);
                                    if (distributionsData != null)
                                    {
                                        distributionsData.InvestmentId = distributionDto.InvestmentId;
                                        distributionsData.StartDate = distributionDto.StartDate;
                                        distributionsData.EndDate = distributionDto.EndDate;
                                        distributionsData.PaymentDate = distributionDto.PaymentDate;
                                        distributionsData.PaymentAmount = distributionDto.PaymentAmount;
                                        distributionsData.Type = distributionDto.Type;
                                        distributionsData.DistributionMethod = distributionDto.PaymentMethod;
                                        distributionsData.Status = 1;
                                        distributionsData.Active = true;
                                        distributionsData.ModifiedBy = offeringDistributionDto.AdminUserId.ToString();
                                        distributionsData.ModifiedOn = DateTime.Now;

                                        distributionsDataList.Add(distributionsData);
                                    }                                   
                                }
                                _unitOfWork.DistributionsRepository.UpdateList(distributionsDataList);
                                contextData.SaveChanges();
                                distribtionTransaction.Commit();
                            }

                            var distributions = (from offeringDistribution in contextData.OfferingDistribution
                                                 where offeringDistribution.Id == offeringDistributionDto.Id
                                                 select new OfferingDistributionDto
                                                 {
                                                     Id = offeringDistribution.Id,
                                                     OfferingId = offeringDistribution.OfferingId,
                                                     StartDate = offeringDistribution.StartDate,
                                                     EndDate = offeringDistribution.EndDate,
                                                     PaymentDate = offeringDistribution.PaymentDate,
                                                     Memo = offeringDistribution.Memo,
                                                     CalculationMethod = offeringDistribution.CalculationMethod,
                                                     Type = offeringDistribution.Type,
                                                     Amount = offeringDistribution.Amount,
                                                     Distributions = (from distribution in contextData.Distributions
                                                                      join investment in contextData.Investment on distribution.InvestmentId equals investment.Id
                                                                      join user in contextData.UserAccount on investment.UserId equals user.Id
                                                                      join profile in contextData.UserProfile on investment.UserProfileId equals profile.Id
                                                                      where distribution.OfferingDistributionId == offeringDistributionDto.Id
                                                                      select new DistributionsDataDto
                                                                      {
                                                                          Id = distribution.Id,
                                                                          OfferingDistributionId = distribution.OfferingDistributionId,
                                                                          OfferingId = investment.OfferingId,
                                                                          UserId = investment.UserId,
                                                                          InvestmentId = distribution.InvestmentId,
                                                                          InvestorName = user.FirstName + " " + user.LastName,
                                                                          ProfileId = investment.UserProfileId,
                                                                          ProfileType = profile.Type,
                                                                          ProfileName = profile.FirstName + " " + profile.LastName + profile.Name + profile.RetirementPlanName + profile.TrustName,
                                                                          PaymentAmount = distribution.PaymentAmount,
                                                                          PaymentMethod = distribution.DistributionMethod,
                                                                          StartDate = distribution.StartDate,
                                                                          EndDate = distribution.EndDate,
                                                                          PaymentDate = distribution.PaymentDate,
                                                                          PercentageFunded = distribution.PercentageFunded,
                                                                          PercentageOwnership = distribution.PercentageOwnership,
                                                                          Type = distribution.Type
                                                                      }).ToList()
                                                 }).FirstOrDefault();
                            return distributions;
                        }
                        return null;
                    }
                    return null;
                }               
            }
            catch (Exception e)
            {
                e.ToString();
                throw;
            }
            finally
            {
                contextData = null;
            }
        }
        public OfferingDistributionDto GetOfferingDistributionDetail(int offeringDistributionId)
        {
            var contextData = _unitOfWork.OfferingDistributionRepository.Context;
            try
            {
                var distributions = (from offeringDistribution in contextData.OfferingDistribution
                                     where offeringDistribution.Id == offeringDistributionId
                                     select new OfferingDistributionDto
                                     {
                                         OfferingId = offeringDistribution.OfferingId,
                                         StartDate = offeringDistribution.StartDate,
                                         EndDate = offeringDistribution.EndDate,
                                         PaymentDate = offeringDistribution.PaymentDate,
                                         Memo = offeringDistribution.Memo,
                                         CalculationMethod = offeringDistribution.CalculationMethod,
                                         Type = offeringDistribution.Type,
                                         Amount = offeringDistribution.Amount,
                                         Distributions = (from distribution in contextData.Distributions
                                                          join investment in contextData.Investment on distribution.InvestmentId equals investment.Id
                                                          join user in contextData.UserAccount on investment.UserId equals user.Id
                                                          join profile in contextData.UserProfile on investment.UserProfileId equals profile.Id
                                                          join distType in contextData.PortfolioDistributionType on distribution.Type equals distType.Id
                                                          where distribution.OfferingDistributionId == offeringDistributionId
                                                          select new DistributionsDataDto
                                                          {
                                                              Id = distribution.Id,
                                                              OfferingDistributionId = distribution.OfferingDistributionId,
                                                              OfferingId = investment.OfferingId,
                                                              UserId = investment.UserId,
                                                              InvestmentId = distribution.InvestmentId,
                                                              InvestorName = user.FirstName + " " + user.LastName,
                                                              ProfileId = investment.UserProfileId,
                                                              ProfileType = profile.Type,
                                                              ProfileName = profile.FirstName + " " + profile.LastName + profile.Name + profile.RetirementPlanName + profile.TrustName,
                                                              PaymentAmount = distribution.PaymentAmount,
                                                              PaymentMethod = distribution.DistributionMethod,
                                                               StartDate = distribution.StartDate,
                                                              EndDate = distribution.EndDate,
                                                              PaymentDate = distribution.PaymentDate,
                                                              PercentageFunded = distribution.PercentageFunded,
                                                              PercentageOwnership = distribution.PercentageOwnership,
                                                              Type = distribution.Type,
                                                              TypeName = distType.Name,
                                                              Memo = distribution.Memo
                                                          }).ToList(),
                                         TotalDistributions = (from distribution in contextData.Distributions
                                                               where distribution.OfferingDistributionId == offeringDistributionId
                                                               select distribution.PaymentAmount).Sum()                                    
            }).FirstOrDefault();
                return distributions;
            }
            catch (Exception e)
            {
                e.ToString();
                throw;
            }
            finally
            {
                contextData = null;
            }
        }
        public bool DeleteOfferingDistribution(int offeringDistributionId, int adminUserId)
        {
            var contextData = _unitOfWork.OfferingDistributionRepository.Context;
            try
            {
                using (var transaction = _unitOfWork.OfferingDistributionRepository.Context.Database.BeginTransaction())
                {
                    var offeringDistributionData = _unitOfWork.OfferingDistributionRepository.Get(x => x.Id == offeringDistributionId && x.Active == true);

                    if (offeringDistributionData != null)
                    {
                        offeringDistributionData.Active = false;
                        offeringDistributionData.ModifiedBy = adminUserId.ToString();
                        offeringDistributionData.ModifiedOn = DateTime.Now;

                        _unitOfWork.OfferingDistributionRepository.Update(offeringDistributionData);
                        contextData.SaveChanges();
                        transaction.Commit();

                        var distributions = (from distribution in contextData.Distributions
                                             where distribution.OfferingDistributionId == offeringDistributionId
                                             select distribution.Id).ToList();

                        if (distributions != null && distributions.Count > 0)
                        {
                            using (var distribtionTransaction = _unitOfWork.DistributionsRepository.Context.Database.BeginTransaction())
                            {
                                List<Distributions> distributionsDataList = new List<Distributions>();
                                foreach (int distributionId in distributions)
                                {
                                    var distributionData = _unitOfWork.DistributionsRepository.GetByID(distributionId);
                                    if (distributionData != null)
                                    {
                                        distributionData.Active = false;//Approved                                        
                                        distributionData.ModifiedBy = adminUserId.ToString();
                                        distributionData.ModifiedOn = DateTime.Now;

                                        distributionsDataList.Add(distributionData);
                                    }

                                    _unitOfWork.DistributionsRepository.UpdateList(distributionsDataList);
                                    contextData.SaveChanges();
                                    distribtionTransaction.Commit();
                                }
                            }
                           
                        }                       
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                e.ToString();
                throw;
            }
            finally
            {
                contextData = null;                
            }           
        }
        public List<CapTableInvestorDto> GetInvestors(int offeringId)
        {
            var contextData = _unitOfWork.InvestmentRepository.Context;
            try
            {
                var investors = (from investment in contextData.Investment
                                 where investment.OfferingId == offeringId
                                      && investment.Status == 1 // Approved
                                      && investment.Active == true
                                 select investment.UserId).ToList();
                var investorsData = (from userAccount in contextData.UserAccount
                                     join userProfile in contextData.UserProfile on userAccount.Id equals userProfile.UserId
                                     where userProfile.IsOwner == true && investors.Contains(userAccount.Id)
                                     select new CapTableInvestorDto
                                     {
                                         UserId = userAccount.Id,
                                         ProfileId = userProfile.Id,
                                         FirstName = userAccount.FirstName,
                                         LastName = userAccount.LastName,
                                         DistributionMethod = userProfile.DistributionTypeId
                                     }).ToList();

                                



               /* var investors = (from userAccount in contextData.UserAccount
                                 join investment in contextData.Investment on userAccount.Id equals investment.UserId into investmentdata
                                 from investment in investmentdata.DefaultIfEmpty()
                                 join userProfile in contextData.UserProfile on userAccount.Id equals userProfile.UserId into profiledata
                                 from userProfile in profiledata.DefaultIfEmpty()
                                 where investment.OfferingId == offeringId
                                      && investment.Status == 1 // Approved
                                      && investment.Active == true
                                      && userProfile.IsOwner == true
                                 select new CapTableInvestorDto
                                 {
                                     UserId = userAccount.Id,
                                     ProfileId = userProfile.Id,
                                     FirstName = userAccount.FirstName,
                                     LastName = userAccount.LastName,
                                     DistributionMethod = userProfile.DistributionTypeId
                                 }).ToList();*/
                return investorsData;
            }
            catch(Exception e)
            {
                e.ToString();
                return null;
            }
            finally
            {
                contextData = null;
            }
        }
    }   
}
