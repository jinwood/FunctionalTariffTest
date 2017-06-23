using System;
using System.Collections.Generic;
using System.Linq;

//My solution for "Tarrif Comparison" interview test
//The instructions state to program in a functional manner, which I have attempted
//There are examples of pure and high level functions, no variable mutation, and use of lambda expressions using LINQ

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
            GetCommandLogic(args).Invoke(args, tariffs, Output);
        }
        
        //higher order function, calls another function depending on the first argument passed
        static Action<string[], List<TarrifEntry>, Action<string>> GetCommandLogic(string[] args) 
        {
            return CommandLogic(args.FirstOrDefault(x => 
                x == "cost" ||
                x == "usage"));
        }

        //Higher-order function, returns the appropriate function for the given command
        static Action<string[], List<TarrifEntry>, Action<string>> CommandLogic(string command) 
        {
            switch (command.ToLower())
            {
                case "cost":
                    return CostLogic.CalculateCost;
                case "usage":
                    return UsageLogic.CalculateUsage;
                default:
                    return null;
            }
        }

        static void Output(string output)
        {
            Console.WriteLine(output);
        }
    }
}