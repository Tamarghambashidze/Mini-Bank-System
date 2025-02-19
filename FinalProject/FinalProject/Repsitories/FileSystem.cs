using FinalProject.Abstracts;
using FinalProject.Database;
using FinalProject.Extentsions;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Repsitories
{
    internal class FileSystem : IFile
    {
        private BankSyStemDbContext _context;
        public FileSystem(BankSyStemDbContext context) => _context = context;
        public void AccountStatement()
        {
            var accounts = _context.Accounts.Include(a => a.Clients)
                .Include(a => a.Currency)
                .Include(a => a.Transactions)
                .ThenInclude(t => t.Type)
                .Include(a => a.Transactions)
                .ThenInclude(t => t.Status);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/FinalProject/Account Statement";
            Directory.CreateDirectory(path);
            foreach (var item in accounts)
            {
                string itemPath = Path.Combine(path, $"account_{item.Id}_statement.txt");
                using (StreamWriter writer = new StreamWriter(itemPath, append: false))
                {
                    writer.WriteLine("Account owners: ");
                    item.Clients.ToList().ForEach(c => writer.WriteLine(c));
                    writer.WriteLine($"Current balance: {item.Balance} {item.Currency.Name}");
                    writer.WriteLine($"Open date: {item.OpenDate}");
                    writer.WriteLine("Transactions");
                    item.Transactions.ToList().ForEach(t =>
                    {
                        writer.WriteLine($"Transaction type: {t.Type.Name}");
                        writer.WriteLine($"Transaction status: {t.Status.Name}");
                        writer.WriteLine($"Amount: {t.Amount}");
                        writer.WriteLine($"Transaction date: {t.TransactionDate}");
                        writer.WriteLine();
                    });
                }
                itemPath.ReadFromFile();              
            }

        }

        public void SystemLog()
        {
            string itemPath = ExtensionMethods.FilePath();
            itemPath.ReadFromFile();
        }
    }
}
