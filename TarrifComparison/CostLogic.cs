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
            //The alternative in C# is TryParse which outputs a decimal if successful, and 0 if not
            var power = decimal.Parse(args[1]);
            var gas = decimal.Parse(args[2]);

            //use lambda expressions to capture the outer variables used in the calling function, ie fuelType

            //in a production system, this function should probably live somewhere
            //where it can be accessed more easily like a helper class, to avoid code duplication
            Func<decimal, string> formatCost = x => x.ToString("#.##");
            Func<TarrifEntry, EnergyRate> calc = (tarrif) => new EnergyRate { Name = tarrif.tariff, Cost = tarrif.standing_charge * 12m + tarrif.rates.gas * gas };

            //use a combination of LINQ expressions, and the lambda expressions above to chain function calls
            //together to generate our output strings. Iterate over them using the ForEach extension and output to our output function
            tarrifs.Select(t => 
                calc(t)).OrderBy(o => o.Cost)
                    .ForEach(e => output($"{e.Name}: £{formatCost(e.Cost)}"));
        }
    }
}
