using Account_Microservice.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Account_Microservice.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public static DateTime d;
        public static int checkno = 700;
        readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AccountRepository));

        HttpClient _httpClient;

        public static List<Account> accounts = new List<Account>() {
            new Account(){AccountId=1,CustomerId=1,Balance=2000,AccountType="Savings",minBalance=1000 },
            new Account(){AccountId=2,CustomerId=1,Balance=1000,AccountType="Current",minBalance=0 },
            new Account(){AccountId=3,CustomerId=2,Balance=1500,AccountType="Savings",minBalance=1000 },
            new Account(){AccountId=4,CustomerId=2,Balance=1000,AccountType="Current",minBalance=0 }
        };

        public static List<Statement> statements = new List<Statement>() {
       new Statement(){ StatementId=1,AccountId=1,date=Convert.ToDateTime("2020-12-27"),refno="Ref75",ValueDate=Convert.ToDateTime("2020-12-27"),Withdrawal=0,Deposit=200,ClosingBalance=1200},
       new Statement(){ StatementId=2,AccountId=1,date=Convert.ToDateTime("2020-12-28"),refno="Ref76",ValueDate=Convert.ToDateTime("2020-12-28"),Withdrawal=100,Deposit=0,ClosingBalance=1100},
       new Statement(){ StatementId=3,AccountId=2,date=Convert.ToDateTime("2020-12-29"),refno="Ref77",ValueDate=Convert.ToDateTime("2020-12-29"),Withdrawal=0,Deposit=600,ClosingBalance=1600},
       new Statement(){ StatementId=4,AccountId=2,date=Convert.ToDateTime("2020-12-30"),refno="Ref78",ValueDate=Convert.ToDateTime("2020-12-30"),Withdrawal=200,Deposit=0,ClosingBalance=1400}
         };

        public AccountRepository()
        {
            _httpClient = new HttpClient();
        }



        
        public bool isAccountThere(int Aid)
        {
            foreach (var account in accounts)
            {
                if (account.AccountId == Aid)
                    return true;
            }
            return false;
        }
        public AccountCreationStatus AddAccount(int CId, string AType)
        {
            Account a = new Account();


            a.AccountId = accounts.Count + 1;
            a.CustomerId = CId;
            a.Balance = 1000;
            a.AccountType = AType;
            if (AType == "Savings")
            {
                a.minBalance = 1000;
            }
            else
            {
                a.minBalance = 0;
            }

            accounts.Add(a);

            return new AccountCreationStatus() { Message = "Account has been successfully created", AccountId = a.AccountId };
        }


       
        public TransactionStatus depositAccount(int AId, int amount)
        {
            //ResponseObj responseObj;
            int c = 0;
            int sbalance = 0, dbalance = 0;
            try
            {
                foreach (var item in accounts)
                {
                    if (item.AccountId == AId)
                    {
                        c = 1;
                        sbalance = item.Balance;
                        item.Balance = item.Balance + amount;
                        dbalance = item.Balance;

                        Statement s = new Statement();

                        s.StatementId = statements.Count + 1;
                        s.AccountId = AId;
                        d = DateTime.Now;
                        s.date = d;

                        checkno = checkno + 1;
                        s.refno = "Ref" + Convert.ToString(checkno);
                        s.ValueDate = d;
                        s.Withdrawal = 0;
                        s.Deposit = amount;
                        s.ClosingBalance = dbalance;
                        statements.Add(s);

                        d = s.date;
                        break;
                    }
                }

                if (c == 1)
                {
                    return new TransactionStatus() { Message = "Your account has been credited", source_balance = sbalance, destination_balance = dbalance };

                }
                else
                {
                    throw new System.ArgumentNullException("Account id is invalid " + AId);

                }
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }


        }




        
        public IEnumerable<Account> getAllAccounts(int CId)
        {

            List<Account> li = new List<Account>();
            try
            {
                foreach (var item in accounts)
                {
                    if (item.CustomerId == CId)
                    {
                        li.Add(item);
                    }
                }
                if (li.Count == 0)
                {
                    throw new System.ArgumentNullException("nothing in the list for this customer id  " + CId);
                }
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }

            return li;
        }

        
        public Account getAccount(int AId)
        {

            Account a = new Account();
            try
            {
                foreach (var item in accounts)
                {
                    if (item.AccountId == AId)
                    {

                        a = item;
                    }
                }

                if (a.AccountId == 0)
                {
                    throw new System.ArgumentNullException("No such accunt is stored");
                }
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }

            return a;
        }


        
        public List<Transactions> getStatement(int AId, DateTime from_date, DateTime to_date)
        {
            string base_uri = "https://localhost:44386/api/Transaction/getTransactions/" + AId;
            HttpResponseMessage response = _httpClient.GetAsync(base_uri).Result;
            List<Transactions> transactions = new List<Transactions>();
            List<Transactions> displayTransactions = new List<Transactions>();


            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                transactions = JsonConvert.DeserializeObject<List<Transactions>>(data);

            }
            Console.WriteLine(transactions);


            foreach (var trans in transactions)
            {
                if (DateTime.Compare(from_date, trans.dateOfTransaction) <= 0 &&
                    DateTime.Compare(trans.dateOfTransaction, to_date) <= 0)
                {
                    displayTransactions.Add(trans);
                }

            }




            return displayTransactions;
        }


       
        public TransactionStatus withdrawAccount(int AId, int amount)
        {

            int c = 0;
            int sbalance = 0, dbalance = 0;
            try
            {
                foreach (var item in accounts)
                {
                    if (item.AccountId == AId)
                    {
                        c = 1;
                        sbalance = item.Balance;
                        item.Balance = item.Balance - amount;
                        dbalance = item.Balance;

                        Statement s = new Statement();

                        s.StatementId = statements.Count + 1;
                        s.AccountId = AId;
                        d = DateTime.Now;
                        s.date = d;
                        checkno = checkno + 1;
                        s.refno = "Ref" + Convert.ToString(checkno);
                        s.ValueDate = d;
                        s.Withdrawal = amount;
                        s.Deposit = 0;
                        s.ClosingBalance = dbalance;
                        statements.Add(s);

                        d = s.date;
                        break;
                    }
                }

                if (c == 1)
                {
                    return new TransactionStatus() { Message = "Your account has been debited", source_balance = sbalance, destination_balance = dbalance };

                }
                else
                {

                    throw new System.ArgumentNullException("Account id is invalid " + AId);
                }
            }

            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
        }



        public IEnumerable<Account> getCustomersAllAccounts()
        {
            return accounts;
        }

        
    }
}
