using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Abstracts
{
    internal interface IAnalytics
    {
        public void SortByActivity();
        public void SortByBalance();
        public void TransactionRecord(int answer);
        public void SortByCurrencies();
    }
}
