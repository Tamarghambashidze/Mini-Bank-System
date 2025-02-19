using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    internal class ClientDetails
    {
        public int Id { get; set; }
        public int ClientId { get; set; } //foreign key
        public Client Client { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsVIPClient { get; set; }

        public override string ToString()
        {
            return $"\nDate of birth: {DateOfBirth}\nAddress: {Adress}\nPhone number:" +
                $" {PhoneNumber}\nEmail: {Email}";
        }
    }
}
