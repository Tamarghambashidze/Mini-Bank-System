using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Abstracts
{
    internal interface ITransaction
    {
        public void Deposit(decimal amount, int accountId);
        public void Withdraw(decimal amount, int accountId);
        public void Transaction(decimal amount, int fromAccountId, int toAccountId, int currencyId);
        public decimal CurrencyConvert(decimal amount, int accountId, int currencyId);
        public void ViewTransactionHistory(int accountId);
    }
}
