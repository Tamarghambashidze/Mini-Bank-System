using FinalProject.Abstracts;
using FinalProject.Database;
using FinalProject.Extentsions;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Repsitories
{
    internal class TransactionRepository : ITransaction
    {
        private BankSyStemDbContext _context;
        public TransactionRepository(BankSyStemDbContext context) => _context = context;

        Account SearchAccount(int id)
        {
            return _context.Accounts.Include(a => a.Transactions)
                .ThenInclude(t => t.Type)
                .Include(a => a.Transactions)
                .ThenInclude(t => t.Status)
                .Include(a => a.Clients)
                .FirstOrDefault(a => a.Id == id);
        }

        Transaction Transaction(decimal amount, int accountId)
        {
            return new Transaction()
            {
                Amount = amount,
                AccountId = accountId,
                TransactionDate = DateTime.Now
            };
        }
        public void Withdraw(decimal amount, int accountId)
        {
            var account = SearchAccount(accountId);
            if (account == null)
            {
                Console.WriteLine("Account not found");
                return;
            }
            var transaction = Transaction(amount, accountId);
            transaction.TransactionTypeId = 1;
            if (account.Balance < amount)
            {
                transaction.StatusId = 3;
                Console.WriteLine("Transaction failed");
            }
            else
            {
                account.Balance -= amount;
                transaction.TransactionTypeId = 2;
                Console.WriteLine("Successfull transaction");
                _context.SaveChanges();
            }
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            string content = $"\nNew Transaction(Withdraw): {DateTime.Now}\nAmount: {amount}";
            content.AddToFile();
            "Account owners: ".AddToFile();
            account.Clients.ToList().ForEach(c => c.ToString().AddToFile());
        }

        public void Deposit(decimal amount, int accountId)
        {
            var account = SearchAccount(accountId);
            if (account == null)
            {
                Console.WriteLine("Account not found");
                return;
            }
            Transaction transaction = Transaction(amount, accountId);
            transaction.TransactionTypeId = 2;
            account.Balance += amount;
            transaction.StatusId = 2;
            Console.WriteLine("Successfull transaction");
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            string content = $"\nNew Transaction(Deposit): {DateTime.Now}\nAmount: {amount}";
            content.AddToFile();
            "Account owners: ".AddToFile();
            account.Clients.ToList().ForEach(c => c.ToString().AddToFile());
        }

        public void Transaction(decimal amount, int fromAccountId, int toAccountId, int currencyId)
        {
            var recieverTransaction = Transaction(amount, toAccountId);
            recieverTransaction.TransactionTypeId = 4;
            var senderTransaction = Transaction(amount, fromAccountId);
            senderTransaction.TransactionTypeId = 3;
            try
            {
                if (amount <= 0)
                    throw new ArgumentException("Amount must be greater than zero.");

                var sender = SearchAccount(fromAccountId);
                var receiver = SearchAccount(toAccountId);

                if (sender == null || receiver == null)
                    throw new Exception("Sender or Receiver account not found.");


                if (currencyId < 0 || currencyId > 4)
                    throw new Exception("Currency id must be between 1 and 3");

                decimal senderAmount = CurrencyConvert(amount, fromAccountId, currencyId);
                decimal receiverAmount = CurrencyConvert(amount, toAccountId, currencyId);

                if (sender.Balance < senderAmount)
                {
                    recieverTransaction.StatusId = 3;
                    senderTransaction.StatusId = 3;
                    throw new Exception("Insufficient funds.");
                }
                sender.Balance -= senderAmount;
                _context.SaveChanges();
                receiver.Balance += receiverAmount;

                recieverTransaction.StatusId = 2;
                senderTransaction.StatusId = 2;
                Console.WriteLine("Successful transaction");
                string content = $"\nNew Transaction(Transfer): {DateTime.Now}\nAmount: {amount}" +
                    $"\nSender: {sender.Clients.First()}\nReceiver{receiver.Clients.First()}";
                content.AddToFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _context.Transactions.Add(recieverTransaction);
            _context.Transactions.Add(senderTransaction);
            _context.SaveChanges();
        }

        public decimal CurrencyConvert(decimal amount, int accountId, int currencyId)
        {
            return SearchAccount(accountId).CurrencyId.CurrencyConvert(amount, currencyId);
        }

        public void ViewTransactionHistory(int accountId)
        {
            var account = SearchAccount(accountId);
            foreach (var client in account.Clients)
            {
                Console.WriteLine(client);
            }
            Console.WriteLine("Transactions: ");
            foreach (var item in account.Transactions)
            {
                Console.WriteLine($" Date: {item.TransactionDate}\n " +
                    $"Type: {item.Type.Name}\n " +
                    $"Status: {item.Status.Name}\n " +
                    $"Amount: {item.Amount}\n");
            }
        }
    }
}
