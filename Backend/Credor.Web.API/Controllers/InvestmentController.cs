using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Web.API.Common.Filters;
using Credor.Client.Entities;

namespace Credor.Web.API.Controllers
{
    [Route("Investment")]
    [Produces("application/json")]
    [ApiController]
    public class InvestmentController : ControllerBase
    {
        private readonly IInvestmentService _investmentService;
        public IConfiguration _configuration { get; }

        public InvestmentController(IInvestmentService investmentService,
                                        IConfiguration configuration
                                        )
        {
            _configuration = configuration;
            _investmentService = investmentService;
        }

        [HttpGet]
        [Route("getofferingsandreservations")]       
        public IActionResult GetOfferingsAndReservations()
        {
            var activeOfferings = _investmentService.GetOfferingsAndReservations();
            return Ok(activeOfferings);
        }

        [HttpGet]
        [Route("getofferingsandreservations/{userid}")]
        public IActionResult GetOfferingsAndReservations(int userid)
        {
            var activeOfferings = _investmentService.GetOfferingsAndReservations(userid);
            return Ok(activeOfferings);
        }

        [HttpGet]
        [Route("getofferingbyid/{offeringId}")]
        public IActionResult GetOfferingById(int offeringId)
        {
            var activeOfferings = _investmentService.GetOfferingById(offeringId);
            return Ok(activeOfferings);
        }
        [HttpGet]
        [Route("getreservationbyid/{reservationId}")]
        public IActionResult GetReservationById(int reservationId)
        {
            var activeOfferings = _investmentService.GetReservationById(reservationId);
            return Ok(activeOfferings);
        }
        [HttpGet]
        [Route("getofferingdetailbyid/{id}")]
        public IActionResult GetOfferingDetailById(int id)
        {
            var activeOfferings = _investmentService.GetOfferingDetailById(id);
            return Ok(activeOfferings);
        }

        [HttpGet]
        [Route("getofferingvisibilty")]
        public IActionResult getOfferingVisibilty()
        {
            var visibilities = _investmentService.GetOfferingVisibilities();
            return Ok(visibilities);
        }

        [HttpPost]
        [Route("addinvestment")]
        public IActionResult AddInvestment([FromBody] InvestmentDto investment)
        {
            if (investment != null)
            {
                if (investment.IsReservation == false)
                {
                    var status = _investmentService.AddInvestment(investment);
                    if (status == 1)
                        return Ok(new { StatusCode = 1, Message = "Investment added" });
                    else
                        return Ok(new { StatusCode = 0, Message = "Failure creating investment" });
                }
                else
                    return Ok(new { StatusCode = 0, Message = "Investment only can be added" });
            }
            else
                return Ok(new { StatusCode = 0, Message = "Invalid Data" });
        }
        [HttpPost]
        [Route("addreservation")]
        public IActionResult AddReservation([FromBody] InvestmentDto reservation)
        {
            if(reservation!= null) 
            {
                if(reservation.IsReservation == true)
                {
                    var status = _investmentService.AddReservation(reservation);
                    if (status == 1)
                        return Ok(new { StatusCode = 1, Message = "Reservation added" });
                    else
                        return Ok(new { StatusCode = 0, Message = "Failure creating reservation" });
                }
                else
                    return Ok(new { StatusCode = 0, Message = "Reservation only can be added" });
            }  
            else
                return Ok(new { StatusCode = 0, Message = "Invalid Data" });
        }

        [HttpPut]
        [Route("updateinvestment")]
        public IActionResult UpdateInvestment([FromBody] InvestmentDto investment)
        {
            if (investment != null)
            {
                if (investment.IsReservation == false)
                {
                    var status = _investmentService.UpdateInvestment(investment);
                    if (status == 1)
                        return Ok(new { StatusCode = 1, Message = "Investment updated" });
                    else
                        return Ok(new { StatusCode = 0, Message = "Failure updating investment" });
                }
                else
                    return Ok(new { StatusCode = 0, Message = "Investment only can be updated" });
            }
            else
                return Ok(new { StatusCode = 0, Message = "Invalid Data" });
        }

        [HttpPut]
        [Route("updatereservation")]
        public IActionResult UpdateReservation([FromBody] InvestmentDto investment)
        {
            if (investment != null)
            {
                if (investment.IsReservation == true)
                {
                    var status = _investmentService.UpdateReservation(investment);
                    if (status == 1)
                        return Ok(new { StatusCode = 1, Message = "Reservation updated" });
                    else
                        return Ok(new { StatusCode = 0, Message = "Failure updating reservation" });
                }
                else
                    return Ok(new { StatusCode = 0, Message = "Reservation only can be updated" });
            }
            else
                return Ok(new { StatusCode = 0, Message = "Invalid Data" });
        }

        [HttpGet]
        [Route("getinvestmentdetailbyid/{userid}/{investmentid}")]
        public IActionResult getInvestmentDetailByUserId(int userid, int investmentid)
        {
            var list = _investmentService.GetInvestmentDetailById(userid, investmentid);
            return Ok(list);
        }

        [HttpGet]
        [Route("getreservationdetailbyid/{userid}")]
        public IActionResult getReservationDetailById(int userid)
        {
            var list = _investmentService.GetReservationDetailById(userid);
            return Ok(list);
        }
        [HttpGet]
        [Route("getportfoliofundinginstructions/{offeringid}")]
        public IActionResult GetPortfolioFundingInstructions(int offeringid)
        {
            var data = _investmentService.GetPortfolioFundingInstructions(offeringid);
            return Ok(data);
        }


    }
}
