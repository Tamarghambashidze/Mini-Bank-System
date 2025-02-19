using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    internal class ClientAccount
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
