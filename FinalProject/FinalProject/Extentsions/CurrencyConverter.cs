using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Extentsions
{
    public static class CurrencyConverter
    {
        const decimal GelToUsdRate = 0.37m;
        const decimal UsdToGelRate = 1 / GelToUsdRate;

        const decimal GelToEurRate = 0.34m;
        const decimal EurToGelRate = 1 / GelToEurRate;

        const decimal UsdToEurRate = 0.92m;
        const decimal EurToUsdRate = 1 / UsdToEurRate; 

        public static decimal GelToUsd(this decimal amount) => amount * GelToUsdRate;
        public static decimal UsdToGel(this decimal amount) => amount * UsdToGelRate;

        public static decimal GelToEur(this decimal amount) => amount * GelToEurRate;
        public static decimal EurToGel(this decimal amount) => amount * EurToGelRate;

        public static decimal UsdToEur(this decimal amount) => amount * UsdToEurRate;
        public static decimal EurToUsd(this decimal amount) => amount * EurToUsdRate;

        public static decimal CurrencyConvert(this int id, decimal amount, int currencyId)
        {
            decimal result = default;
            if (currencyId == id)
                result = amount;
            else
            {
                if (currencyId == 1 && id == 2)
                    result = amount.GelToUsd();
                else if (currencyId == 2 && id == 1)
                    result = amount.UsdToGel();
                else if (currencyId == 1 && id == 3)
                    result = amount.GelToEur();
                else if (currencyId == 3 && id == 1)
                    result = amount.EurToGel();
                else if (currencyId == 2 && id == 3)
                    result = amount.UsdToEur();
                else if (currencyId == 3 && id == 2)
                    result = amount.EurToUsd();
                else
                    Console.WriteLine("Incorrect input");
            }
            return result;
        }
    }
}
