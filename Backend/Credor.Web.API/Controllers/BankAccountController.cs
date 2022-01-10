using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Client.Entities;

namespace Credor.Web.API.Controllers
{
    [Route("BankAccount")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;
        public IConfiguration _configuration { get; }

        public BankAccountController(IBankAccountService bankAccountService,
                                        IConfiguration configuration
                                        )
        {
            _configuration = configuration;
            _bankAccountService = bankAccountService;
        }
        [HttpGet]
        [Route("getbankaccounttypes")]
        public IActionResult GetBankAcountTypes()
        {
            var bankAccountTypes = _bankAccountService.GetBankAccountTypes();
            return Ok(bankAccountTypes);
        }
        [HttpPost]
        [Route("createbankaccount")]
        public IActionResult CreateBankAccount([FromBody]BankAccountDto bankAccount)
        {
            var result = _bankAccountService.CreateBankAccount(bankAccount);
            return Ok(result);
        }
        [HttpPut]
        [Route("updatebankaccount")]
        public IActionResult UpdateBankAccount([FromBody]BankAccountDto bankAccount)
        {
            var result = _bankAccountService.UpdateBankAccount(bankAccount);
            return Ok(result);
        }
        [HttpDelete]
        [Route("deletebankaccount/{userid}/{bankaccountid}")]
        public IActionResult DeleteBankAccount(int userid, int bankaccountid)
        {
            var result = _bankAccountService.DeleteBankAccount(userid,bankaccountid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getbankaccountbyid/{userid}/{bankaccountid}")]
        public IActionResult Getbankaccountbyid(int userid, int bankaccountid)
        {
            var result = _bankAccountService.GetBankAccountById(userid, bankaccountid);
            return Ok(result);
        }
        [HttpGet]
        [Route("getallbankaccounts/{userid}")]
        public IActionResult GetAllUserProfile(int userid)
        {
            var result = _bankAccountService.GetAllBankAccounts(userid);
            return Ok(result);
        }


    }
}
