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
            if (command == null) Exit("Invalid command argument provided");
            command(tariffs, Console.WriteLine);
            Exit("");
        }
        

        //Higher-order function, returns the appropriate function for the given command
        static Action<List<TarrifEntry>, Action<string>> GetCommand(string[] args) 
        {
            //declare outside of the function declarations below so it is accessible by
            //both cost and usage functions
            Func<decimal, string> format2DP = x => x.ToString("#.##");

            string command = args[0].ToLower();
            switch (command)
            {
                case "cost":
                    //In this instance, I think it's worth throwing an exception over any other alternative
                    //ie C# has a TryParse method which returns either a value if successful, or 0 if not
                    //I feel falling back to 0 would potentially muddy results.
                    //This kind of input failiure could be caught before getting this far in the program
                    decimal gasUsage;
                    decimal powerUsage;
                    try
                    {
                        powerUsage = decimal.Parse(args[1]);
                        gasUsage = decimal.Parse(args[2]);
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                    //lambda function to handle cost calculation, stored temporarily in an EnergyRate type
                    Func<TarrifEntry, EnergyRate> CalculateEnergyRate = (tarrif) => new EnergyRate
                    {
                        Name = tarrif.tariff,
                        Cost = 
                            tarrif.rates.gas > 0
                            ?
                                ((tarrif.rates.gas * gasUsage)
                                +
                                (tarrif.rates.power * powerUsage)
                                + tarrif.standing_charge * 12m)*(1m + Constants.VatPercent)
                            :
                                (((tarrif.rates.power * powerUsage)
                                + tarrif.standing_charge * 12m)*(1m + Constants.VatPercent))
                    };

                    //use a combination of LINQ expressions, and the lambda expressions above to chain function calls
                    //together to generate our output strings. Iterate over them using the ForEach extension and output to our output function
                    return (tarrifs, output) => tarrifs.Select(CalculateEnergyRate)
                                                       .OrderBy(x => x.Cost)
                                                       .ToList()
                                                       .ForEach(x => output($"{x.Name}: £{format2DP(x.Cost)}"));
                case "usage":
                    var tarrifName = args[1];
                    var fuelType = args[2];
                    decimal targetMonthlySpend;
                    try
                    {
                        targetMonthlySpend = decimal.Parse(args[3]);
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                    Func<TarrifEntry, decimal> CalculateAnnualSpend =
                        (tarrif) => (targetMonthlySpend / (fuelType == "gas" ? tarrif.rates.gas : tarrif.rates.power)
                                    + tarrif.standing_charge) * 12 * (1m+Constants.VatPercent);
                    
                    return (tarrifs, output) => 
                            output(
                                tarrifs.Where(x =>
                                    x.tariff == tarrifName)
                                        .Select(y => $"ANNUAL_{fuelType.ToUpper()}_USAGE {format2DP(CalculateAnnualSpend(y))} KW/h")
                                        .FirstOrDefault()
                                        .ToString());
                default:
                    //I'm aware that null is bad in functional programming, but unsure how best to handle a situation like this
                    return null;
            }
        }
        static void Exit(string err)
        {
            Console.WriteLine(string.IsNullOrEmpty(err) 
                ? string.Empty 
                : $"An error occured- {err}");
            Console.ReadLine();
        }
    }
}