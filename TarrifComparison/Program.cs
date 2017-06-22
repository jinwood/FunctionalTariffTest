using System;
using System.Linq;

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

        static Func<string[],string[]> CommandLogic(string command)
        {
            switch (command.ToLower())
            {
                case "cost":
                    Func<string[],string[]> costLogic = CostLogic.CalculateCost;
                    return costLogic;
                case "usage":
                    Func<string[], string[]> usageLogic = UsageLogic.CalculateUsage;
                    return usageLogic;
                default:
                    return null;

            }
        }
    }
}