using System;
using System.Linq;

namespace TarrifComparison
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = ParseArgs(args);
            if (string.IsNullOrEmpty(command)) Exit("Invalid command");

            var logicDelegate = CommandLogic(command, args);
            var result = logicDelegate.Invoke(args);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }

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

        static Func<string[],string[]> CommandLogic(string command, string[] args)
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