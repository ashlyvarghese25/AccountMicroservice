using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Microservice.Model
{
    public class AccountStatement
    {
        public int AccountId { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
    }
}
