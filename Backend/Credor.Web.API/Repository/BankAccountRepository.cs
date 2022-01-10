using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Credor.Web.API.Common.UnitOfWork;
using Credor.Data.Entities;
using Credor.Client.Entities;
using Microsoft.AspNetCore.SignalR;
using Credor.Web.API.Common;
using Microsoft.Extensions.Configuration;

namespace Credor.Web.API
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly UnitOfWork _unitOfWork;
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public BankAccountRepository(IMapper mapper,INotificationRepository notificationRepository, 
            IHubContext<NotificationHub> hubContext,IConfiguration configuration)
        {
            _unitOfWork = new UnitOfWork(configuration);
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }
        public List<BankAccountType> GetBankAccountTypes()
        {
            try
            {
                var result = _unitOfWork.BankAccountTypeRepository.GetAll().ToList();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public List<BankAccountDto> GetAllBankAccounts(int userId)
        {
            List<BankAccountDto> bankAccountList = new List<BankAccountDto>();
            var contextData = _unitOfWork.BankAccountRepository.Context;
            try
            {
                bankAccountList = (from bankAccount in contextData.BankAccount
                                   join AT in contextData.BankAccountType on bankAccount.AccountType equals AT.Id
                                   where bankAccount.UserId == userId && bankAccount.Active == true
                                   select new BankAccountDto
                                   {
                                       Id = bankAccount.Id,
                                       UserId = bankAccount.UserId,
                                       BankName = bankAccount.BankName,
                                       AccountType = bankAccount.AccountType,
                                       AccountTypeName = AT.Type,
                                       RoutingNumber = bankAccount.RoutingNumber,
                                       AccountNumber = bankAccount.AccountNumber,
                                       Status = bankAccount.Status,
                                       Active = bankAccount.Active,
                                       CreatedBy = bankAccount.CreatedBy,
                                       CreatedOn = bankAccount.CreatedOn,
                                       ModifiedOn = bankAccount.ModifiedOn,
                                       ModifiedBy = bankAccount.ModifiedBy,
                                       ApprovedOn = bankAccount.ApprovedOn,
                                       ApprovedBy = bankAccount.ApprovedBy
                                   }).OrderBy(x => x.CreatedOn).ToList();

            }
            catch
            {
                bankAccountList = null;
            }
            finally
            {
                contextData = null;
            }
            return bankAccountList;
        }
        public BankAccountDto GetBankAccountById(int userId, int bankAccountId)
        {
            try
            {
                var contextData = _unitOfWork.BankAccountRepository.Context;

                var bankAccountData = (from bankAccount in contextData.BankAccount
                                       where bankAccount.Id == bankAccountId && bankAccount.UserId == userId && bankAccount.Active == true
                                       select new BankAccountDto
                                       {
                                           Id = bankAccount.Id,
                                           UserId = bankAccount.UserId,
                                           BankName = bankAccount.BankName,
                                           AccountType = bankAccount.AccountType,
                                           RoutingNumber = bankAccount.RoutingNumber,
                                           AccountNumber = bankAccount.AccountNumber,
                                           Status = bankAccount.Status,
                                           Active = bankAccount.Active,
                                           CreatedBy = bankAccount.CreatedBy,
                                           CreatedOn = bankAccount.CreatedOn,
                                           ModifiedOn = bankAccount.ModifiedOn,
                                           ModifiedBy = bankAccount.ModifiedBy,
                                           ApprovedOn = bankAccount.ApprovedOn,
                                           ApprovedBy = bankAccount.ApprovedBy
                                       }).FirstOrDefault();
                contextData = null;
                return bankAccountData;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int CreateBankAccount(BankAccountDto bankAccountDto)
        {            
            if (bankAccountDto.Id == 0)
            {
                try
                {
                    using (var transaction = _unitOfWork.BankAccountRepository.Context.Database.BeginTransaction())
                    {

                        BankAccount bankAccount = _mapper.Map<BankAccount>(bankAccountDto);
                        bankAccount.Active = true;
                        bankAccount.Status = 1;
                        bankAccount.CreatedBy = bankAccountDto.UserId.ToString();
                        bankAccount.CreatedOn = DateTime.Now;
                        _unitOfWork.BankAccountRepository.Insert(bankAccount);
                        _unitOfWork.Save();
                        transaction.Commit();

                        string messgae = "You added bank account " + bankAccount.BankName;
                        var notification = _notificationRepository.AddNotification(bankAccountDto.UserId, "Bank Account Added", messgae);
                        if (notification != null)
                        {
                            _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                        }
                    }                   
                    return 1; //Success
                }

                catch (Exception)
                {
                    return 0;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return 0;//Failure
        }
        public int UpdateBankAccount(BankAccountDto bankAccountDto)
        {
            var bankAccount = _unitOfWork.BankAccountRepository.Get(x => x.Id == bankAccountDto.Id);
            if (bankAccount.Id != 0)
            {
                try
                {
                    using (var transaction = _unitOfWork.BankAccountRepository.Context.Database.BeginTransaction())
                    {
                        BankAccount bankAccountData = bankAccount;
                        bankAccountData.RoutingNumber = bankAccountDto.RoutingNumber;
                        bankAccountData.AccountNumber = bankAccountDto.AccountNumber;
                        bankAccountData.AccountType = bankAccountDto.AccountType;
                        bankAccountData.BankName = bankAccountDto.BankName;                        
                        bankAccountData.ModifiedBy = bankAccountDto.UserId.ToString();
                        _unitOfWork.BankAccountRepository.Update(bankAccountData);
                        _unitOfWork.Save();
                        transaction.Commit();
                        string messgae = "You updated bank account " + bankAccount.BankName;
                        var notification = _notificationRepository.AddNotification(bankAccountDto.UserId, "Bank Account Updated", messgae);
                        if (notification != null)
                        {
                            _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                        }
                    }
                    return 1; //Success
                }

                catch (Exception e)
                {
                    var ex = e.ToString();
                    return 0;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return 0;//Failure
        }
        public int DeleteBankAccount(int userId, int  bankAccountId)
        {
            var bankAccount = _unitOfWork.BankAccountRepository.Get(x => x.Id == bankAccountId && x.UserId == userId);
            if (bankAccount.Id != 0)
            {
                try
                {
                    using (var transaction = _unitOfWork.BankAccountRepository.Context.Database.BeginTransaction())
                    {

                        BankAccount bankAccountData = bankAccount;
                        bankAccountData.ModifiedBy = userId.ToString();
                        bankAccountData.ModifiedOn = DateTime.Now;
                        bankAccountData.Active = false;
                        _unitOfWork.BankAccountRepository.Update(bankAccountData);
                        _unitOfWork.Save();
                        transaction.Commit();
                        string messgae = "You deleted bank account " + bankAccount.BankName;
                        var notification = _notificationRepository.AddNotification(bankAccount.UserId, "Bank Account Deleted", messgae);
                        if (notification != null)
                        {
                            _hubContext.Clients.All.SendAsync("Push_Notification", notification);
                        }
                    }
                    return 1; //Success
                }

                catch (Exception e)
                {
                    e.ToString();
                    return 0;//Failure
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
            else
                return 0;//Failure
        }
    }
}

