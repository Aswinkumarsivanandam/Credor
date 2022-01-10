using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Credor.Web.API.Controllers
{
    [Route("MyInvestment")]
    [Produces("application/json")]
    [ApiController]
    public class MyInvestmentController : ControllerBase
    {
        private readonly IMyInvestmentService _myInvestmentService;
        public IConfiguration _configuration { get; set; }

        public MyInvestmentController(IMyInvestmentService myInvestmentService,
                                        IConfiguration configuration
                                        )
        {
            _configuration = configuration;
            _myInvestmentService = myInvestmentService;
        }
        [HttpGet]
        [Route("getheaderelements/{userid}")]
        public IActionResult GetHeaderElements(int userid)
        {
            var list = _myInvestmentService.GetHeaderElements(userid);
            return Ok(list);
        }

        [HttpGet]
        [Route("getinvestmentandreservationbyuserid/{userid}")]
        public IActionResult getInvestmentAndReservationsByUserId(int userid)
        {
            var list = _myInvestmentService.GetInvestmentAndReservationsByUserId(userid);
            return Ok(list);
        }
        [HttpGet]
        [Route("getinvestmentlistbyuserid/{userid}")]
        public IActionResult getInvestmentListByUserId(int userid)
        {
            var list = _myInvestmentService.GetInvestmentListByUserId(userid);
            return Ok(list);
        }
        [HttpGet]
        [Route("getReservationlistbyuserid/{userid}")]
        public IActionResult getReservationListByUserId(int userid)
        {
            var list = _myInvestmentService.GetReservationListByUserId(userid);
            return Ok(list);
        }
        [HttpGet]
        [Route("getinvestmentdetailbyid/{userid}/{investmentid}")]
        public IActionResult getInvestmentDetailByUserId(int userid, int investmentid)
        {
            var list = _myInvestmentService.GetInvestmentDetailById(userid, investmentid);
            return Ok(list);
        }
        [HttpGet]
        [Route("getreservationdetailbyid/{userid}/{reservationid}")]
        public IActionResult getReservationDetailById(int userid,int reservationid)
        {
            var list = _myInvestmentService.GetReservationDetailById(userid, reservationid);
            return Ok(list);
        }
        [HttpGet]
        [Route("getinvestmentstatuses")]
        public IActionResult GetInvestmentStatuses()
        {
            var statusList = _myInvestmentService.GetInvestmentStatuses();
            return Ok(statusList);
        }
    }
}
