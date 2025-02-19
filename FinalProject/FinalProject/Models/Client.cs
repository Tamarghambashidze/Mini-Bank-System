using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    internal class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ClientDetails ClientDetails { get; set; } // one to one
        public ICollection<Account> Accounts { get; set; } //Many to many

        public override string ToString()
        {
            return $"Client: {FirstName} {LastName}";
        }
    }
}
