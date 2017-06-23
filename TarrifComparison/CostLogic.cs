using System;
using System.Collections.Generic;
using System.Linq;

namespace TarrifComparison
{
    public static class CostLogic
    {
        public static void CalculateCost(string[] args, List<TarrifEntry> tarrifs, Action<string> output)
        {
            //In this instance I think its better to throw an exception due to a 
            //bad argument, than to attempt to parse it and potentionaly generate some
            //erroneous results. The calling function should probably protect against this
            var power = decimal.Parse(args[1]);
            var gas = decimal.Parse(args[2]);

            Func<decimal, string> formatCost = x => x.ToString("#.##");
            Func<TarrifEntry, EnergyRate> calc = (tarrif) => new EnergyRate { Name = tarrif.tariff, Cost = tarrif.standing_charge * 12m + tarrif.rates.gas * gas };

            tarrifs.Select(t => 
                calc(t)).OrderBy(o => o.Cost).ToList()
                    .ForEach(e => output($"{e.Name}: £{formatCost(e.Cost)}"));
        }
    }
}
