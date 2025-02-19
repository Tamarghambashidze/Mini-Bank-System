using FinalProject.Abstracts;
using FinalProject.Database;
using FinalProject.Extentsions;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FinalProject.Repsitories
{
    internal class AnalyticsRepository : IAnalytics
    {
        private BankSyStemDbContext _context;
        public AnalyticsRepository(BankSyStemDbContext context) => _context = context;

        List<Account> ReturnAccount()
        {
            return _context.Accounts.Include(a => a.Clients)
                .Include(a => a.Currency).Include(a => a.Type).ToList();
        }
        void Print(List<Account> accounts)
        {
            foreach (var account in accounts)
            {
                Console.WriteLine("Account owners: ");
                account.Clients.ToList().ForEach(c => Console.WriteLine(c));
                Console.WriteLine($"Account type: {account.Type.Name}");
                Console.WriteLine($"Balance: {account.Balance} {account.Currency.Name}");
                Console.WriteLine($"Open date: {account.OpenDate}");
                Console.WriteLine();
            }
        }
        public void SortByActivity()
        {
            var sortedAccounts = ReturnAccount()
                .OrderByDescending(a => a.IsActive)
                .ThenBy(a => a.OpenDate).ToList();
            Print(sortedAccounts);

        }
        public void SortByBalance()
        {
            var accounts = ReturnAccount();
            //convert balance to usd
            var sortedAccounts = accounts.OrderBy(a => a.CurrencyId.CurrencyConvert(a.Balance, 2));
            Print(sortedAccounts.ToList());
        }

        public void TransactionRecord(int answer)
        {
            var transactions = _context.Transactions.Include(t => t.Account)
                .ThenInclude(a => a.Clients).Include(t => t.Type).Include(t => t.Status);
            var sortedTransactions = transactions.OrderBy(t => t.TransactionDate);
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions;
            switch (answer)
            {
                case 1:
                    groupedTransactions = sortedTransactions.GroupBy(st => st.Status.Name);
                    break;
                case 2:
                    groupedTransactions = sortedTransactions.GroupBy(st => st.Type.Name);
                    break;
                default:
                    Console.WriteLine("Incorrect input");
                    return;
            }

            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"\n {group.Key}");
                foreach (var transaction in group)
                {
                    Console.WriteLine();
                    if (answer == 1)
                        Console.WriteLine($"Transaction type: {transaction.Type.Name}");
                    else if(answer == 2)
                        Console.WriteLine($"Transaction status: {transaction.Status.Name}");
                    Console.WriteLine("Transaction made by: ");
                    transaction.Account.Clients.ToList().ForEach(c => Console.WriteLine(c));
                    Console.WriteLine($"Amount: {transaction.Amount}");
                    Console.WriteLine($"Transaction date: {transaction.TransactionDate}");
                }
                Console.WriteLine("----");
            }
        }
        public void SortByCurrencies()
        {
            var currencies = _context.CurrencyTypes.Include(c => c.Accounts).ThenInclude(a => a.Clients)
                .Include(c => c.Accounts).ThenInclude(a => a.Type);
            foreach (var item in currencies)
            {
                Console.WriteLine(item.Name);
                Console.WriteLine("Accouts: ");
                var accounts = item.Accounts;
                if(accounts.Any())
                    Print(accounts.ToList());
                else
                    Console.WriteLine("No accounts");
                Console.WriteLine("----");
            }
        }
    }
}
