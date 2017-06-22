using System.Linq;

namespace TarrifComparison
{
    public static class UsageLogic
    {
        private static decimal targetMonthlySpend;

        public static string[] CalculateUsage(string[] args)
        {
            if (args.Length < 3) return null;

            var tarrifName = args[1];
            var fuelType = args[2];
            decimal.TryParse(args[3], out targetMonthlySpend);

            if (fuelType != "gas" && fuelType != "power") return null;

            var data = Data.Load();
            var result = data.Where(x =>
                x.tariff == tarrifName)
                    .Select(y => $"ANNUAL_{fuelType.ToUpper()}_USAGE £{CalculateSpend(y, fuelType, targetMonthlySpend)}").ToArray();

            return result;
        }

        public static string CalculateSpend(TarrifEntry tarrif, string fuelType, decimal targetMonthlySpend)
        {
            return ((((targetMonthlySpend * (fuelType == "gas" ? tarrif.rates.gas : tarrif.rates.power))
                    + tarrif.standing_charge * 12)
                    * Constants.VAT) * 12.0m).ToString("#.##");
        }
    }
}
