using FinalProject.Extentsions;
using FinalProject.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    internal class TransactionManagement
    {
        private TransactionRepository _repository;
        public TransactionManagement(TransactionRepository repository) => _repository = repository;

        public void TransactionManagementSystem(AccountRepository repository)
        {
            do
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Transactions");
                Console.Write("1. Deposit Money\n2. Withdraw Money\n3. Transfer Money" +
                    "\n4. Transaction History\n5. Exit\n Answer: ");
                int answer = Console.ReadLine().IntParse();
                switch (answer)
                {
                    case 1:
                        {
                            int id = GetAccountId(repository);
                            if(id != default)
                            {
                                decimal amount = Amount(repository, id);
                                if(amount != default)
                                    _repository.Deposit(amount, id);
                            }
                            break;
                        }
                    case 2:
                        {
                            int id = GetAccountId(repository);
                            if (id != default)
                            {
                                decimal amount = Amount(repository, id  );
                                if (amount != default)
                                    _repository.Withdraw(amount, id);
                            }
                            break;
                        }
                    case 3:
                        TransferMoney(repository);
                        break;
                    case 4:
                        {
                            int id = GetAccountId(repository);
                            if (id != default)
                                _repository.ViewTransactionHistory(id);
                            break;
                        }
                    case 5:
                        return;
                    case 6:
                        Console.WriteLine("Incorrect input");
                        continue;
                }
            } while (true);
        }
        int GetAccountId(AccountRepository repository)
        {
            Console.Write("Account number: ");
            string accountNumber = Console.ReadLine();
            int id = repository.GetAccountId(accountNumber);
            if (id == default)
                Console.WriteLine("Account not found");
            return id;
        }
        void TransferMoney(AccountRepository repository)
        {
            Console.WriteLine("Sender");
            int senderId = GetAccountId(repository);
            Console.WriteLine("Receiver");
            int receieverId = GetAccountId(repository);
            if(senderId != default && receieverId != default)
            {
                Console.Write("Amount: ");
                decimal amount = Console.ReadLine().DecimalParse();
                Console.Write("Enter Id: ");
                int currencyId = Console.ReadLine().IntParse();
                _repository.Transaction(amount, senderId, receieverId, currencyId);
            }
        }

        decimal Amount(AccountRepository repository, int accountId)
        {
            decimal result = default;
            if(accountId != null)
            {
                Console.Write("Amount: ");
                decimal amount = Console.ReadLine().DecimalParse();
                repository.ViewCurrencies();
                Console.Write("Enter Id: ");
                int currencyId = Console.ReadLine().IntParse();
                result = _repository.CurrencyConvert(amount, accountId, currencyId);
            }
            return result;
        }
    }
}
