using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Microservice.Model
{
    public class Transactions
    {
        public int transactionId { get; set; }
        public int accountId { get; set; }
        public int amount { get; set; }

        public int customerId { get; set; }

        public string message { get; set; }
        public int source_balance { get; set; }

        public int destination_balance { get; set; }

        public DateTime dateOfTransaction { get; set; }
    }
}
