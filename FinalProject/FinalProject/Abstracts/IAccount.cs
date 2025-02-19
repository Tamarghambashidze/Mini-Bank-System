using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Abstracts
{
    internal interface IAccount
    {
        public int GetAccountId(string accountNumber);
        public void OpenNewAccount(List<int> clientIds, int currencyId, int accountTypeId);
        public void ClientAccount(int accountId, int clientId);
        public void CloseAccount(int accountId);
        public void ActivateAccount(int accountId);
        public void ViewBalance(int accountId);
        public void ViewClientAccounts(int clientId);
        public void ViewCurrencies();
        public void ViewAccountTypes();
    }
}
