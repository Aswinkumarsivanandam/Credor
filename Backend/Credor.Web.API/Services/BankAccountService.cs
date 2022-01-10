using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Data.Entities;
using Credor.Web.API.Common.UnitOfWork;
using Credor.Client.Entities;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class BankAccountService :IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly UnitOfWork _unitOfWork;
        public BankAccountService(IBankAccountRepository bankAccountRepository,
                                     IConfiguration configuration)
        {
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = new UnitOfWork(configuration);
        }
        public List<BankAccountType> GetBankAccountTypes()
        {
            return _bankAccountRepository.GetBankAccountTypes();
        }        
        public List<BankAccountDto> GetAllBankAccounts(int userId)
        {
            return _bankAccountRepository.GetAllBankAccounts(userId);
        }
        public BankAccountDto GetBankAccountById(int userId, int bankAccountId)
        {
            return _bankAccountRepository.GetBankAccountById(userId,bankAccountId);
        }
        public int CreateBankAccount(BankAccountDto bankAccountDto)
        {
            return _bankAccountRepository.CreateBankAccount(bankAccountDto);
        }
        public int UpdateBankAccount(BankAccountDto bankAccountDto)
        {
            return _bankAccountRepository.UpdateBankAccount(bankAccountDto);
        }
        public int DeleteBankAccount(int userId, int bankAccountId)
        {
            return _bankAccountRepository.DeleteBankAccount(userId, bankAccountId);
        }
    }
}
