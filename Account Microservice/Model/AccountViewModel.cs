using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Microservice.Model
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string AccountType { get; set; }
        public double Balance { get; set; }
    }
}
