using FinalProject.Abstracts;
using FinalProject.Database;
using FinalProject.Extentsions;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Repsitories
{
    internal class ClientRepository : IClient
    {
        private BankSyStemDbContext _context;
        public ClientRepository(BankSyStemDbContext context) => _context = context;

        bool IsVip(int clientId)
        {
            var client = SearchClient(clientId);
            var accounts = client.Accounts.ToArray();
            if (accounts.Any())
                return client.Accounts.ToArray().Length > 4;
            else
                return false;
        }

        public int SearchClientId(string firstname, string lastname)
        {
            var client =  _context.Clients.FirstOrDefault(c => c.FirstName == firstname 
            && c.LastName == lastname);
            if (client == null)
                return default;
            else
                return client.Id;
        }
        Client SearchClient(int id)
        {
            return _context.Clients.Include(c => c.ClientDetails).Include(c => c.Accounts).FirstOrDefault(c => c.Id == id);
        }
        public void AddClient(string firstName, string lastName)
        {
            Client client = new Client()
            {
                FirstName = firstName,
                LastName = lastName,
                RegistrationDate = DateTime.Now
            };
            int id = SearchClientId(firstName, lastName);
            if (id == default)
            {
                _context.Clients.Add(client);
                _context.SaveChanges();
                Console.WriteLine("Client added successfully");
            }
            else
                Console.WriteLine("This account already exists");
        }

        public void AddClientDetails(int clientId, DateTime birthDate,
            string address, string phoneNumber, string email)
        {
            var client = SearchClient(clientId);
            if (client.ClientDetails == default && client != null)
            {
                ClientDetails newClientDetails = new ClientDetails()
                {
                    ClientId = clientId,
                    DateOfBirth = birthDate,
                    Adress = address,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    IsVIPClient = IsVip(clientId)
                };
                _context.ClientDetailsTable.Add(newClientDetails);
                _context.SaveChanges();
                Console.WriteLine("Client details filled successfully");
            }
            else if (client == null)
                Console.WriteLine("This client doesn't exist");
            else
                Console.WriteLine("This client has already filled details");
        }

        public void UpdateClientAddress(int clientId, string address)
        {
            var client = SearchClient(clientId);
            if (client != null && address != client.ClientDetails.Adress)
            {
                client.ClientDetails.Adress = address;
                _context.SaveChanges();
                Console.WriteLine(client + " " + client.ClientDetails);
            }
            else if (address == client.ClientDetails.Adress)
                Console.WriteLine($"{address} is your current address");
            else
                Console.WriteLine("This client doesn't exist");
        }

        public void UpdateClientPhoneNumber(int clientId, string phoneNumber)
        {
            var client = SearchClient(clientId);
            if (client != null && phoneNumber != client.ClientDetails.PhoneNumber)
            {
                client.ClientDetails.PhoneNumber = phoneNumber;
                _context.SaveChanges();
                Console.WriteLine(client + " " + client.ClientDetails);
            }
            else if (phoneNumber == client.ClientDetails.PhoneNumber)
                Console.WriteLine($"{phoneNumber} is your current phone number");
            else
                Console.WriteLine("This client doesn't exist");
        }

        public void UpdateClientEmail(int clientId, string email)
        {
            var client = SearchClient(clientId);
            if (client != null && email != client.ClientDetails.Email)
            {
                client.ClientDetails.Email = email;
                _context.SaveChanges();
                Console.WriteLine(client + " " + client.ClientDetails);
            }
            else if(email == client.ClientDetails.Email)
                Console.WriteLine($"{email} is your current email");
            else
                Console.WriteLine("This client doesn't exist");
        }

        public void DeleteClient(int clientId)
        {
            var client = SearchClient(clientId);
            if (client != null)
            {
                Console.WriteLine($"{client} - deleted successfully");
                _context.Clients.Remove(client);
                _context.ClientDetailsTable.Remove(client.ClientDetails);
                _context.SaveChanges();
            }
            else
                Console.WriteLine("This client doesn't exist");
        }
        public void ViewClients()
        {
            var clients = _context.Clients.Include(c => c.ClientDetails);
            foreach (var client in clients)
            {
                Console.Write(client);
                Console.WriteLine(client.ClientDetails + "\n");
            }
        }
        public void ViewVipClients()
        {
            var vipClients = _context.Clients.Include(_ => _.ClientDetails)
                .Where(c => c.ClientDetails.IsVIPClient);
            foreach (var client in vipClients)
            {
                Console.Write(client);
                Console.WriteLine(client.ClientDetails + "\n");
            }
        }
    }
}
