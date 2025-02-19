using FinalProject.Extentsions;
using FinalProject.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    internal class Analytics
    {
        private AnalyticsRepository _repository;
        public Analytics(AnalyticsRepository repository) => _repository = repository;
        public void AnalyticsSystem()
        {
            do
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Analytics");
                Console.Write("1. Most Active Accounts\n2. Clients with the " +
                    "Highest Balances\n3. Daily Transaction Report\n4. Account " +
                    "Statistics by Currencies\n5. Exit\n Answer: ");
                int answer = Console.ReadLine().IntParse();
                switch (answer)
                {
                    case 1:
                        Console.WriteLine("Most active accounts");
                        _repository.SortByActivity();
                        break;
                    case 2:
                        Console.WriteLine("Clients with the highest balance");
                        _repository.SortByBalance();
                        break;
                    case 3:
                        Console.WriteLine("Daily transaction report");
                        Console.WriteLine("1. Group by transaction status");
                        Console.WriteLine("2. Group by transaction type");
                        Console.Write(" Answer: ");
                        int num = Console.ReadLine().IntParse();
                        _repository.TransactionRecord(num);
                        break;
                    case 4:
                        Console.WriteLine("Statistics by currencies");
                        _repository.SortByCurrencies();
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Incorrect input");
                        break;
                }
            } while (true);
        }
    }
}
