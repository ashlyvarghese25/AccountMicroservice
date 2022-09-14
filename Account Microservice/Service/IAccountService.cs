using Account_Microservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Microservice.Service
{
    public interface IAccountService
    {
        AccountCreationStatus AddAccount(int CustomerId, string AccountType);
        List<AccountViewModel> getAllAccounts(int CustomerId);
        Account getCustomerAccount(int AccountId);
        public bool isAccountThere(int Aid);
        List<Transactions> getStatement(int AccountId, DateTime from_date, DateTime to_date);
        TransactionStatus depositAccount(int AccountId, int amount);
        TransactionStatus withdrawAccount(int AccountId, int amount);
        IEnumerable<Account> getCustomersAllAccounts();
    }
}
