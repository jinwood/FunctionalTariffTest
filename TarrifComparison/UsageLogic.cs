using System;
using System.Collections.Generic;
using System.Linq;

namespace TarrifComparison
{
    public static class UsageLogic
    {
        public static void CalculateUsage(string[] args, List<TarrifEntry> tarrifs, Action<string> output)
        {
            var tarrifName = args[1];
            var fuelType = args[2];
            var targetMonthlySpend = decimal.Parse(args[3]);

            Func<TarrifEntry, decimal> calculateSpend =
                (tarrif) => (targetMonthlySpend / (fuelType == "gas" ? tarrif.rates.gas : tarrif.rates.power)
                            + tarrif.standing_charge).ApplyVAT() * 12;

            output(tarrifs.Where(x =>
                x.tariff == tarrifName)
                    .Select(y => $"ANNUAL_{fuelType.ToUpper()}_USAGE {calculateSpend(y).ToString("#.##")} KW/h").FirstOrDefault().ToString());
        }
    }
}
