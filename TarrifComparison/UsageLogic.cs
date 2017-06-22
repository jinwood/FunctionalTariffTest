using System.Linq;

namespace TarrifComparison
{
    public static class UsageLogic
    {
        private static decimal targetMonthlySpend;

        //not a pure function as it accesses data which can vary results
        public static string[] CalculateUsage(string[] args)
        {
            if (args.Length < 3) return null;

            var tarrifName = args[1];
            var fuelType = args[2];
            decimal.TryParse(args[3], out targetMonthlySpend);

            if (fuelType != "gas" && fuelType != "power") return null;

            var data = Data.Load();

            //use LINQ functions to iterate our dataset and generate a new array of strings
            //correctly formatted to display the result
            var result = data.Where(x =>
                x.tariff == tarrifName)
                    .Select(y => $"ANNUAL_{fuelType.ToUpper()}_USAGE {CalculateSpend(y, fuelType, targetMonthlySpend)} KW/h").ToArray();

            return result;
        }

        //Pure function to calc the annual spend. The ternary on "gas" is not ideal. For a prod system I'd probably
        //write a function for each fuel type, or move the string value to a const file.
        public static string CalculateSpend(TarrifEntry tarrif, string fuelType, decimal targetMonthlySpend)
        {
            return ((((targetMonthlySpend * (fuelType == "gas" ? tarrif.rates.gas : tarrif.rates.power))
                    + tarrif.standing_charge * 12)
                    * Constants.VAT) * 12.0m).ToString("#.##");
        }
    }
}
