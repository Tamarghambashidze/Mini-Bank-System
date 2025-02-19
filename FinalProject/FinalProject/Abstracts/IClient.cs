using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Abstracts
{
    internal interface IClient
    {
        public int SearchClientId(string firstname, string lastname);
        public void AddClient(string firstName, string lastName);
        public void AddClientDetails(int clientId, DateTime birthDate,
            string address, string phoneNumber, string email);
        public void UpdateClientAddress(int clientId, string address);
        public void UpdateClientPhoneNumber(int clientId, string phoneNumber);
        public void UpdateClientEmail(int clientId, string email);
        public void DeleteClient(int clientId);
        public void ViewClients();
        public void ViewVipClients();
    }
}
