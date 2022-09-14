using Account_Microservice.Model;
using Account_Microservice.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Microservice.Service
{
    public class AccountService : IAccountService
    {
        readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AccountService));
        IAccountRepository _Repository;
        public AccountService(IAccountRepository Repository)
        {
            _Repository = Repository;
        }

       
        public AccountCreationStatus AddAccount(int CustomerId, string AccountType)
        {
            AccountCreationStatus acs = new AccountCreationStatus();

            acs = _Repository.AddAccount(CustomerId, AccountType);
            return acs;
        }


       
        public TransactionStatus depositAccount(int AccountId, int amount)
        {
            TransactionStatus ts = new TransactionStatus();
            try
            {
                ts = _Repository.depositAccount(AccountId, amount);

            }

            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
            return ts;
        }


        
        public List<AccountViewModel> getAllAccounts(int CustomerId)
        {
            List<Account> Listaccount = new List<Account>();
            List<AccountViewModel> accountViews = new List<AccountViewModel>();
            try
            {
                Listaccount = _Repository.getAllAccounts(CustomerId).ToList();

                AccountViewModel model;

                foreach (Account acc in Listaccount)
                {
                    model = new AccountViewModel();
                    model.Id = acc.AccountId;
                    model.Balance = acc.Balance;
                    model.AccountType = acc.AccountType;
                    accountViews.Add(model);
                }

            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }

            return accountViews;
        }


        
        public bool isAccountThere(int Aid)
        {
            bool res = _Repository.isAccountThere(Aid);
            return res;
        }
        public Account getCustomerAccount(int AccountId)
        {
            Account a = new Account();
            try
            {
                a = _Repository.getAccount(AccountId);

            }
            catch (Exception e)
            {
                throw e;
            }

            return a;
        }

        
        public List<Transactions> getStatement(int AccountId, DateTime from_date, DateTime to_date)
        {
            List<Transactions> transactions = new List<Transactions>();
            try
            {
                transactions = _Repository.getStatement(AccountId, from_date, to_date).ToList();

            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }

            return transactions;
        }

       
        public TransactionStatus withdrawAccount(int AccountId, int amount)
        {
            TransactionStatus ts = new TransactionStatus();
            try
            {
                ts = _Repository.withdrawAccount(AccountId, amount);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }

            return ts;
        }


       
        public IEnumerable<Account> getCustomersAllAccounts()
        {
            List<Account> accounts = new List<Account>();
            accounts = _Repository.getCustomersAllAccounts().ToList();
            try
            {
                if (accounts.Count == 0)
                {
                    throw new System.ArgumentNullException("nothing in the list");
                }
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }

            return accounts;
        }
    }
}
