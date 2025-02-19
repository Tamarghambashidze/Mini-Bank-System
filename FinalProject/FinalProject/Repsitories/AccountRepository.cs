using FinalProject.Database;
using FinalProject.Extentsions;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Repsitories
{
    internal class AccountRepository : Abstracts.IAccount
    {
        private BankSyStemDbContext _context;
        public AccountRepository(BankSyStemDbContext context) => _context = context;

        Account SearchAccount(int accountId)
        {
            return _context.Accounts.Include(a => a.Currency).Include(a => a.Type).Include(a => a.Clients)
                .FirstOrDefault(a => a.Id == accountId);
        }

        bool CheckClientAccount(int accountId, int clientId)
        {
            return _context.ClientToAccountTable.FirstOrDefault(ca =>
                ca.ClientId == clientId && ca.AccountId == accountId) == null;
        }
        public int GetAccountId(string accountNumber)
        {
            var accounts = _context.Accounts.ToList();
            var account = accounts.FirstOrDefault(a => accountNumber.Verify(a.AccountNumber));
            if (account == null)
                return default;
            else
                return account.Id;
        }
        public void ClientAccount(int accountId, int clientId)
        {
            var clientAccount = new ClientAccount()
            {
                ClientId = clientId,
                AccountId = accountId
            };
            if (CheckClientAccount(accountId, clientId))
            {
                _context.ClientToAccountTable.Add(clientAccount);
                _context.SaveChanges();
                Console.WriteLine("Client added to account successfully");
            }
            else
                Console.WriteLine("This user is already on account");
        }

        string GenerateAccountNumber()
        {
            string guid = Guid.NewGuid().ToString("N");
            return guid.Substring(0, 16);
        }
        public void OpenNewAccount(List<int> clientIds, int currencyId, int accountTypeId)
        {
            var currency = _context.CurrencyTypes.FirstOrDefault(ct => ct.Id == currencyId);
            var accountType = _context.AccountTypes.FirstOrDefault(at => at.Id == accountTypeId);
            if (currency == null || accountType == null)
                return;
            string accountNumber = GenerateAccountNumber();
            Console.WriteLine("Your account number: " + accountNumber);
            var account = new Account()
            {
                AccountNumber = accountNumber.Hash(),
                Balance = 0,
                CurrencyId = currencyId,
                AccountTypeId = accountTypeId,
                IsActive = true,
                OpenDate = DateTime.Now
            };
            _context.Accounts.Add(account);
            _context.SaveChanges();
            string content = $"\nNew Account added: {DateTime.Now}";
            content.AddToFile();
            foreach (var id in clientIds)
            {
                ClientAccount(GetAccountId(accountNumber), id);
                var client = _context.Clients.Include(c => c.Accounts).FirstOrDefault(c => c.Id == id);
                if(client.Accounts.ToList().Count >= 5)
                {
                    _context.ClientDetailsTable.FirstOrDefault(c => c.ClientId == id).IsVIPClient = true;
                    _context.SaveChanges();
                }
                $"Account owner: {client}".AddToFile();
            }
            Console.WriteLine("--------------- \nAccount added succcessfully");
        }

        public void CloseAccount(int accountId)
        {
            var account = SearchAccount(accountId);
            if (account != null && account.IsActive)
            {
                account.IsActive = false;
                _context.SaveChanges();
                Console.WriteLine("Account closed successfully");
                string content = $"\nNew Account closed: {DateTime.Now}";
                content.AddToFile();
                "Account owners: ".AddToFile();
                account.Clients.ToList().ForEach(c => c.ToString().AddToFile());
            }
            else if (!account.IsActive)
                Console.WriteLine("This account has already been deactivated");
            else
                Console.WriteLine("Account not found");
        }
        public void ActivateAccount(int accountId)
        {
            var account = SearchAccount(accountId);
            if (account != null && !account.IsActive)
            {
                account.IsActive = true;
                _context.SaveChanges();
                Console.WriteLine("Account activated successfully");
                string content = $"\nNew Account added: {DateTime.Now}";
                content.AddToFile();
                "Account owners: ".AddToFile();
                account.Clients.ToList().ForEach(c => c.ToString().AddToFile());
            }
            else if (account.IsActive)
                Console.WriteLine("This account has already been activated");
            else
                Console.WriteLine("Account not found");
        }

        public void ViewBalance(int accountId)
        {
            var account = SearchAccount(accountId);
            if (account != null)
                Console.WriteLine($"Balance: {account.Balance} {account.Currency.Name}");
            else
                Console.WriteLine("Account not found");
        }

        public void ViewClientAccounts(int clientId)
        {
            var client = _context.Clients
                .Include(c => c.Accounts)  // load accounts          
                .ThenInclude(a => a.Currency)  // load currencies
                .Include(c => c.Accounts)       
                .ThenInclude(a => a.Type)   // load account types                    
                .FirstOrDefault(c => c.Id == clientId);
            if (client != null && client.Accounts.Any())
            {
                var activeAccounts = client.Accounts.Where(a => a.IsActive);
                var closedAccouts = client.Accounts.Where(a => !a.IsActive);
                Console.WriteLine(client);
                Console.WriteLine("\n Active accounts: ");
                if (activeAccounts.Any())
                {
                    foreach (var item in activeAccounts)
                    {
                        Console.WriteLine($"Balance: {item.Balance} {item.Currency.Name} - " +
                            $"Account type: {item.Type.Name} \n");
                    }
                }
                else
                    Console.WriteLine("Client has no active accounts");
                if (closedAccouts.Any())
                {
                    Console.WriteLine("\n Closed accounts: ");
                    foreach (var item in closedAccouts)
                    {
                        Console.WriteLine($"Balance: {item.Balance} {item.Currency.Name} " +
                            $"Account type: {item.Type.Name}");
                    }
                }
                else
                    Console.WriteLine("Client has no closed accounts");
            }
            else if (!client.Accounts.Any())
                Console.WriteLine("This client doesn't have any accounts");
            else
                Console.WriteLine("This client can't be found");
        }

        public void ViewCurrencies()
        {
            var currencies = _context.CurrencyTypes;
            foreach( var currency in currencies)
            {
                Console.WriteLine($"Id:{currency.Id} - Name: {currency.Name}");
            }
        }

        public void ViewAccountTypes()
        {
            var accountTypes = _context.AccountTypes;
            foreach (var accountType in accountTypes)
            {
                Console.WriteLine($"Id:{accountType.Id} - Name: {accountType.Name}");
            }
        }
    }
}
