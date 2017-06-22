using System.Linq;

namespace TarrifComparison
{
    public static class CostLogic
    {
        private static int gas;
        private static int power;

        public static string[] CalculateCost(string[] args)
        {
            int.TryParse(args[1], out power);
            int.TryParse(args[2], out gas);

            var data = Data.Load();
            var result = 
                data.Select(x =>
                    $"{x.tariff} {SumGasAndPower(x, power, gas)}").ToArray();
                                
            return result;
        }

        private static string SumGasAndPower(TarrifEntry tarrif, int power, int gas)
        {
            return (((power * tarrif.rates.power) + (gas * tarrif.rates.gas) + tarrif.standing_charge) * Constants.VAT).ToString();
        }
    }
}
