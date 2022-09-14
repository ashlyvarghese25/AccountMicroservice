using Account_Microservice.Model;
using Account_Microservice.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AccountController));
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

       

        [HttpPost("createAccount")]
        public IActionResult createAccount([FromBody] dynamic obj)
        {
            ResponseObj responseObj = new ResponseObj();
            if (Convert.ToInt32(obj) == 0)
            {
                _log4net.Warn("User has sent some invalid CustomerId " + Convert.ToInt32(obj));
                responseObj.isSuccess = false;
                responseObj.resMessage = "User has sent some invalid CustomerId";
                responseObj.resObject = null;



                return NotFound(responseObj);
            }
            try
            {
                AccountCreationStatus acs, acs1 = new AccountCreationStatus();


                acs = _service.AddAccount(Convert.ToInt32(obj), "Savings");
                _log4net.Info("Savings account has been successfully created");
                acs1 = _service.AddAccount(Convert.ToInt32(obj), "Current");
                _log4net.Info("Current account has been successfully created");
                responseObj.isSuccess = true;
                responseObj.resMessage = "Current account has been successfully created";
                //  responseObj.resMessage = "Saving account has been successfully created";
                responseObj.resObject = acs1;

                return Ok(responseObj);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                responseObj.isSuccess = false;
                responseObj.resMessage = "User already exists";
                responseObj.resObject = null;
                return BadRequest(responseObj);
            }
        }

       
        [HttpGet]
        [Route("getCustomerAccounts/{CustomerId}")]
        public IActionResult getCustomerAccounts(int CustomerId)
        {
            if (CustomerId == 0)
            {
                _log4net.Warn("invalid accountid " + CustomerId);
                return NotFound();
            }
            try
            {

                var Listaccount = _service.getAllAccounts(CustomerId);
                _log4net.Info("Customer's account has been successfully returned");
                return Ok(Listaccount);

            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
        }
        
        [HttpGet]
        [Route("getAllCustomerAccounts")]
        public IActionResult getAllCustomerAccounts()
        {
            List<Account> Listaccount = new List<Account>();
            try
            {
                Listaccount = _service.getCustomersAllAccounts().ToList();

            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }

            _log4net.Info("Customer's account has been successfully returned");
            return Ok(Listaccount);
        }


       
        [HttpGet]
        [Route("getAccount/{AccountId}")]
        public IActionResult getAccount(int AccountId)
        {
            if (AccountId == 0)
            {
                _log4net.Warn("invalid accountid " + AccountId);
                return NotFound();

            }
            try
            {
                Account a = new Account();
                a = _service.getCustomerAccount(AccountId);
                _log4net.Info("successfully returned acccount model");
                return Ok(a);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                return NotFound(new { status = 404, reqMessage = "Customer ID does not exist" });

            }

        }


       
        [HttpPost("getAccountStatement")]
        //[Route("getAccountStatement")]
        public IActionResult getAccountStatement([FromBody] AccountStatement obj)
        {
            bool flag = _service.isAccountThere(obj.AccountId);
            if (Convert.ToInt32(obj.AccountId) == 0 || !flag)
            {
                _log4net.Warn("invalid accountid : " + Convert.ToInt32(obj.AccountId));
                return NotFound(new { status = 404, isSuccess = false, resMessage = "Couldn't find account, enter valid ID" });
            }
            try
            {
                List<Transactions> statements = new List<Transactions>();





                statements = _service.getStatement(Convert.ToInt32(obj.AccountId), Convert.ToDateTime(obj.from_date), Convert.ToDateTime(obj.to_date));

                if (statements.Count > 0)
                    return Ok(new { isSuccess = true, resObject = statements });
                else
                    return NotFound(new { status = 404, isSuccess = false, resMessage = "Couldn't find transactions during this time period" });
                _log4net.Info("statement returned for given accountid");


            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
        }



        
        [HttpPost("deposit")]
        public IActionResult deposit([FromBody] dynamic obj)
        {
            if (Convert.ToInt32(obj.accountId) == 0 || Convert.ToInt32(obj.amount) == 0)
            {
                _log4net.Warn("invalid accountid or amount 0 for id: " + Convert.ToInt32(obj.accountId));
                return NotFound(new { isSuccess = false, resMessage = "Invalid account id" });
            }

            try
            {
                TransactionStatus ts = new TransactionStatus();
                ts = _service.depositAccount(Convert.ToInt32(obj.accountId), Convert.ToInt32(obj.amount));
                _log4net.Info("account has been credited successfully");
                //   return Ok(new { resObject = ts , isSuccess=true});
                return Ok(ts);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                return NotFound(new { isSuccess = false, resMessage = "Invalid account ID or Insufficient balance" });
            }

        }

       
        [HttpPost("withdraw")]
        public IActionResult withdraw([FromBody] dynamic obj)
        {

            if (Convert.ToInt32(obj.accountId) == 0 || Convert.ToInt32(obj.amount) == 0)
            {
                _log4net.Warn("invalid accountid or amount 0");
                return NotFound();
            }
            try
            {
                TransactionStatus ts = new TransactionStatus();
                ts = _service.withdrawAccount(Convert.ToInt32(obj.accountId), Convert.ToInt32(obj.amount));

                _log4net.Info("account has been debited successfully");
                return Ok(ts);

            }

            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }


        }
    }
}
