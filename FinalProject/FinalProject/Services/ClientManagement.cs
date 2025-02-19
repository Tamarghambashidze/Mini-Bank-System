using FinalProject.Exceptions;
using FinalProject.Extentsions;
using FinalProject.Repsitories;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    internal class ClientManagement
    {
        private ClientRepository _repository;
        public ClientManagement(ClientRepository repository) => _repository = repository;
        public void ClientManagementSyStem()
        {
            do
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Client Management");
                Console.Write("1. Add New Client\n2. Update Client Information\n" +
                    "3. Delete Client\n4. View Client List\n5. VIP Clients List" +
                    "\n6. Exit \n Answer: ");
                int answer = Console.ReadLine().IntParse();
                switch (answer)
                {
                    case 1:
                        AddClient();
                        break;
                    case 2:
                        UpdateClient();
                        break;
                    case 3:
                        DeleteClient();
                        break;
                    case 4:
                        _repository.ViewClients();
                        break;
                    case 5:
                        _repository.ViewVipClients();
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("Incorrect input");
                        break;
                }
            } while (true);
        }

        void AddClient()
        {
            string firstName, lastName;
            do
            {
                try
                {
                    Console.Write("FirstName: ");
                    firstName = Console.ReadLine();
                    if (firstName.Length < 3)
                        throw new ClientException("Must be longer than 3");

                    Console.Write("LastName: ");
                    lastName = Console.ReadLine();
                    if (lastName.Length < 4)
                        throw new ClientException("Must be longer than 4");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                break;
            } while (true);
            _repository.AddClient(firstName, lastName);
            int id;
            DateTime birthDate;
            string address, phonenumber, email;
            do
            {
                try
                {
                    id = _repository.SearchClientId(firstName, lastName);
                    Console.Write("Enter birth year: ");
                    int year = Console.ReadLine().IntParse();
                    if (year < 1930 || year > 2006)
                        throw new ClientException("Birth year must be between 1930 and 2006");
                    Console.Write("Enter birth month: ");
                    int month = Console.ReadLine().IntParse();
                    if (month >= 13 || year < 0)
                        throw new ClientException("Birth month must be between 1 and 12");
                    Console.Write("Enter birth day: ");
                    int day = Console.ReadLine().IntParse();
                    if (day > 31 || day < 0)
                        throw new ClientException("Birth day must be between 1 and 31");
                    birthDate = new DateTime(year, month, day);

                    Console.Write("Address: ");
                    address = Console.ReadLine();
                    if (address == null)
                        throw new ClientDetailsException("This field must be filled");

                    Console.Write("Enter phone number: ");
                    phonenumber = Console.ReadLine();
                    phonenumber.IntParse();
                    if (phonenumber.Length != 9)
                        throw new ClientException("Phone number must be 9 digits");

                    Console.Write("Enter email: ");
                    email = Console.ReadLine();
                    if (!email.Contains('a') || !email.Contains('.'))
                        throw new ClientException("Email must contain '@' and '.'");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                break;
            } while (true);
            _repository.AddClientDetails(id, birthDate, address, phonenumber, email);
        }
        void UpdateClient()
        {
            Console.Write("First name: ");
            string firstname = Console.ReadLine();
            Console.Write("Last name: ");
            string lastname = Console.ReadLine();
            int id = _repository.SearchClientId(firstname, lastname);
            if (id == default)
            {
                Console.WriteLine("Account not found");
                return;
            }
            Console.Write("1. Phone number\n2. Email\n3. Address\n4. Exit\n Answer: ");
            int answer = Console.ReadLine().IntParse();
            switch (answer)
            { 
                case 1:
                    string phoneNumber;
                    do
                    {
                        Console.Write("Enter phone number: ");
                        phoneNumber = Console.ReadLine();
                        try
                        {
                            phoneNumber.IntParse();
                            if (phoneNumber.Length != 9)
                                throw new ClientException("Phone number must be 9 digits");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                        break;
                    } while (true);
                    _repository.UpdateClientPhoneNumber(id, phoneNumber);
                    break;
                case 2:
                    string email;
                    do
                    {
                        Console.Write("Enter email: ");
                        email = Console.ReadLine();
                        try
                        {
                            if (!email.Contains('a') || !email.Contains('.'))
                                throw new ClientException("Email must contain '@' and '.'");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                        break;
                    } while (true);
                    _repository.UpdateClientEmail(id, email);
                    break;
                case 3:
                    string address;
                    do
                    {
                        Console.Write("Enter address: ");
                        address = Console.ReadLine();
                        try
                        {
                            if (address == null)
                                throw new ClientDetailsException("This field must be filled");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                        break;
                    } while (true);
                    _repository.UpdateClientAddress(id, address);
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Incorrect input");
                    break;
            }
        }
        void DeleteClient()
        {
            Console.Write("First name: ");
            string firstname = Console.ReadLine();
            Console.Write("Last name: ");
            string lastname = Console.ReadLine();
            int id = _repository.SearchClientId(firstname, lastname);
            if (id == default)
            {
                Console.WriteLine("Account not found");
                return;
            }
            _repository.DeleteClient(id);
        }
    }
}
