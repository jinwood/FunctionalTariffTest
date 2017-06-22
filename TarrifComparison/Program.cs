using System;
using System.Linq;

//My solution for "Tarrif Comparison" interview test
//The instructions state to program in a functional manner, which I have attempted

namespace TarrifComparison
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var command = ParseArgs(args);
            if (string.IsNullOrEmpty(command)) Exit("Invalid command");

            var result = CommandLogic(command).Invoke(args);

            if (result == null) Exit("Error parsing user input");

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
            Environment.Exit(0);

        }

        //pure function as it either returns "cost", "usage" or empty string
        static string ParseArgs(string[] args) 
        {
            return args.FirstOrDefault(x => 
                x == "cost" ||
                x == "usage");
        }

        static void Exit(string message)
        {
            Console.WriteLine(
                string.IsNullOrEmpty(message)
                    ? "Exiting application"
                    : $"Exiting application - {message}");

            Environment.Exit(0);
        }

        //Higher-order function to return the appropriate function for the given command
        static Func<string[],string[]> CommandLogic(string command) 
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
    }
}