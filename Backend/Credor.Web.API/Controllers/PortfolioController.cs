using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;
using ClosedXML.Excel;
using System.IO;

namespace Credor.Web.API.Controllers
{
    [Route("portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IInvestorService _investorService;
        public IConfiguration _configuration { get; }
        public PortfolioController(IPortfolioService portfolioService,   
                                   IInvestorService investorService,
                                   IConfiguration configuration)
        {
            _portfolioService = portfolioService;
            _investorService = investorService;
            _configuration = configuration;

        }
        [HttpGet]
        [Route("getportfolioofferings")]
        public IActionResult GetPortfolioOfferings()
        {
            var result = _portfolioService.GetPortfolioOfferings();
            return Ok(result);
        }
        [HttpGet]
        [Route("getportfolioreservations")]
        public IActionResult GetPortfolioReservations()
        {
            var result = _portfolioService.GetPortfolioReservations();
            return Ok(result);
        }
        [HttpGet]
        [Route("getportfoliooffering/{offeringid}")]
        public IActionResult GetPortfolioOfferings(int offeringid)
        {
            var result = _portfolioService.GetPortfolioOffering(offeringid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getportfolioreservation/{reservationid}")]
        public IActionResult GetPortfolioReservations(int reservationid)
        {
            var result = _portfolioService.GetPortfolioReservation(reservationid);
            return Ok(result);
        }
        [HttpPost]
        [Route("addportfoliooffering")]
        public IActionResult AddPortfolioOffering(PortfolioOfferingDto portfolioOffering)
        {
            var result = _portfolioService.AddPortfolioOffering(portfolioOffering);
            return Ok(result);
        }
        [HttpPost]
        [Route("addportfolioreservation")]
        public IActionResult AddPortfolioReservation(PortfolioOfferingDto portfolioReservation)
        {
            var result = _portfolioService.AddPortfolioReservation(portfolioReservation);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateportfoliooffering")]
        public IActionResult UpdatePortfolioOffering(PortfolioOfferingDto portfolioOffering)
        {
            var result = _portfolioService.UpdatePortfolioOffering(portfolioOffering);
            return Ok(result);
        }
        [HttpPost]
        [Route("updateportfolioreservation")]
        public IActionResult UpdatePortfolioReservation(PortfolioOfferingDto portfolioReservation)
        {
            var result = _portfolioService.UpdatePortfolioReservation(portfolioReservation);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deleteportfoliooffering/{adminuserid}/{offeringid}")]
        public IActionResult DeletePortfolioOffering(int adminuserid, int offeringid)
        {
            var result = _portfolioService.DeletePortfolioOffering(adminuserid,offeringid);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deleteportfolioreservation/{adminuserid}/{reservationid}")]
        public IActionResult DeletePortfolioReservation(int adminuserid,int reservationid)
        {
            var result = _portfolioService.DeletePortfolioReservation(adminuserid, reservationid);
            return Ok(result);
        }
        [HttpPut]
        [Route("restoreportfoliooffering/{adminuserid}/{offeringid}")]
        public IActionResult RestorePortfolioOffering(int adminuserid, int offeringid)
        {
            var result = _portfolioService.RestorePortfolioOffering(adminuserid, offeringid);
            return Ok(result);
        }
        [HttpPut]
        [Route("restoreportfolioreservation/{adminuserid}/{reservationid}")]
        public IActionResult RestorePortfolioReservation(int adminuserid, int reservationid)
        {
            var result = _portfolioService.RestorePortfolioReservation(adminuserid, reservationid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getarchivedportfolioofferings")]
        public IActionResult GetArchivedPortfolioOfferings()
        {
            var result = _portfolioService.GetArchivedPortfolioOfferings();
            return Ok(result);
        }
        [HttpGet]
        [Route("getarchivedportfolioofferingandreservations")]
        public IActionResult GetArchivedPortfolioOfferingAndReservations()
        {
            var result = _portfolioService.GetArchivedPortfolioOfferingAndReservations();
            return Ok(result);
        }
        [HttpPost]
        [Route("uploadportfoliogallery")]
        public async Task<IActionResult> AddPortfolioGalleryImages([FromForm] AddPortfolioGalleryDto galleryDto)
        {
            var result = await _portfolioService.AddPortfolioGalleryImages(galleryDto);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateportfoliogallery")]
        public IActionResult UpdatePortfolioGalleryImage([FromBody] PortfolioGalleryDto galleryDto)
        {
            var result =  _portfolioService.UpdatePortfolioGalleryImage(galleryDto);
            return Ok(result);
        }
        [HttpPost]
        [Route("addportfoliosummary")]
        public IActionResult AddPortfolioSummary([FromBody] PortfolioSummaryDto portfolioSummary)
        {
            var result =  _portfolioService.AddPortfolioSummary(portfolioSummary);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateportfoliosummary")]
        public IActionResult UpdatePortfolioSummary([FromBody] PortfolioSummaryDto portfolioSummary)
        {
            var result = _portfolioService.UpdatePortfolioSummary(portfolioSummary);
            return Ok(result);
        }
        [HttpPost]
        [Route("uploadofferingdocuments")]
        public async Task<IActionResult> UploadOfferingDocuments([FromForm] DocumentModelDto documents)
        {
            var result = await _portfolioService.UploadOfferingDocuments(documents);            
            return Ok(result);
        }
        [HttpDelete]
        [Route("deleteofferingdocument/{adminuserid}/{offeringid}/{documentid}")]
        public IActionResult DeleteOfferingDocument(int adminuserid, int offeringid, int documentid)
        {
            var updates = _portfolioService.DeleteOfferingDocument(adminuserid,offeringid, documentid);
            return Ok(updates);
        }
        [HttpPost]
        [Route("adddefaultportfoliokeyhighlights/{id}/{adminuserid}")]
        public IActionResult AddDefaultPortfolioKeyHighlights(int id,int adminuserid)
        {
            var result = _portfolioService.AddDefaultPortfolioKeyHighlights(id,adminuserid);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateportfoliokeyhightlight")]
        public IActionResult UpdatePortfolioKeyHightlight([FromBody] UpdatePortfolioKeyHightlightDto updatePortfolioKeyHightlight)
        {
            var result = _portfolioService.UpdatePortfolioKeyHightlight(updatePortfolioKeyHightlight);
            return Ok(result);
        }
        [HttpPost]
        [Route("addportfoliolocation")]
        public IActionResult AddPortfolioLocation([FromBody] PortfolioLocationDto portfolioLocation)
        {
            var result = _portfolioService.AddPortfolioLocation(portfolioLocation);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateportfoliolocation")]
        public IActionResult UpdatePortfolioLocation([FromBody] PortfolioLocationDto portfolioLocation)
        {
            var result = _portfolioService.UpdatePortfolioLocation(portfolioLocation);
            return Ok();
        }
        [HttpPost]
        [Route("addportfoliofundinginstructions")]
        public IActionResult AddPortfolioFundingInstructions([FromBody] PortfolioFundingInstructionsDto portfolioFundingInstructions)
        {
            var result = _portfolioService.AddPortfolioFundingInstructions(portfolioFundingInstructions);
            return Ok(result);
        }
        [HttpPut]
        [Route("updateportfoliofundinginstructions")]
        public IActionResult UpdatePortfolioFundingInstructions([FromBody] PortfolioFundingInstructionsDto portfolioFundingInstructions)
        {
            var result = _portfolioService.UpdatePortfolioFundingInstructions(portfolioFundingInstructions);
            return Ok(result);
        }
        [HttpGet]
        [Route("getportfolioinvestorssummary/{offeringid}")]
        public IActionResult GetPortfolioInvestorsSummary(int offeringid)
        {
            var result = _portfolioService.GetPortfolioInvestorsSummary(offeringid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getportfolioofferinginvestments/{offeringid}")]
        public IActionResult GetPortfolioOfferingInvestments(int offeringid)
        {
            var result = _portfolioService.GetPortfolioOfferingInvestments(offeringid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getportfolioreservationssummary/{offeringid}")]
        public IActionResult GetPortfolioReservationsSummary(int offeringid)
        {
            var result = _portfolioService.GetPortfolioReservationsSummary(offeringid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getportfolioofferingReservations/{offeringid}")]
        public IActionResult GetPortfolioOfferingReservations(int offeringid)
        {
            var result = _portfolioService.GetPortfolioOfferingReservations(offeringid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getreservationslist/{reservationid}")]
        public IActionResult GetReservationsList(int reservationid)
        {
            var result = _portfolioService.GetReservationsList(reservationid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getreservationssummary/{reservationid}")]
        public IActionResult GetReservationsSummary(int reservationid)
        {
            var result = _portfolioService.GetReservationsSummary(reservationid);
            return Ok(result);
        }
        [HttpPost]
        [Route("addinvestment")]
        public IActionResult AddInvestment([FromForm] InvestmentDataDto investmentDataDto)
        {
            var status = _investorService.AddInvestment(investmentDataDto);
            return Ok(status);
        }
        [HttpPut]
        [Route("updateinvestment")]
        public IActionResult UpdateInvestment([FromForm] InvestmentDataDto investmentDataDto)
        {
            var status = _investorService.UpdateInvestment(investmentDataDto);
            return Ok(status);
        }
        [HttpPut]
        [Route("addinvestmentnotes")]
        public IActionResult AddInvestmentNotes([FromBody] InvestmentNotesDto investmentNotesDto)
        {
            var status = _portfolioService.AddInvestmentNotes(investmentNotesDto);
            return Ok(status);
        }
        [HttpPut]
        [Route("addreservationnotes")]
        public IActionResult AddReservationNotes([FromBody] ReservationNotesDto reservationNotes)
        {
            var status = _portfolioService.AddReservationNotes(reservationNotes);
            return Ok(status);
        }
        [HttpDelete]
        [Route("deleteinvestment/{adminuserid}/{reservationid}")]
        public IActionResult DeleteInvestment(int adminuserid, int reservationid)
        {
            var status = _investorService.DeleteInvestment(adminuserid, reservationid);
            return Ok(status);
        }
        [HttpPost]
        [Route("addreservation")]
        public IActionResult AddReservation(ReservationDataDto reservationDataDto)
        {
            var status = _investorService.AddReservation(reservationDataDto);
            return Ok(status);
        }
        [HttpPut]
        [Route("updatereservation")]
        public IActionResult UpdateReservation(ReservationDataDto reservationDataDto)
        {
            var status = _investorService.UpdateReservation(reservationDataDto);
            return Ok(status);
        }
        [HttpDelete]
        [Route("deletereservation/{adminuserid}/{reservationid}")]
        public IActionResult DeleteReservation(int adminuserid, int reservationid)
        {
            var status = _investorService.DeleteReservation(adminuserid, reservationid);
            return Ok(status);
        }
        [HttpGet]
        [Route("getinvestmentstatuses")]
        public IActionResult GetInvestmentStatuses()
        {
            var statusList = _portfolioService.GetInvestmentStatuses();
            return Ok(statusList);
        }
        [HttpGet]
        [Route("getportfoliodistributiontypes")]
        public IActionResult GetPortfolioDistributionTypes()
        {
            var statusList = _portfolioService.GetPortfolioDistributionTypes();
            return Ok(statusList);
        }
        [HttpGet]
        [Route("getportfolioofferingcaptable/{offeringid}")]
        public IActionResult GetPortfolioOfferingCapTable(int offeringid)
        {
            var capTableData = _portfolioService.GetPortfolioOfferingCapTable(offeringid);
            return Ok(capTableData);
        }
        [HttpPost]
        [Route("addofferingdistributions")]
        public IActionResult AddOfferingDistributions([FromBody] OfferingDistributionDto distributionsData)
        {
            var statusList = _portfolioService.AddOfferingDistributions(distributionsData);
            return Ok();
        }
        [HttpPut]
        [Route("confirmdistributions/{offeringdistributionid}/{adminuserid}")]
        public IActionResult ConfirmDistributions(int offeringdistributionid, int adminuserid)
        {
            var result = _portfolioService.ConfirmDistributions(offeringdistributionid, adminuserid);
            return Ok();
        }
        [HttpGet]
        [Route("getofferingdistributions/{offeringid}")]
        public IActionResult GetOfferingDistributions(int offeringid)
        {
            var capTableData = _portfolioService.GetOfferingDistributions(offeringid);
            return Ok(capTableData);
        }
        [HttpGet]
        [Route("getinvestors/{offeringid}")]
        public IActionResult GetInvestors(int offeringid)
        {
            var capTableData = _portfolioService.GetInvestors(offeringid);
            return Ok(capTableData);
        }
        [HttpGet]
        [Route("getofferingdistributiondetail/{offeringdistributionid}")]
        public IActionResult GetOfferingDistributionDetail(int offeringdistributionid)
        {
            var capTableData = _portfolioService.GetOfferingDistributionDetail(offeringdistributionid);
            return Ok(capTableData);
        }
        [HttpPut]
        [Route("updateofferingdistribution")]
        public IActionResult UpdateOfferingDistribution([FromBody] OfferingDistributionDto offeringDistributionDto)
        {
            var status = _portfolioService.UpdateOfferingDistribution(offeringDistributionDto);
            return Ok(status);
        }
        [HttpDelete]
        [Route("deleteofferingdistribution/{offeringdistributionid}/{adminuserid}")]
        public IActionResult DeleteOfferingDistribution(int offeringdistributionid, int adminuserid)
        {
            var status = _portfolioService.DeleteOfferingDistribution(offeringdistributionid, adminuserid);
            return Ok(status);
        }
        [HttpPut]
        [Route("updatecaptable")]
        public IActionResult UpdateCapTable([FromBody] UpdateCapTableDto updateCapTable)
        {
            var status = _portfolioService.UpdateCapTable(updateCapTable);
            return Ok(status);
        }
        [HttpPut]
        [Route("exportofferingcaptable/{offeringid}")]
        public IActionResult ExportOfferingCapTable(int offeringid)
        {
            try
            {
                var capTable = _portfolioService.GetPortfolioOfferingCapTable(offeringid);
                if (capTable != null && capTable.CapTableInvestments != null)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("CapTable");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = "Id";
                        worksheet.Cell(currentRow, 2).Value = "Investor Name";
                        worksheet.Cell(currentRow, 3).Value = "Profile Type";
                        worksheet.Cell(currentRow, 4).Value = "Profile Name";
                        worksheet.Cell(currentRow, 5).Value = "Percentage Funded";
                        worksheet.Cell(currentRow, 6).Value = "Percentage Ownership";
                        worksheet.Cell(currentRow, 7).Value = "Payment Amount";
                        worksheet.Cell(currentRow, 8).Value = "Funded Date";
                        foreach (var investment in capTable.CapTableInvestments)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = investment.Id;
                            worksheet.Cell(currentRow, 2).Value = investment.InvesterName;
                            worksheet.Cell(currentRow, 3).Value = investment.ProfileTypeName;
                            worksheet.Cell(currentRow, 4).Value = investment.ProfileName;
                            worksheet.Cell(currentRow, 5).Value = investment.FundedPercentage;
                            worksheet.Cell(currentRow, 6).Value = investment.OwnershipPercentage;
                            worksheet.Cell(currentRow, 7).Value = investment.FundedAmount;
                            worksheet.Cell(currentRow, 8).Value = investment.FundedDate;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "CapTableData.xlsx");
                        }
                    }
                }
                else
                    return Ok(new { StatusCode = 0, Message = "No Distributions Found" });
            }
            catch (Exception e)
            {
                e.ToString();
                return Ok(new { StatusCode = 1, Message = "Error creating file" });
            }
        }
        [HttpPut]
        [Route("exportofferingdistributions/{offeringdistributionid}")]
        public IActionResult ExportOfferingDistributions(int offeringdistributionid)
        {
            try
            {
                var offeringDistributions = _portfolioService.GetOfferingDistributionDetail(offeringdistributionid);
                if (offeringDistributions != null && offeringDistributions.Distributions.Count > 0)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Distributions");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = "Id";
                        worksheet.Cell(currentRow, 2).Value = "Investor Name";
                        worksheet.Cell(currentRow, 3).Value = "Profile Type";
                        worksheet.Cell(currentRow, 4).Value = "Profile Name";
                        worksheet.Cell(currentRow, 5).Value = "Percentage Funded";
                        worksheet.Cell(currentRow, 6).Value = "Percentage Ownership";
                        worksheet.Cell(currentRow, 7).Value = "Funded";
                        worksheet.Cell(currentRow, 8).Value = "Funded Date";
                        foreach (var distribution in offeringDistributions.Distributions)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = distribution.Id;                            
                            worksheet.Cell(currentRow, 2).Value = distribution.InvestorName;
                            worksheet.Cell(currentRow, 3).Value = distribution.ProfileType;
                            worksheet.Cell(currentRow, 4).Value = distribution.ProfileName;
                            worksheet.Cell(currentRow, 5).Value = distribution.PercentageFunded;
                            worksheet.Cell(currentRow, 6).Value = distribution.PercentageOwnership;
                            worksheet.Cell(currentRow, 7).Value = distribution.PaymentAmount;
                            worksheet.Cell(currentRow, 8).Value = distribution.PaymentDate;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "CapTableData.xlsx");
                        }
                    }
                }
                else
                    return Ok(new { StatusCode = 0, Message = "No Distributions Found" });
            }
            catch (Exception e)
            {
                e.ToString();
                return Ok(new { StatusCode = 1, Message = "Error creating file" });
            }
        }
        [HttpPut]
        [Route("importdistributions")]
        public IActionResult ImportDistributions([FromForm] FormFile capTableFile)
        {
            //var status = _portfolioService.UpdateOfferingDistribution(offeringDistributionDto);
            return Ok();
        }
    }
}
