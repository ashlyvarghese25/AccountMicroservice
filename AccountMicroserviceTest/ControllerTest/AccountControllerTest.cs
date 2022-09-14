using Account_Microservice.Controllers;
using Account_Microservice.Model;
using Account_Microservice.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountMicroserviceTest.ControllerTest
{
    [ExcludeFromCodeCoverage]
    [TestFixture]

    public class AccountControllerTest
    {
        [Test]
        public void AddAccountTest_Ok()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.AddAccount(1, "Savings"))
           .Returns(new AccountCreationStatus { Message = "Account has been successfully created", AccountId = 2 });



            var accountController = new AccountController(mockAccountService.Object);
            var result = accountController.createAccount(1);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.NotNull(result);
        }
        [Test]
        public void AddAccount_BadRequest()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.AddAccount(1, "Savings"))
           .Returns(new AccountCreationStatus { Message = "User already exists" });



            var accountController = new AccountController(mockAccountService.Object);
            var result = accountController.createAccount(0);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
            Assert.NotNull(result);
        }
        [Test]
        public void depositAccountTest_Ok()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.depositAccount(1, 1000))
           .Returns(new TransactionStatus { Message = "Depoisted", source_balance = 0, destination_balance = 1000 });



            var accountController = new AccountController(mockAccountService.Object);
            var result = accountController.deposit(new Transactions { accountId = 1, amount = 1000 });
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.NotNull(result);
        }
        [Test]
        public void depositAccountTest_BadRequest()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.depositAccount(1,20000))
           .Returns(new TransactionStatus { Message = "Not Depoisted" });



            var accountController = new AccountController(mockAccountService.Object);
            var result = accountController.deposit(new Transactions { accountId = 0, amount = 0 });
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
            Assert.NotNull(result);
        }
        [Test]
        public void withdrawAccountTest_Ok()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.withdrawAccount(1, 100))
           .Returns(new TransactionStatus { Message = "withdrawn", source_balance = 0, destination_balance = 1000 });



            var accountController = new AccountController(mockAccountService.Object);
            var result = accountController.withdraw(new Transactions { accountId = 1, amount = 100 });
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.NotNull(result);
        }
        [Test]
        public void withdrawAccountTest_BadRequest()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.withdrawAccount(1, 100))
           .Returns(new TransactionStatus { Message = "Not withdrawn"});



            var accountController = new AccountController(mockAccountService.Object);
            var result = accountController.withdraw(new Transactions { accountId = 0, amount = 0 });
            Assert.That(result, Is.TypeOf<NotFoundResult>());
            Assert.NotNull(result);
        }
        [Test]
        public void getCustomerAccountsTest_Ok()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.getAllAccounts(1))
           .Returns(new List<AccountViewModel>() { new AccountViewModel() { Id = 1, Balance = 1000 } });



            var accountController = new AccountController(mockAccountService.Object);
            var result = accountController.getCustomerAccounts(1);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.NotNull(result);
        }
        [Test]
        public void getCustomerAccountsTest_BadRequest()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.getAllAccounts(1))
           .Returns(new List<AccountViewModel>() { new AccountViewModel() { Id = 1, Balance = 1000 } });



            var accountController = new AccountController(mockAccountService.Object);
            var result = accountController.getCustomerAccounts(0);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
            Assert.NotNull(result);
        }
        [Test]
        public void getAllCustomerAccountsTest_Ok()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.getCustomersAllAccounts())
           .Returns(new List<Account>() { new Account() { AccountId = 1, CustomerId = 1, Balance = 1000, AccountType = "Savings", minBalance = 1000 } });



            var accountController = new AccountController(mockAccountService.Object);
            var result = accountController.getAllCustomerAccounts();
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.NotNull(result);
        }
        [Test]
        public void getAccountTest_Ok()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.getCustomerAccount(1))
           .Returns(new Account() { AccountId = 1, CustomerId = 1, Balance = 1000, AccountType = "Savings", minBalance = 1000  });



            var accountController = new AccountController(mockAccountService.Object);
            var result = accountController.getAccount(1);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.NotNull(result);
        }
        [Test]
        public void getAccountTest_BadRequest()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.getCustomerAccount(1))
           .Returns(new Account() {});



            var accountController = new AccountController(mockAccountService.Object);
            var result = accountController.getAccount(0);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
            Assert.NotNull(result);
        }
        //[Test]
        //public void getAccountStatementTest_Ok()
        //{
        //    var mockAccountService = new Mock<IAccountService>();
        //    mockAccountService.Setup(x => x.getStatement(1, new DateTime(2022, 09, 01), new DateTime(2022, 09, 12)))
        //   .Returns(new List<Transactions>{ new Transactions(){accountId=1 }});



        //    var accountController = new AccountController(mockAccountService.Object);
        //    var result = accountController.getAccountStatement(new AccountStatement { AccountId = 1,from_date= new DateTime(2022, 09, 01),to_date= new DateTime(2022, 09, 12) });
        //    Assert.That(result, Is.TypeOf<OkObjectResult>());
        //    Assert.That(1, Is.TypeOf<OkObjectResult>());
        //    Assert.NotNull(result);
        //}
        //[Test]
        //public void getAccountStatementTest_BadRequest()
        //{
        //    var mockAccountService = new Mock<IAccountService>();
        //    mockAccountService.Setup(x => x.getStatement(1, new DateTime(2022, 09, 01), new DateTime(2022, 09, 12)))
        //   .Returns(new List<Transactions> {  });



        //    var accountController = new AccountController(mockAccountService.Object);
        //    var result = accountController.getAccountStatement(new AccountStatement { AccountId = 0, from_date = new DateTime(2022, 09, 01), to_date = new DateTime(2022, 09, 12) });
        //    Assert.That(result, Is.TypeOf<NotFoundResult>());
        //    Assert.NotNull(result);
        //}
    }
}


