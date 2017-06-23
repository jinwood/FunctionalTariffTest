using System;
using System.Collections.Generic;
using System.Linq;

namespace TarrifComparison
{
    public static class CostLogic
    {
        private static int gas;
        private static int power;
        
        //not a pure function as it accesses data which can vary results
        public static void CalculateCost(string[] args, List<TarrifEntry> tarrifs, Action<List<string>> output)
        {
            int.TryParse(args[1], out power);
            int.TryParse(args[2], out gas);

            //use LINQ functions to iterate our dataset and generate a new array of strings
            //correctly formatted to display the result
            var result = 
                tarrifs.Select(x =>
                    $"{x.tariff} £{SumGasAndPower(x, power, gas)}");

            output(result.ToList());
        }

        //Pure function
        private static string SumGasAndPower(TarrifEntry tarrif, int power, int gas)
        {
            return (
                ((power * tarrif.rates.power) + (gas * tarrif.rates.gas) + tarrif.standing_charge) 
                    * Constants.VAT)
                .ToString("#.##");
        }
    }
}
