using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credor.Data.Entities;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public interface IBankAccountService
    {
        public List<BankAccountType> GetBankAccountTypes();
        public List<BankAccountDto> GetAllBankAccounts(int userId);
        public BankAccountDto GetBankAccountById(int userId, int bankAccountId);
        public int CreateBankAccount(BankAccountDto bankAccountDto);
        public int UpdateBankAccount(BankAccountDto bankAccountDto);
        public int DeleteBankAccount(int userId, int bankAccountId);

    }
}
