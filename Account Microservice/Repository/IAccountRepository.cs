using Account_Microservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Microservice.Repository
{
    public interface IAccountRepository
    {
        AccountCreationStatus AddAccount(int CustomerId, string AccountType);
        IEnumerable<Account> getAllAccounts(int CustomerId);
        Account getAccount(int AccountId);
        List<Transactions> getStatement(int AccountId, DateTime from_date, DateTime to_date);
        TransactionStatus depositAccount(int AccountId, int amount);

        bool isAccountThere(int AccountId);
        TransactionStatus withdrawAccount(int AccountId, int amount);
        IEnumerable<Account> getCustomersAllAccounts();
    }
}
