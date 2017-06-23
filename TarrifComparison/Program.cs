using System;
using System.Collections.Generic;
using System.Linq;

//My solution for "Tarrif Comparison" interview test
//The instructions state to program in a functional manner, which I have attempted
//There are examples of pure and high level functions, no variable mutation, and use of lambda expressions using LINQ
//C# is an imperitive language with some functional programming features, as such, some of the program is written 
//with OO in mind, although I've tried to keep that minimal

namespace TarrifComparison
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //the only non-pure function, does not affect any other functions due to its location
            var tariffs = Data.Load();
            //get commoand logic based off args, invoke, passing a function to handle output
            var command = GetCommand(args);
            command(tariffs, Console.WriteLine);
        }
        
        //higher order function, calls another function depending on the first argument passed

        //Higher-order function, returns the appropriate function for the given command
        static Action<List<TarrifEntry>, Action<string>> GetCommand(string[] args) 
        {
            Func<decimal, string> formatCost = x => x.ToString("£#.##");

            string command = args[0].ToLower();
            switch (command)
            {
                case "cost":
                    var powerUsage = decimal.Parse(args[1]);
                    var gasUsage = decimal.Parse(args[2]);

                    Func<TarrifEntry, EnergyRate> calc = (tarrif) => new EnergyRate
                    {
                        Name = tarrif.tariff,
                        Cost = tarrif.standing_charge * 12m + tarrif.rates.gas * gasUsage * (1m+Constants.VatPercent)
                    };

                    //use a combination of LINQ expressions, and the lambda expressions above to chain function calls
                    //together to generate our output strings. Iterate over them using the ForEach extension and output to our output function
                    return (tarrifs, output) => tarrifs.Select(calc)
                                                       .OrderBy(x => x.Cost)
                                                       .ToList()
                                                       .ForEach(x => output($"{x.Name}: {formatCost(x.Cost)}"));
                case "usage":
                    var tarrifName = args[1];
                    var fuelType = args[2];
                    var targetMonthlySpend = decimal.Parse(args[3]);

                    Func<TarrifEntry, decimal> calculateSpend =
                        (tarrif) => (targetMonthlySpend / (fuelType == "gas" ? tarrif.rates.gas : tarrif.rates.power)
                                    + tarrif.standing_charge) * 12 * (1m+Constants.VatPercent);
                    
                    return (tarrifs, output) => 
                                tarrifs.Where(x =>
                                    x.tariff == tarrifName)
                                        .Select(y => $"ANNUAL_{fuelType.ToUpper()}_USAGE {formatCost(calculateSpend(y))} KW/h")
                                        .FirstOrDefault()
                                        .ToString();
                default:
                    return null;
            }
        }
    }
}