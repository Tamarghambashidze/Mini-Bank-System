using FinalProject.Database;
using FinalProject.Extentsions;
using FinalProject.Models;
using FinalProject.Repsitories;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    internal class AccountManagement
    {
        private AccountRepository _repository;
        public AccountManagement(AccountRepository repository) => _repository = repository;
        public void AccountManagementSystem(ClientRepository repository)
        {
            do
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Client Management");
                Console.Write("1. Open New Account\n2. Close Account\n" +
                    "3. Activate Account\n4. View Balance\n5. View Client Accounts" +
                    "\n6. Add Client to Account\n7. Exit\n Answer: ");
                int answer = Console.ReadLine().IntParse();
                switch (answer)
                {
                    case 1:
                        OpenNewAccount(repository);
                        break;
                    case 2:
                        int closeId = AccountId();
                        if (closeId != default)
                            _repository.CloseAccount(closeId);
                        break;
                    case 3:
                        int activateId = AccountId();
                        if (activateId != default)
                            _repository.ActivateAccount(activateId);
                        break;
                    case 4:
                        int id = AccountId();
                        if (id != default)
                            _repository.ViewBalance(id);
                        break;
                    case 5:
                        {
                            int clientId = ClientId(repository);
                            if (clientId != default)
                                _repository.ViewClientAccounts(clientId);
                        }
                        break;
                    case 6:
                        {
                            int accountId = AccountId();
                            int clientId = ClientId(repository);
                            if (clientId != default && accountId != default)
                                _repository.ClientAccount(accountId, clientId);
                        }
                        break;
                    case 7:
                        return;
                    default:
                        Console.WriteLine("Incorrect input");
                        break;
                }
            } while (true);
        }
        void OpenNewAccount(ClientRepository repository)
        {
            Console.Write("Client quantity for this account: ");
            int quantity = Console.ReadLine().IntParse();
            var clientIds = new List<int>();
            for(int i = 0; i < quantity; i++)
            {
                int clientId = ClientId(repository);
                if(clientId != default)
                    clientIds.Add(clientId);
                else
                {
                    Console.WriteLine("Client not found");
                    i--;
                }
            }
            _repository.ViewCurrencies();
            Console.Write(" Enter Id: ");
            int currencyId = Console.ReadLine().IntParse();
            _repository.ViewAccountTypes();
            Console.Write(" Enter Id: ");
            int accountTypeId = Console.ReadLine().IntParse();
            _repository.OpenNewAccount(clientIds, currencyId, accountTypeId);
        }

        int ClientId(ClientRepository repository)
        {
            Console.Write("Client first name: ");
            string firstname = Console.ReadLine();
            Console.Write("Client last name: ");
            string lastname = Console.ReadLine();
            int id =  repository.SearchClientId(firstname, lastname);
            if (id == default)
                Console.WriteLine("Client not found");
            return id;
        }
        int AccountId()
        {
            Console.Write("Enter account number: ");
            string accountNumber = Console.ReadLine();
            int id = _repository.GetAccountId(accountNumber);
            if (id == default)
                Console.WriteLine("Account not found");
            return id;
        }
    }
}
