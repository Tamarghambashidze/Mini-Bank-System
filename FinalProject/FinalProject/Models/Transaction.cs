using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    internal class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int TransactionTypeId { get; set; } //foreign key
        public TransactionType Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public int StatusId { get; set; } // foreign key
        public TransactionStatus Status { get; set; }
        public int AccountId { get; set; } //foreign key
        public Account Account { get; set; } //one to many
    }
}
